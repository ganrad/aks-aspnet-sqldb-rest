# Build and deploy an Azure Function for ingesting Claims records at scale
This sub-project describes the steps for building an **Azure Function** to ingest Claims records into the Claims backend data store.

The Function module is implemented as a **Claims Data Handler** and executes within the **Azure Function** runtime.  The Azure Functions runtime itself is packaged within a container and deployed on AKS.  The data handler Function reads Claims records from an Azure Storage Queue and ingests them to the storage backend via the Claims Web API implemented in the parent project.

The solution leverages KEDA on AKS.  Kubernetes based Functions provides the Functions runtime in a Docker container with event-driven scaling through KEDA.

**Prerequisites:**
1. Readers are required to complete Sections A thru G in the [parent project](https://github.com/ganrad/aks-aspnet-sqldb-rest) before proceeding with the hands-on labs in this sub-project.
