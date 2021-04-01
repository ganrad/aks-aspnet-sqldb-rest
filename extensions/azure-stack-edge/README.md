# Deploy Claims API on Azure Stack Edge

**Functional Architecture:**

![alt tag](./images/EXT-ase-arch.png)

Prerequisites:
* You have access to a [fully functional ASE with internet access and allocated sufficient IP address space for k8s services](https://docs.microsoft.com/en-us/azure/databox-online/azure-stack-edge-gpu-deploy-configure-network-compute-web-proxy#enable-compute-network) exopsed on your local network.
* [Create a new user and namespace with proper RBAC on ASE k8s](https://docs.microsoft.com/en-us/azure/databox-online/azure-stack-edge-gpu-create-kubernetes-cluster).
* [Enable edge container registry on ASE](https://docs.microsoft.com/en-us/azure/databox-online/azure-stack-edge-gpu-edge-container-registry).
* Install Helm3 on your build agent.
* **Optional**: You can [create a VM on ASE](https://docs.microsoft.com/en-us/azure/databox-online/azure-stack-edge-gpu-deploy-virtual-machine-portal) and use it as the ADO agent for building and deploying the Claims API.

Steps:
1. Create a new branch (e.g. "ase-test") on GitHub.
2. Checkout the new branch.
3. Rename your existing claims-api directory to **aks-aspnet-sqldb-rest/claims-api.orig**.
4. Copy **aks-aspnet-sqldb-rest/extensions/azure-stack-edge/claims-api** to **aks-aspnet-sqldb-rest/claims-api**.
5. Make necessary changes under "imageCredentials" in **values.yaml**.
6. Commit and push changes to new branch.
7. Go to the ADO project and update the build pipeline using new branch (instead of master).
8. Create a new container registry service connection for your edge container registry and update the build pipeline (e.g. "https://ecr.dbe-70c9r53.microsoftdatabox.com:31001/").
9. Create a new Kubernetes service connection using the kubeconfig file for the user who's grant access to the namespace for deployment.
10. In the release pipeline, edit the namespace and use the one you created for Claims API deployment.
11. Run pipeline and verify result:
```bash
azureuser@Ubuntu1804:~$ helm ls -n claims-api-dev
NAME                    NAMESPACE       REVISION        UPDATED                                 STATUS          CHART                   APP VERSION
aks-aspnetcore-lab      claims-api-dev  1               2021-03-31 23:44:33.354484376 +0000 UTC deployed        claims-api-0.1.0        1.0
azureuser@Ubuntu1804:~$ kubectl get services -n claims-api-dev
NAME             TYPE           CLUSTER-IP       EXTERNAL-IP   PORT(S)        AGE
claims-api-svc   LoadBalancer   172.28.121.185   192.168.1.9   80:31608/TCP   154m
```

Notes:
* Since we are using the edge container registry instead of ACR, we will need to create imagePullSecret on k8s to store the credentials. This is handled by helm 3 charts.
