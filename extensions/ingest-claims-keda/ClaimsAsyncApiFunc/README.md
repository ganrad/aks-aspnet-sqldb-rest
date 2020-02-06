# Build and deploy this Azure Function Application locally on the Linux VM

Follow the steps below for building and running the Function application locally on the Linux VM.

1. Update the `./local.settings.json` file with appropriate values as shown in the table below.

   Function Parameter Name | Value | Description
   ----------------------- | ----- | -----------
   AzServiceBusInputQueueName | claims-req-queue | This is the queue the **GetSbQCallWebApi** Function reads/gets the Claims transactions from.  The function then calls the Claims Web API and invokes an HTTP POST request.  
   AzServiceBusOutputQueueName | claims-del-queue | This is the queue the **GetSbQCallWebApi** Function puts/saves the Claims transactions to.  The HTTP POST response (JSON) returned by the Claims Web API is saved in this queue.
   AzServiceBusConnection | "Endpoint=" | Specify the value of Azure Service Bus namespace **Connection String**.
   ClaimsApiHost | x.x.x.x | Azure Load Balancer **Service** IP address where the backend Claims Web API ('claims-api-svc') is exposed.
   ClaimsApiEndpoint | /api/v1/claims | Context path of the backend Claims Web API.
   ClaimsApiMethod | POST | HTTP method invoked by the **GetSbQCallWebApi** Function on the backend Claims Web API.

2. Run the Function Application.

   Run the following commands in a Linux terminal window.
   ```bash
   # Switch to the Function Application 'ClaimsAsyncApiFunc' directory
   $ cd $HOME/git-repos/aks-aspnet-sqldb-rest/extensions/ingest-claims-keda/ClaimsAsyncApiFunc
   #
   # Run the function application
   $ func start
   #
   # Leave this terminal window open.

[Return back to ingest-claims-keda extension](../#c-build-and-test-the-azure-function-applications-locally-on-the-linux-vm)

