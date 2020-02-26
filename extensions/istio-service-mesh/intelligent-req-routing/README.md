# Intelligent Request Routing

Thus far, three different versions of the Claims API microservice have been deployed on the AKS cluster and are running concurrently.  However, when you hit the OpenAPI end-point multiple times, you will observe that you get a **page can't be found** exception (HTTP Status code 404) once in every two attempts.  This is because without an explicit default service version to route to, Istio sends the request directly to the Kubernetes service end-point which then routes the request to one of the service end-points in a round robin fashion.

End-point to view the OpenAPI spec for the Claims Web API : http://[Istio Ingress Gateway ALB Public IP]/swagger/index.html

In this section, we will configure the Virtual Service **claims-api-vs** to route all API requests to the default version of the microservice.  In this case, the default version will be the latest version **v3**.  This latest version includes all the enhancements made to the Claims API microservice.

>**NOTE:**You should have completed all Sections **A** thru **I** in the parent project.

Edit/View the Virtual Service resource file `./istio-resources/claims-api-vs.yaml` to understand how the service is configured to route all API requests that don't match the prefix context path `/api/v1/claims` to version v3 of the Claims API.  Keep in mind, the order of the routing rules in the Virtual Service resource is very important!

Refer to the command snippet below to apply the **Claims-api-vs** Virtual Service on the service mesh. 

```bash
# Apply the Virtual Service resource 'claims-api-vs' on the service mesh
$ kubectl apply -f ./istio-resources/claims-api-vs.yaml
#
```

Using a browser (or curl), access the OpenAPI end-point for the Claims API microservice multiple times (refresh). You will observe that the API Spec is returned each time.  This is because you configured Istio to forward all API traffic for all context paths other than `/api/v1/claims` to be routed to version **v3** of the **claims-api** service.

In this section, you have successfully routed traffic to one specific version of the Claims API microservice.  It is also possible to route traffic to different service versions based on HTTP headers.  This option is left as an exercise for you to explore.

**References:**
- [Istio Request Routing](https://istio.io/docs/tasks/traffic-management/request-routing/)
- [Virtual Service Configuration](https://istio.io/docs/reference/config/networking/virtual-service/)
