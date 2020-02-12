# Use AAD Pod Identity and FlexVolume driver to inject Secrets into the Claims Web API container

This sub-project describes the steps for configuring AAD Pod Identity and Azure FlexVolume driver for retreiving secrets from Azure Key Vault and mounting them onto the file system within the application (Claims Web API) container.

In production environments, confidential data such as database user names and passwords are stored in a secure location such as Azure Key Vault and then injected into the application container at runtime.

In this project, the Azure SQL Database **Connection String** will be stored in Azure Key Vault.  The FlexVolume driver will be used to retrieve the connection string from Key Vault and mount it as a file on the container's file system.  

Azure Active Directory (AAD) Pod Identity enables Kubernetes applications to access cloud resources securely using managed identities and service principals.  Without any code modifications, containerized applications can access any resource on Azure cloud that uses AAD as an Identity provider.

**Functional Diagram:**

**Prerequisites:**
1. Readers are required to complete Sections A thru G in the [parent project](https://github.com/ganrad/aks-aspnet-sqldb-rest) before proceeding with the hands-on labs in this sub-project.

Readers are advised to go thru the following on-line resources before proceeding with the hands-on sections.
- [Azure AAD Pod Identity](https://github.com/Azure/aad-pod-identity)
- [Azure/Kubernetes Key Vault FlexVolume Driver](https://github.com/Azure/kubernetes-keyvault-flexvol)

## A. Deploy pre-requisite Azure resources
**Approx. time to complete this section: 10 minutes**

The Azure SQL Database *Connection String* will be stored in an Azure Key Vault. To provision a Key Vault and save the connection string as a **Secret**, refer to the steps below.

1. Login to the Azure Portal.

   Login to the [Azure Portal](https://portal.azure.com) using your credentials.

2. Provision an Azure Key Vault and create a Secret.

   Refer to the tutorial below to provision an Azure Key Vault and create a *Secret* to store the Azure SQL Database connection string.
   - [Set and retrieve a secret from Azure key Vault using the Azure Portal](https://docs.microsoft.com/en-us/azure/key-vault/quick-create-portal)

   Create a secret for the SQL database connection string in the Key Vault.  Refer to the table below.

   Name | Value | Description
   -----|-------|------------

   sqldbconn | Server=tcp:SQL_SRV_PREFIX.database.windows.net;Initial Catalog=ClaimsDB;Persist Security Info=False;User ID=SQL_USER_ID;Password=SQL_USER_PWD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; | Substitute actual values for SQL_SRV_PREFIX, SQL_USER_ID & SQL_USER_PWD 

## B. Deploy Azure Key Vault FlexVolume Driver on AKS cluster
**Approx. time to complete this section: 10 minutes**

Follow the steps below to provision the FlexVolume driver on the AKS cluster.

1. Login into the Linux VM via SSH.
  
   ```bash
   # ssh into the VM. Substitute the public IP address for the Linux VM in the command below.
   $ ssh labuser@x.x.x.x
   #
   ```
2. Deploy Key Vault FlexVolume Driver on the AKS Cluster

   ```bash
   # Deploy the KV FlexVolume Driver on the AKS cluster.
   $ kubectl create -f https://raw.githubusercontent.com/Azure/kubernetes-keyvault-flexvol/master/deployment/kv-flexvol-installer.yaml
   #
   # Verify keyvault-flexvolume pods are running on each node.
   # By default, the flex volume pods are deployed in the 'kv' namespace.
   $ kubectl get pods -n kv
   #
   ```

## C. Install AAD Pod Identity resources on AKS Cluster
**Approx. time to complete this section: 45 minutes**

AAD Pod Identity consists of two key components and custom resources.  The two core components are described below.
- Managed Identity Controller (MIC)
  The MIC is a custom Kubernetes resource that watches for changes to Pods, Identities and Bindings through the Kubernetes API Server.   When it detects a change, the MIC adds or deletes assigned identities as required.
- Node Managed Identity (NMI)
  The NMI component is responsible for intercepting a Service Principal Token request sent by a Pod to an MSI endpoint, retrieving a matching Azure Identity from MIC and then making an [ADAL]() request to get a token for the client id.  The token is then returned to the Pod (FlexVolume driver).

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
   # - name => Managed Identity name
   # Important: Save the values of 'clientId' and 'id' from the json output.
   #
   $ az identity create -g <resource-group> -n <name> -o json
   #
   ```
4. Assign AKS cluster SPN Role.

   If the Service Principal used for the AKS cluster was created separately (not automatically assigned at cluster creation), assign it the **Managed Identity Operator** role.

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
   # - sp-id => Service Principal ID (output of previous command)
   # - managed-identity-id => value of 'id' from json output in step (3) above.
   #
   # az role assignment create --role "Managed Identity Operator" --assignee <sp-id> --scope <managed-identity-id>
   #
   ```

## D. Use Azure DevOps to build and deploy the containerized Function Applications

This section is left as an exercise to the readers and workshop attendees.  Refer to the parent project to implement the Azure DevOps build, release and delivery pipelines.

**Hint:**
- Use the `Dockerfile` in the build pipeline to build the Function application container images.  
- Use the `Helm Charts` in the release pipeline to deploy the containerized Function applications on AKS.

**--The END.**
