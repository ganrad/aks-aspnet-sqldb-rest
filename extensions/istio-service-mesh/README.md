# Explore **Istio** Service Mesh features

This extension project explores many of the advanced capabilities supported by Istio Service Mesh.

What is a Service Mesh ? Here's are a few excerpts taken from the Istio Service Mesh website
>The term service mesh is used to describe the network of microservices that make up such applications and the interactions between them.

>In production environments, Istio's diverse feature set lets you successfully and efficiently run a distributed microservice architecture and provides a uniform way to secure, connect and monitor microservices.

Istio provides the following four capabilities uniformly across a network of services:
- Traffic Management

  Let's you control the flow of traffic and API calls between services.

- Security

  Provides the underlying secure communication channel, and manages authentication, authorization, and encryption of service communication at scale.

- Policies

  Let's you configure custom policies for applications which enforce rules at runtime such as rate limiting, denials, white and black lists and header redirects and rewrites.

- Observability

  Supports robust tracing, monitoring and logging features which provide deep insights into the service mesh deployment. 

In this project, we will reuse the Claims Web API microservice and Azure Function Apps introduced in previous projects to demonstrate the following Service Mesh features.

- Intelligent request routing
- Traffic shifting
- Request timeouts
- Fault injection
- Circuit breaking
- Rate limiting

**Functional Diagram:**

Refer to the architecture diagram [here](https://istio.io/docs/ops/deployment/architecture/)

**Prerequisites:**
1. Readers are required to complete Sections A thru G in the [parent project](https://github.com/ganrad/aks-aspnet-sqldb-rest) before proceeding with the hands-on labs in this project.

Readers are advised to go thru the following on-line resources before proceeding with the hands-on sections.
- [Istio Service Mesh](https://istio.io/docs/concepts/what-is-istio/)

## A. Deploy an Nginx Ingress Controller on the AKS Cluster

Congrats! In this extension, you examined many of the advanced features supported by Istio Service Mesh and how they can help you build scalable, fault tolerant cloud-native applications.  Now that you have fully explored the power of Kubernetes cloud-native platform on Azure (AKS) and associated open source ecosystem of frameworks and runtimes, go build and deploy business applications on Azure Cloud! 
