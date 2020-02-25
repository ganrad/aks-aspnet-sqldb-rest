# Explore **Istio** Service Mesh features

This extension project explores many of the advanced capabilities supported by Istio Service Mesh.

What is a Service Mesh ? Here are a few excerpts taken from the Istio Service Mesh website
>The term service mesh is used to describe the network of microservices that make up such applications and the interactions between them.

>In production environments, Istio's diverse feature set lets you successfully and efficiently run a distributed microservice architecture and provides a uniform way to secure, connect and monitor microservices.

At a high level, Istio provides the following four capabilities uniformly across a network of services:
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

## A. Deploy an Istio Service Mesh on AKS
**Approx. time to complete this section: 20 minutes**

In this section, we will install **Istio** Service Mesh using Istio CLI (`istioctl`).  You should have installed the CLI on the Linux VM (Bastion Host) in the parent project.

Follow the steps below.

1. Install the **demo** profile.

   To explore the broad feature set of Istio Service Mesh, we will deploy the **demo** profile in this project.

   >**NOTE:** The **default** profile serves as a good starting point for **production* clusters.  Refer to the Istio documentation for deploying a custom profile.

   ```bash
   # Install the 'demo' profile
   # Istio Service Mesh installation install many services, pods & an Ingress Gateway in the
   # 'istio-system' namespace.
   #
   $ istioctl manifest apply --set profile=demo
   #
   ```

2. Verify the installation.

   Verify the service mesh installation by ensuring kubernetes services are deployed and they all have a ClusterIP (except **jaeger-agent** service).  Also, the **istio-ingressgateway** service should have been assigned an Public IP on the Azure Load Balancer.  Verify the Public IP for this service under the **EXTERNAL-IP** column.

   ```bash
   # Verify all services
   # Sample output is provided below.
   #
   $ kubectl get svc -n istio-system
   NAME                     TYPE           CLUSTER-IP       EXTERNAL-IP     PORT(S)                 AGE
   grafana                  ClusterIP      172.21.211.123   <none>          3000/TCP                   2m
   istio-citadel            ClusterIP      172.21.177.222   <none>          8060/TCP,15014/TCP         2m
   istio-egressgateway      ClusterIP      172.21.113.24    <none>          80/TCP,443/TCP,15443/TCP   2m
   istio-galley             ClusterIP      172.21.132.247   <none>          443/TCP,15014/TCP,9901/TCP 2m
   istio-ingressgateway     LoadBalancer   172.21.144.254   52.116.22.242   15020:31831/TCP            2m
   istio-pilot              ClusterIP      172.21.105.205   <none>          15010/TCP,15011/TCP,8080/TCP,15014/TCP   2m
   istio-policy             ClusterIP      172.21.14.236    <none>          9091/TCP,15004/TCP,15014/TCP             2m
   istio-sidecar-injector   ClusterIP      172.21.155.47    <none>          443/TCP,15014/TCP          2m
   istio-telemetry          ClusterIP      172.21.196.79    <none>          9091/TCP,15004/TCP,15014/TCP,42422/TCP   2m
   jaeger-agent             ClusterIP      None             <none>          5775/UDP,6831/UDP,6832/UDP 2m
   jaeger-collector         ClusterIP      172.21.135.51    <none>          14267/TCP,14268/TCP        2m
   jaeger-query             ClusterIP      172.21.26.187    <none>          16686/TCP                  2m
   kiali                    ClusterIP      172.21.155.201   <none>          20001/TCP                  2m
   prometheus               ClusterIP      172.21.63.159    <none>          9090/TCP                   2m
   tracing                  ClusterIP      172.21.2.245     <none>          80/TCP                     2m
   zipkin                   ClusterIP      172.21.182.245   <none>          9411/TCP                   2m
   #
   ```

3. (Optional) Verify a successful installation.

   ```bash
   # Generate a manifest for the 'demo' profile
   $ istioctl manifest generate --set profile=demo > $HOME/istio-manifest.yaml
   #
   # Run the 'verify-install' command to see if the installation was successful
   $ istioctl verify-install -f $HOME/istio-manifest.yaml
   #
   ```

Congrats! In this extension, you examined many of the advanced features supported by Istio Service Mesh and how they can help you build scalable, fault tolerant cloud-native applications.  Now that you have fully explored the power of Kubernetes cloud-native platform on Azure (AKS) and associated open source ecosystem of frameworks and runtimes, go build and deploy business applications on Azure Cloud! 
