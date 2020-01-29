# Extensions to the **Claims API** Microservice
The extensions below explore advanced features of AKS (Kubernetes) and additional open source solutions. Each extension (or sub-project) describes a particular use case and details the steps for implementing a potential solution using an advanced feature/capability.

- [Ingest Claims records using KEDA](./ingest-claims-keda)

   In this sub-project, a *generic* Claims Web API is used to persist Claims data in the backend SQL Server database in a guaranteed manner.  The generic Web API uses Azure **Service Bus Queues** to *reliably* ingest claims data into the backend data store.  Azure **Function Apps** are used to expose the generic Claims Web API and for persisting the Claims data in the data store via the Claims Web API (implemented in the parent project).  The solution details the steps for running Azure Function Apps on AKS and using [Kubernetes-based Event Driven Autoscaling](https://docs.microsoft.com/en-us/azure/azure-functions/functions-kubernetes-keda) to process messages (claims records) at event driven scale in Kubernetes.

