# Intelligent Request Routing

**Prerequisites:**
1. You should have completed Sections **A** thru **I** in the parent project.

Thus far, three different versions of the Claims API microservice have been deployed on the AKS cluster and are running concurrently.  However, when you hit the OpenAPI end-point multiple times, you will observe that you get a **page can't be found** exception (HTTP Status code 404) once in every two attempts.  This is because without an explicit default service version to route to, Istio sends the request directly to one of the Kubernetes service (**claims-api** service) end-points in a round robin fashion.

End-point to view the OpenAPI spec for the Claims Web API : http://[Istio Ingress Gateway ALB Public IP]/swagger/index.html

In this section, we will configure the Virtual Service **claims-api-vs** to route all API requests to the default version of the microservice.  The default version in this case will be the latest version **v3** which also exposes the OpenAPI spec.  The latest version v3 includes all the enhancements made to the Claims API microservice.

Edit/View the Virtual Service resource file `./istio-resources/claims-api-vs.yaml` to understand how the service is configured to route all API requests to version v3 of the Claims API.

Refer to the command snippet below to apply the **Claims-api-vs** Virtual Service on the service mesh. 

```bash
# Apply the Virtual Service resource 'claims-api-vs' on the service mesh
$ kubectl apply -f ./istio-resources/claims-api-vs.yaml -n dev-claims-istio
#
```

Using a browser (or curl), access the OpenAPI end-point for the Claims API microservice multiple times (refresh). You will observe that the API Spec is returned each time.  This is because we configured Istio to forward all API traffic for all context paths to version **v3** of the **claims-api** service.

In this section, you have successfully routed traffic to one specific version of the Claims API microservice.  It is also possible to route traffic to different service versions based on the value of custom HTTP headers.  This option is left as an exercise for you to explore.

**References:**
- [Istio Request Routing](https://istio.io/docs/tasks/traffic-management/request-routing/)
- [Virtual Service Configuration](https://istio.io/docs/reference/config/networking/virtual-service/)
