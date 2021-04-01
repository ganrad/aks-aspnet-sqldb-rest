# Deploy Claims API on Azure Stack Edge

Steps:
1. Create a new branch (e.g. "ase-test") on GitHub.
2. Checkout the new branch.
3. Rename your existing claims-api directory to **aks-aspnet-sqldb-rest/claims-api.orig**.
4. Copy **aks-aspnet-sqldb-rest/extensions/azure-stack-edge/claims-api** to **aks-aspnet-sqldb-rest/claims-api**.
5. Make necessary changes under "imageCredentials" in **values.yaml**.
6. Commit and push changes to new branch.
7. Go to the ADO project and rebuild the pipeline using the new branch (instead of master).
