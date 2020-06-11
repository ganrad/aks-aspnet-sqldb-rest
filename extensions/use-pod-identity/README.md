# Use AAD Pod Identity and Key Vault FlexVolume driver to inject Secrets into applications on AKS

This project extension describes the steps for configuring AAD Pod Identity and Azure FlexVolume driver for retreiving secrets from Azure Key Vault and mounting them onto the file system within the Claims Web API application container.

In production environments, confidential data such as database user names and passwords are stored in a secure location such as Azure Key Vault and then injected into the application container at runtime.

In this project, the Azure SQL Database **Connection String** will be stored in Azure Key Vault.

The **Azure Key Vault FlexVolume Driver** will be used to retrieve the connection string from Key Vault and mount it as a file on the container's file system.  

**Azure Active Directory (AAD) Pod Identity** enables Kubernetes applications to access cloud resources securely using managed identities and service principals.  Without any code modifications, containerized applications can access any resource on Azure cloud that uses AAD as an Identity provider.

**Functional Diagram:**

Refer to the architecture diagram [here](https://docs.microsoft.com/en-us/azure/aks/operator-best-practices-identity#use-pod-identities).

**Prerequisites:**
1. Readers are required to complete Sections A thru G in the [parent project](https://github.com/ganrad/aks-aspnet-sqldb-rest) before proceeding with the hands-on labs in this sub-project.

Readers are advised to go thru the following on-line resources as needed.
- [Azure AAD Pod Identity](https://github.com/Azure/aad-pod-identity)
- [Azure/Kubernetes Key Vault FlexVolume Driver](https://github.com/Azure/kubernetes-keyvault-flexvol)

## A. Deploy an Nginx Ingress Controller on the AKS Cluster
**Approx. time to complete this section: 10 minutes**

An ingress controller provides reverse proxy, configurable traffic routing and TLS termination for applications (Web Services) deployed on the AKS cluster.  The ingress controller acts as a single entry point for all external HTTP traffic inbound into the cluster and intelligently routes the calls to the respective application service end-points.

In this sub-project, we will deploy an [Nginx Ingress Controller](https://github.com/kubernetes/ingress-nginx) and configure it to intercept and route HTTP calls to the Claims API microservice.

Follow the steps below to provision the Nginx Ingress Controller on the AKS cluster.  You can also refer to the steps in the AKS documentation [here](https://docs.microsoft.com/en-us/azure/aks/ingress-basic).

1. Login into the Linux VM via SSH.
  
   ```bash
   # ssh into the VM. Substitute the public IP address for the Linux VM in the command below.
   $ ssh labuser@x.x.x.x
   #
   ```

2. Install the Nginx Ingress Controller.

   ```bash
   # Create a k8s namespace for the Ingress Controller
   $ kubectl create namespace ingress-basic
   #
   # Add the official stable repository for k8s helm charts
   $ helm repo add stable https://kubernetes-charts.storage.googleapis.com/
   #
   # Use Helm to deploy the Nginx Ingress Controller on AKS
   $ helm install nginx-ingress stable/nginx-ingress \
     --namespace ingress-basic \
     --set controller.replicaCount=2 \
     --set controller.nodeSelector."beta\.kubernetes\.io/os"=linux \
     --set defaultBackend.nodeSelector."beta\.kubernetes\.io/os"=linux
   #
   # Verify the ingress controller Pods are 'running'
   # There should 2 ingress controller pods and one backend pod
   #
   $ kubectl get pods -n ingress-basic
   #
   # Retreive the Ingress Controller external interface (ALB) Public IP address.
   # Important: Make a note of the Public IP address listed under the 'EXTERNAL-IP' column.
   # You will be using this IP address to access the Claims Web API's later. 
   #
   $ kubectl get svc -n ingress-basic
   #
   ```

## B. Deploy Azure Key Vault FlexVolume Driver on AKS cluster
**Approx. time to complete this section: 10 minutes**

The Azure Key Vault FlexVolume driver retrieves secrets from Azure Key Vault and mounts them into separate files on the application container's file system.

Follow the steps below to provision the FlexVolume driver on the AKS cluster.

1. Deploy Azure Key Vault FlexVolume Driver on the AKS Cluster

   ```bash
   # Deploy the KV FlexVolume Driver on the AKS cluster.
   $ kubectl create -f https://raw.githubusercontent.com/Azure/kubernetes-keyvault-flexvol/master/deployment/kv-flexvol-installer.yaml
   #
   # Verify keyvault-flexvolume pods are running on each node.
   # By default, the flex volume pods are deployed in the 'kv' namespace.
   $ kubectl get pods -n kv
   #
   ```

## C. Install AAD Pod Identity components on AKS Cluster
**Approx. time to complete this section: 30 minutes**

AAD Pod Identity consists of two key components and custom resources.  The two core components are briefly described below.

- **Managed Identity Controller (MIC)**

  The MIC is a custom Kubernetes resource that watches for changes to Pods, Identities and Bindings through the Kubernetes API Server.   When it detects a change, the MIC adds or deletes assigned identities as required.
- **Node Managed Identity (NMI)**

  The NMI component is responsible for intercepting a Service Principal Token request sent by a Pod to an MSI endpoint, retrieving a matching Azure Identity from MIC and then making an [ADAL](https://docs.microsoft.com/en-us/azure/active-directory/azuread-dev/active-directory-authentication-libraries) request to get a token for the client id.  The token is then returned to the Pod (FlexVolume driver).

Follow the steps below to deploy AAD Pod Identity components and custom resources.

1. (If you haven't already) Login into the Linux VM via SSH.
  
   ```bash
   # ssh into the VM. Substitute the public IP address for the Linux VM in the command below.
   $ ssh labuser@x.x.x.x
   #
   ```

2. Deploy MIC, NMI components and custom resources on the AKS cluster.

   ```bash
   # Deploy MIC, NMI components and custom resources for a non-RBAC enabled AKS cluster.
   #
   $ kubectl apply -f https://raw.githubusercontent.com/Azure/aad-pod-identity/master/deploy/infra/deployment.yaml
   #
   # For AKS clusters, deploy the MIC and AKS add-on exception
   $ kubectl apply -f https://raw.githubusercontent.com/Azure/aad-pod-identity/master/deploy/infra/mic-exception.yaml
   #
   # Verify MIC (Deployments) and NMI (Daemon sets) pods have been deployed on the cluster.
   # The pods will be deployed in the 'default' namespace.  There should be as many instances of
   # NMI and MIC pods running as there are nodes in the cluster (1 pod on each node).
   #
   $ kubectl get pods -n default -o wide
   #
   ```

3. Create an Azure Managed Identity.

   Save the values of `clientId` and `id` from the command output.

   ```bash
   # Create a managed identity.
   # Make sure you are logged in to your Azure account and have configured the correct subscription.
   # Substitute correct values for the following parameters:
   # - resource-group => Azure resource group 
   # - name => Managed Identity name eg., claims-api-mid
   #
   $ az identity create -g <resource-group> -n <name> -o json
   # Important: Save the json output of the above command in a file !!
   # We will need to use 'clientId', 'principalId', 'id' and other values from the json output in 
   # the subsequent steps below.
   #
   ```
4. Assign AKS cluster SPN Role.

   If the Service Principal used for the AKS cluster was created separately (not automatically assigned at cluster creation), assign it the **Managed Identity Operator** role for (scope of) the managed identity.

   ```bash
   # Retrieve the AKS cluster service principal id.
   # Substitute correct values for the following parameters:
   # - resource-group => Azure resource group 
   # - name => AKS cluster name
   #
   $ az aks show -g <resourcegroup> -n <name> --query servicePrincipalProfile.clientId -o tsv
   #
   # Assign the 'Managed Identity Operator' role to the AKS cluster service principal on the
   # managed identity.
   # Substitute correct values for the following parameters:
   # - sp-id => AKS Service Principal ID (output of previous command)
   # - managed-identity-id => value of 'id' from json output in step (3) above.
   #
   $ az role assignment create --role "Managed Identity Operator" --assignee <sp-id> --scope <managed-identity-id>
   #
   ```

## D. Deploy Azure Key Vault and Custom Resources
**Approx. time to complete this section: 45 minutes**

The Azure SQL Database *Connection String* will be stored in an Azure Key Vault. To provision a Key Vault and save the connection string as a **Secret**, refer to the steps below.

1. Login to the Azure Portal.

   Login to the [Azure Portal](https://portal.azure.com) using your credentials.

2. Provision an Azure Key Vault and create a Secret.

   Refer to the tutorial below to provision an Azure Key Vault and create a *Secret* to store the Azure SQL Database connection string.
   - [Set and retrieve a secret from Azure key Vault using the Azure Portal](https://docs.microsoft.com/en-us/azure/key-vault/quick-create-portal)

   Create a secret for the SQL database connection string in the Key Vault.  Refer to the table below.

   | Secret Name | Value | Description |
   | ----------- | ----- | ----------- |
   sqldbconn | Server=tcp:{SQL_SRV_PREFIX}.database.windows.net;Initial Catalog=ClaimsDB;Persist Security Info=False;User ID={SQL_USER_ID};Password={SQL_USER_PWD};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; | The Azure SQL Database connection string. Value of 'ConnectionStrings.SqlServerDb' parameter in `appsettings.json` file.  **IMPORTANT:** Make sure to substitute correct values for SQL_SRV_PREFIX, SQL_USER_ID & SQL_USER_PWD in the connection string. |

3. Assign Azure Identity Roles.

   Assign Azure Identity role assignments to the managed identity so it can read the Key Vault instance and access it's contents.

   ```bash
   # Switch to the Linux VM terminal window.
   #
   # Assign 'reader' role to the managed identity for the Key Vault
   # Specify correct values for the following parameters:
   # - principal-id => 'principalId' value of the managed identity created in Section B step 3
   # - subscriptionid => Azure Subscription ID containing the Key Vault resource
   # - resourcegroup => Resource group containing the Key Vault
   # - keyvaultname => Name of the Key Vault instance eg., claims-api-kv
   #
   $ az role assignment create --role Reader --assignee <principal-id> --scope /subscriptions/<subscriptionid>/resourcegroups/<resourcegroup>/providers/Microsoft.KeyVault/vaults/<keyvaultname>
   #
   # Set policy to access secrets from the Key Vault
   # Specify correct values for the following parameters:
   # - kv-name => Key Vault name
   # - client-id => 'clientId' value of the managed identity created in Section B step 3
   #
   $ az keyvault set-policy -n <kv-name> --secret-permissions get --spn <client-id>
   #
   ```

4. Create a new Kubernetes namespace for deploying Claims Web API with AAD Pod Identity.
  
   ```bash
   # Create a new Kubernetes namespace 'dev-claims-podid' for deploying the Claims Web API application with AAD Pod Identity 
   $ kubectl create namespace dev-claims-podid 
   #
   ```

5. Install Azure Pod Identity Kubernetes resource.

   This custom Kubernetes resource contains the ID's of the Azure Managed Identity.

   ```bash
   # Switch to the 'use-pod-identity' directory.
   $ cd ./extensions/use-pod-identity
   #
   # Edit the Pod Identity Kubernetes manifest file `./k8s-resources/aadpodidentity.yaml`, update 
   # values for the following two attributes and then save the file.
   # - resourceID => 'id' attribute value of the managed identity created in Section B step 3
   # - clientID => 'clientId' value of the managed identity created in Section B step 3
   #
   # Deploy the pod identity custom resource on AKS
   $ kubectl apply -f ./k8s-resources/aadpodidentity.yaml -n dev-claims-podid
   #
   # Verify the Azure Identity resource got created in Kubernetes
   $ kubectl get azureidentity -n dev-claims-podid  
   #
   ```

6. Install the Azure Pod Identity Binding Kubernetes resource.
  
   This custom Kubernetes resource binds the Claims Web API Pod (via the 'selector') with the Azure Managed Identity.

   ```bash
   # Deploy the pod identity binding custom resource on AKS
   $ kubectl apply -f ./k8s-resources/aadpodidentitybinding.yaml -n dev-claims-podid
   #
   # Verify the Azure Identity Binding resource got created in Kubernetes
   $ kubectl get azureidentitybinding -n dev-claims-podid  
   #
   ```

## E. Deploy the Claims Web API application
**Approx. time to complete this section: 20 minutes**

Execute the steps below to deploy the Claims Web API application on AKS.

1. Update the Helm chart for the Claims Web API application.

   Update the Helm chart `./claims-api/values.yaml` file by referring to the table below.

   Parameter Name | Value | Description
   -------------- | ----- | -----------
   image.repository | acr-name.azurecr.io/claims-api | Specify the ACR name and image name for the Claims Web API container image.
   image.tag | latest | Specify the claims-api image tag name.
   kv.secretName | sqldbconn | Specify the name of the Azure Key Vault **secret** containing the Azure SQL Database connection string.
   kv.resourceGroup | resource-group | Specify the name of the resource group containing the Azure Key Vault.
   kv.subscriptionId | subscription-id | Specify the Azure subscription in which the Key Vault is provisioned.
   kv.tenantId | tenant-id | Specify the AAD Tenant in which the Key Vault is provisioned.

   ```bash
   # (If you have not already) Switch to the 'use-pod-identity' extension directory.
   $ cd ./extensions/use-pod-identity
   #
   # Edit the './claims-api/values.yaml` file by referring to the table above.
   #
   ```

2. Deploy the Claims Web API application.

   ```bash
   # Use Helm to install the Claims Web API application in namespace 'dev-claims-podid'
   $ helm install ./claims-api/ --namespace dev-claims-podid --name claims-api-podid
   #
   # Verify the Claims Web API pod is running
   $ kubectl get pods -n dev-claims-podid
   #
   ```

3. Access the Claims Web API application.

   Retrieve the Public IP address of the Nginx ingress controller. See the command snippet below.
   
   ```bash
   # Get the ALB IP address for the Nginx Ingress Controller service.
   # The ALB Public IP address should be listed under column 'EXTERNAL-IP' in the command output.
   #
   $ kubectl get svc -n ingress-basic
   #
   ```

   Access the Claims Web API service using a browser eg., http://[ALB Public IP]/api/v1/claims.

Congrats! In this project extension, you installed **Azure Key Vault FlexVolume** driver and **AAD Pod Identity** components on the AKS cluster.  Next, you secured the SQL Connection String by storing it in an Azure Key Vault. Finally, you configured the Claims Web API application to use FlexVolume driver and managed Pod Identity to retrieve the SQL Connection String from Key Vault and connect to the SQL Database. 
