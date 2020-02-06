# Build and deploy this Azure Function Application locally on the Linux VM

Follow the steps below for building and running the Function application locally on the Linux VM.

1. Update the `./local.settings.json` file with appropriate values as shown in the table below.

   Function Parameter Name | Value | Description
   ----------------------- | ----- | -----------
   ClaimsPostQueue | claims-req-queue | This is the queue the **GetHttpApiFunctions** application puts/delivers the Claims transactions to.  The function **PostClaimsHttpApiToSbQueue** puts/saves the Claims record/transaction into this queue.
   ClaimsDelQueue | claims-del-queue | This is the queue the **AsyncSbQueueApiFunc** application gets/reads the Claims transactions from.  The function **GetSbQueueCallClaimsDelApi** reads a Claims record from this queue and sends a HTTP DELETE request to the backend Claims Web API.  The backend Claims Web API deletes the Claims record from the persistent data store (Azure SQL DB).
   AzServiceBusConnection | "Endpoint=" | Specify the value of Azure Service Bus namespace **Connection String**.
   ClaimsApiHost | x.x.x.x | Azure Load Balancer **Service** IP address where the Claims Web API ('claims-api-svc') is exposed.
   ClaimsApiEndpoint | /api/v1/claims | Context path of the backend Claims Web API.

2. Run the Function Application.

   Run the following commands in a Linux terminal window.
   ```bash
   # Switch to the Function Application 'ClaimsApiAsyncFunc' directory
   $ cd $HOME/git-repos/aks-aspnet-sqldb-rest/extensions/ingest-claims-keda/ClaimsApiAsyncFunc
   #
   # Run the function application
   # Upon successful startup, the Function runtime should display the URL to access the Claims
   # HTTP end-point.
   $ func start
   #
   # Leave this terminal window open.

[Return back to ingest-claims-keda extension](../#c-build-and-test-the-azure-function-applications-locally-on-the-linux-vm)

