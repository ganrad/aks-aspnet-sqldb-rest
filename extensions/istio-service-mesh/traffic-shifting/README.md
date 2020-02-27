# Traffic Shifting

**Prerequisites:**
1. You should have completed Sections **A** thru **I** in the parent project.

A common use case is to migrate traffic gradually from one version of a microservice to another.  This traffic shifting concept is often referred to as **Canary** deployments.  With a canary deployment, a small percentage of the web traffic is directed to the new deployment (application version) and the rest is sent to the current/existing deployment.  Over a period of time if everything goes smoothly, all the traffic is gradually sent to the new deployment.

For the Claims API microservice, we implemented the logic to expose OpenAPI specifications in version **v2**.  In version **v3**, we introduced the logic to send application metrics data to Azure Application Insights.  In this section, we will split the traffic equally between versions **v2** and **v3**, meaning 50% of the API traffic will be routed to claims-api:v2 and the remaining 50% to claims-api:v3.

Edit/View the Virtual Service resource file `./istio-resources/claims-api-vs.yaml` to understand how the service is configured to split the traffic between versions **v2** and **v3** of the microservice.

Refer to the command snippet below and apply the traffic shifting configuration.

```bash
# Update the Virtual Service API resource 'claims-api-vs'
$ kubectl apply -f ./istio-resources/claims-api-vs.yaml -n dev-claims-istio
#
```

Use the Azure Portal and access the *Application Insights* instance. Generate some Claims API traffic using a script (curl) or a web browser.  In Azure Application Insights, the claims API traffic can be viewed in real time on the Live Stream Metrics blade.  You will observe that metrics data for only 50% of the API calls are received by Application Insights.  This is because we configured Istio to split traffic and send only 50% of the API calls to version **v3** of the microservice.

Leave the Application Insights Live Stream Metrics blade open.  Update the **claims-api-vs** Virtual Service one more time and direct 100% of the traffic to claims-api:v3 service.  Observe how Istio directs 100% of the API traffic to v3!

In this section, you have seen how Istio service mesh can be used to split traffic among multiple versions of a microservice.

**References:**
- [Istio Traffic Shifting](https://istio.io/docs/tasks/traffic-management/traffic-shifting/)
