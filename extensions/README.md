# Extensions to the **Claims API** Microservice
The extensions below explore advanced features of AKS (Kubernetes) and additional open source solutions. Each extension (or sub-project) describes a particular use case and details the steps for implementing a potential solution using an advanced feature/capability.

- [Ingest Claims records using **KEDA**](./ingest-claims-keda)

   In this sub-project, a *generic* Claims Web API is used to persist Claims data in the backend SQL Server database in a guaranteed manner.  The generic Web API uses Azure **Service Bus Queues** to *reliably* ingest claims data into the backend data store.  Azure **Function Apps** are used to expose the generic Claims Web API and for persisting the Claims data in the data store via the Claims Web API (implemented in the parent project).  The solution details the steps for running Azure Function Apps on AKS and using [Kubernetes-based Event Driven Autoscaling](https://docs.microsoft.com/en-us/azure/azure-functions/functions-kubernetes-keda) to process messages (claims records) at event driven scale in Kubernetes.

- [Inject Azure Key Vault Secrets using **AAD Pod Identity**](./use-pod-identity)

  In this sub-project, secrets stored in Azure **Key Vault** are fetched and injected inside the Claims Web API application container at runtime.  The project makes use of two open source projects - [AAD Pod Identity](https://github.com/Azure/aad-pod-identity) and [Azure Key Vault Kubernetes Flex Volume](https://github.com/Azure/kubernetes-keyvault-flexvol).  Storing application secrets in Azure Key Vault provides a secure alternative to storing them in standard Kubernetes *Secret* API objects on the etcd server.  The secrets stored in Key Vault never come to reside on the nodes and are directly injected into the application container at runtime.

- [Access Azure SQL using **Managed Identity for Azure Resources**](./use-pod-identity-mid)

  In this sub-project, the Claims Web API application uses an Azure *Managed Identity* to access the Azure SQL Database.  [Managed Identities for Azure Resources](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview) is a feature of Azure Active Directory and can be used by Azure service instances (eg., AKS) to securely access other Azure resources (eg., Key Vault, SQL Server ...) that support AD authentication.  With Managed Identity, applications deployed on Azure service instances need not store any credential information.  This project also makes use of [AAD Pod Identity](https://github.com/Azure/aad-pod-identity).

- [Explore advanced features of **Istio** Service Mesh](./istio-service-mesh)

  This sub-project examines the advanced features supported by [Istio Service Mesh](https://istio.io/docs/concepts/what-is-istio/).  Features such as intelligent request/traffic routing, traffic splitting (a.k.a Canary rollouts), request timeouts, circuit breaking, fault injection, secure intra-pod communication, rate limiting and others are explored in greater depth and detail.  
