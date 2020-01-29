# Build and deploy Azure Functions for ingesting Claims records at scale
This sub-project describes the steps for ingesting Claims records into the backend Azure SQL Server Database using **Azure Functions** serverless applications.

The Functions application modules are implemented in .NET Core and Nodejs respectively. The Azure Functions runtime itself is packaged within a Linux container and deployed on AKS.

A brief description of the Functions applications is provided below.
- [ClaimsApiSyncFunc Application](./ClaimsApiAsyncFunc)
  This .NET Core application comprises of two Functions (modules) written in C#.
  - **GenHttpApiFunctions**:
    This Function exposes a generic REST API for processing new medical Claims.  Upon receiving medical Claims transactions, this function stores them in a persistent Azure Service Bus Queue. This step ensures the received Claims transactions are processed in a guaranteed and reliable manner.
  - **AsyncSbQueueApiFunc**:
    This Function reads medical Claims transactions from an Azure Service Bus Queue and then invokes the Claims REST API end-points implemented in the parent project. In the event the backend application is down or unreachable, the Claims transactions persist in the service bus queue.

- [ClaimsAsyncApiFunc Application](./ClaimsAsyncApiFunc)
  This Nodejs application comprises of one Function (module) written in Javascript.
  - **GetSbQCallWebApi**:
    This Function reads the medical Claims transactions from the Azure Service Bus Queue and then invokes the Claims REST API end-point implemented in the parent project.  This step executes an atomic transaction and ensures the Claims records are successfully delivered to the backend application.  In the event the backend application is down or unreachable, the Claims transactions remain in the service bus queue.

The solution leverages *Kubernetes Event Driven Auto-scaling* (KEDA) on AKS.  Kubernetes based Functions provides the Functions runtime in a Docker container with event-driven scaling through KEDA.

**Prerequisites:**
1. Readers are required to complete Sections A thru G in the [parent project](https://github.com/ganrad/aks-aspnet-sqldb-rest) before proceeding with the hands-on labs in this sub-project.

Readers are advised to go thru the following on-line resources before proceeding with the hands-on sections.
- [Azure Functions Documentation](https://docs.microsoft.com/en-us/azure/azure-functions/)
- [Kubernetes based event driven auto-scaling - KEDA](https://keda.sh/)
- [KEDA on AKS](https://docs.microsoft.com/en-us/azure/azure-functions/functions-kubernetes-keda)
