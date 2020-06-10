# Use AAD Pod Identity and Azure Managed Identity to access Azure SQL Server Database

This extension project describes the steps for configuring the Claims Web API application to use **AAD Pod Identity** and **Managed Identity**.

*AAD Pod Identity* enables Kubernetes applications to access cloud resources securely using managed identities and service principals. Without any code modifications, containerized applications can access any resource on Azure cloud that uses AAD as an Identity provider.

*Managed Identity* makes applications more secure by eliminating secrets such as credentials in connection strings. 

In this extension project, you will work on completing the following tasks.

- Deploy *AAD Pod Identity* components on AKS cluster
- Configure Azure SQL Database to allow *Managed Identity** access to resources (eg., Tables)
- Configure the Claims Web API application to retrieve data from Azure SQL Database Tables using Azure *Managed Identity*  

**Functional Diagram:**

Refer to the architecture diagram [here](https://docs.microsoft.com/en-us/azure/aks/operator-best-practices-identity#use-pod-identities).

**Prerequisites:**
1. Readers are required to complete Sections A thru G in the [parent project](https://github.com/ganrad/aks-aspnet-sqldb-rest) before proceeding with the hands-on labs in this project.

Readers are advised to refer to the following on-line resources as needed.
- [Azure Active Directory] (https://docs.microsoft.com/en-us/azure/active-directory/)
- [Azure AAD Pod Identity](https://github.com/Azure/aad-pod-identity)
- [Managed Identities for Azure resources](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview)
- [Use Managed Identities in AKS](https://docs.microsoft.com/en-us/azure/aks/use-managed-identity)

## A. Install AAD Pod Identity components on AKS Cluster
**Approx. time to complete this section: 30 minutes**

AAD Pod Identity consists of two key components and custom resources.  The two core components are described below.
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
   # For AKS Clusters, deploy the MIC and AKS add-on exception
   $ kubectl apply -f https://raw.githubusercontent.com/Azure/aad-pod-identity/master/deploy/infra/mic-exception.yaml
   #
   # Verify MIC (Deployments) and NMI (Daemon sets) pods have been deployed on the cluster.
   # The pods will be deployed in the 'default' namespace.  There should be as many instances of
   # NMI pods running as there are nodes in the cluster (1 pod on each node).
   #
   $ kubectl get pods -n default -o wide
   #
   ```

3. Create an Azure Managed Identity in the **Node** Resource Group.

   Save the values of `clientId` and `id` from the command output.

   ```bash
   # Create a managed identity in the AKS 'Node' Resource Group. The resource group name should have a 'MC_'
   # prefix..
   # Make sure you are logged in to your Azure account and have configured the correct subscription.
   # Substitute correct values for the following parameters:
   # - node-resource-group => Azure Kubernetes Service Node resource group 
   # - name => Managed Identity name eg., claims-api-mid
   #
   $ az identity create -g <node-resource-group> -n <name> -o json
   # Important: Save the json output of the above command in a file !! We will need to use 'clientId',
   # 'id' and other values from the json output in the subsequent commands below.
   #
   ```

4. Assign Roles to AKS cluster Service Principal.

   Assign the AKS Service Principal, **Managed Identity Operator** and **Virtual Machine Contributor** roles for (scope of) the **Node** or **Cluster** Resource Group.

   ```bash
   # Retrieve the AKS cluster service principal id.
   # Substitute correct values for the following parameters:
   # - resource-group => Azure Kubernetes Service resource group 
   # - name => AKS cluster name
   #
   $ az aks show -g <resource-group> -n <name> --query servicePrincipalProfile.clientId -o tsv
   #
   # Assign the 'Managed Identity Operator' role to the AKS cluster service principal on the
   # 'Node' resource group
   # Substitute correct values for the following parameters:
   # - sp-id => AKS Service Principal ID (output of previous command)
   # - subscription-id => Azure Subscription ID
   # - node-resource-group => Azure Kubernetes Service Node resource group 
   #
   $ az role assignment create --role "Managed Identity Operator" --assignee <sp-id> --scope /subscriptions/<subscription-id>/resourcegroups/<node-resource-group>
   $ az role assignment create --role "Virtual Machine Contributor" --assignee <sp-id> --scope /subscriptions/<subscription-id>/resourcegroups/<node-resource-group>
   #
   ```

## B. Configure Azure SQL Database
**Approx. time to complete this section: 45 minutes**

To allow **Managed Identity** access to Azure SQL Database resources (eg., Tables), a managed identity user has to be created in the database and granted specific roles.  This would allow the managed identity user to manipulate data in the database tables.

1. Login to the Azure Portal.

   Login to the [Azure Portal](https://portal.azure.com) using your credentials.

2. Configure Active Directory Admin account for Azure SQL Server.

   In order to create a managed identity **user** in Azure SQL, an Azure Active Directory **Account** is required. Use Azure Portal to set the AD Admin Account for Azure SQL by following the steps below.

   Access the Azure SQL Server instance and click on **Active Directory admin** blade as shown in the screenshot below

   ![alt tag](./images/B-01.PNG)

   Click on **Set admin**.

   In the **Add admin** window, search for your AD Account Name and click **Select** as shown in the screenshot below.

   ![alt tag](./images/B-02.PNG)

3. Create Azure SQL Managed Identity User and Grant roles.

   In Azure SQL Server service, access the **SQL databases** blade and click on the **Database** name as shown below.

   ![alt tag](./images/B-03.PNG)

   Select **Query editor** blade and then click on your account name under **Active Directory authentication** as shown in the screenshot below.

   ![alt tag](./images/B-04.PNG)

   Run the following SQL-Transact commands in the **Query** panel/window.  Click **Run**.

   ```bash
   # IMPORTANT:
   # Substitute the correct value for the managed identity name.
   # - managed-id-name => Managed Identity name eg., claims-api-mid.
   #
   CREATE USER [managed-id-name] FROM EXTERNAL PROVIDER;
   ALTER ROLE db_datareader ADD MEMBER [managed-id-name];
   ALTER ROLE db_datawriter ADD MEMBER [managed-id-name];
   ALTER ROLE db_ddladmin ADD MEMBER [managed-id-name];
   GO
   ```

   See screenshot below.

   ![alt tag](./images/B-05.PNG)

## C. Deploy AAD Pod Identity resoureces on AKS

1. Create a new Kubernetes namespace for deploying Claims Web API application;
  
   ```bash
   # Create a new Kubernetes namespace 'dev-claims-mid' for deploying the Claims Web API application with 
   # AAD Pod Identity and Managed Identity
   #
   $ kubectl create namespace dev-claims-mid 
   #
   ```

2. Install Azure Pod Identity Kubernetes resource.

   This custom Kubernetes resource contains the ID's of the Azure Managed Identity.

   ```bash
   # Switch to the 'use-pod-identity-mid' directory.
   $ cd ./extensions/use-pod-identity-mid
   #
   # Edit the Pod Identity Kubernetes manifest file `./k8s-resources/azureIdentity.yaml`, update 
   # values for the following two attributes and then save the file.
   # - ResourceID => 'id' attribute value of the managed identity created in Section A step 3
   # - ClientID => 'clientId' value of the managed identity created in Section A step 3
   #
   # Deploy the pod identity custom resource on AKS
   $ kubectl apply -f ./k8s-resources/azureIdentity.yaml -n dev-claims-mid
   #
   # Verify the Azure Identity resource got created in Kubernetes
   $ kubectl get azureidentity -n dev-claims-mid  
   #
   ```

6. Install the Azure Pod Identity Binding Kubernetes resource.
  
   This custom Kubernetes resource binds the Claims Web API Pod (via the 'selector') with the Azure Managed Identity.

   ```bash
   # Deploy the pod identity binding custom resource on AKS
   $ kubectl apply -f ./k8s-resources/azureIdentityBinding.yaml -n dev-claims-mid
   #
   # Verify the Azure Identity Binding resource got created in Kubernetes
   $ kubectl get azureidentitybinding -n dev-claims-mid  
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

Congrats! In this extension, you installed Azure **FlexVolume** driver and **AAD Pod Identity** components.  Finally, you configured the Claims Web API application to use FlexVolume driver and the managed Pod Identity to retrieve SQL Connection String from an Azure Key Vault. 
