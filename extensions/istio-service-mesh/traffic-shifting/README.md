# Traffic Shifting

**Prerequisites:**
1. You should have completed Sections **A** thru **I** in the parent project.

A common use case is to migrate traffic gradually from one version of a microservice to another.  This traffic shifting logic is often referred to as **Canary** deployments.  With a canary deployment, a small percentage of the web traffic is directed to the new deployment and the rest goes to the current/existing deployment.  If everything goes smoothly, all the traffic is gradually sent to the new deployment.

For the Claims API microservice, we introduced the logic to expose OpenAPI specifications in version **v2**.  In version **v3**, we introduced the logic to send application metrics data to Azure Application Insights.  Let's now split the traffic equally between versions **v2** and **v3**.

Edit/View the Virtual Service resource file `./istio-resources/claims-api-vs.yaml` to understand how the service is configured to split the traffic between versions **v2** and **v3** of the microservice.

Refer to the command snippet below.

```bash
# Apply the updated Virtual Service resource 'claims-api-vs' on the service mesh
$ kubectl apply -f ./istio-resources/claims-api-vs.yaml -n dev-claims-istio
#
```

Use the Azure Portal and access the *Application Insights* instance. Generate some Claims API traffic and in Azure Application Insights review the application map, live stream metrics and dashboards.  You will observe that only 50% of the API calls are instrumented by Application Insights.  This is because we configured Istio to split traffic and send only 50% of the API calls to version **v3** of the microservice.

In this section, you have seen how Istio service mesh can be used to split traffic among multiple versions of a microservice.

**References:**
- [Istio Traffic Shifting](https://istio.io/docs/tasks/traffic-management/traffic-shifting/)
