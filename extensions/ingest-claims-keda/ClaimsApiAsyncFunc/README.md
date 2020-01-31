# Build and deploy this Azure Function Application locally on the Linux VM

Follow the steps below for building and running the Function application locally on the Linux VM.

1. Update the `./local.settings.json` file with appropriate values as shown in the table below.

   Function Parameter Name | Value | Description
   ----------------------- | ----- | -----------
   ClaimsPostQueue | claims-req-queue | This is the queue the **GetHttpApiFunctions** application puts/delivers the Claims transactions to.  The function **PostClaimsHttpApiToSbQueue** puts/saves the HTTP response returned by a HTTP POST request to the backend Claims Web API.  The backend Claims Web API returns a Claims record (JSON) after saving the record in the backed data store (Azure SQL DB).
   ClaimsDelQueue | claims-del-queue | This is the queue the **AsyncSbQueueApiFunc** application gets/reads the Claims transactions from.  The function **GetSbQueueCallClaimsDelApi** reads a Claims record from this queue and sends a DELETE HTTP request to the backend Claims Web API.  The backend Claims Web API deletes the Claims record from the backend data store (Azure SQL DB).
   AzServiceBusConnection | "Endpoint=" | Specify the value of Azure Service Bus namespace **Connection String**.
   ClaimsApiHost | claims-api-svc.development | Kubernetes DNS **Service** name where the Claims Web API is exposed.
   ClaimsApiEndpoint | /api/v1/claims | Context path of the backend Claims Web API.

2. Run the Function Application.

   Run the following commands in a Linux terminal window.
   ```bash
   # Switch to the Function Application 'ClaimsApiAsyncFunct' directory
   $ cd $HOME/git-repos/aks-aspnet-sqldb-rest/extensions/ingest-claims-keda/ClaimsApiAsyncFunc
   #
   # Run the function application
   # Upon successful startup, the Function runtime should display the URL to access the Claims
   # HTTP end-point.
   $ func start
   #
   # Leave this terminal window open.

[Return back to ingest-claims-keda extension](../#c-build-and-test-the-azure-application-locally-on-the-linux-vm)

