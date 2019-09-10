#! /bin/bash
counter=1
echo "INSERT - RETRIEVE - UPDATE - RETRIEVE - DELETE"
echo "*****Executing test run with [$1] institutional and professional claims"
echo
while [ $counter -le $1 ]
do
  ## Get all claims
  # curl -i -k https://grclaimsapi.azurewebsites.net/api/claims/
  curl -i http://$(svcIp.KubectlOutput)/api/v1/claims/
  # sleep 1

  ## Add a claim for institutional provider 1
  echo -e "*****Inserting claim [$counter] for institutional provider 1"
  clmid=`curl -X POST -H "Content-Type: application/json" -k -d @./claim01.json http://$(svcIp.KubectlOutput)/api/v1/claims/ | jq-linux64 '.claimItemId'`
  # Get & update the claim
  echo -e "*****Retrieving and updating the claim for institutional provider 1"
  record=`curl -k http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid | sed -e "s/1234.50/4321.50/g"`
  echo
  echo -e "*****Institutional claim for provider 1 after changing value of field totalClaimCharge"
  echo -e $record
  echo 
  # Update the claim
  echo -e "*****Updating the claim for institutional provider 1"
  curl -X PUT -H "Content-Type: application/json" -k --data "$record" http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid
  echo
  echo -e "*****Retrieving the updated institutional claim"
  # Get the claim
  curl -k https://$(svcIp.KubectlOutput)/api/v1/claims/$clmid
  # Delete the institutional claim
  echo -e "*****Deleting the claim for institutional provider 
  curl -X DELETE -k http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid 
  sleep 1

  echo
  echo -e "*****Inserting claim [$counter] for professional provider 1"
  ## Add a claim for professional provider 1
  clmid=`curl -X POST -H "Content-Type: application/json" -k -d @./claim02.json https://$(svcIp.KubectlOutput)/api/v1/claims/ | jq-linux64 '.claimItemId'`
  # Get & update the claim
  echo -e "*****Retrieving and updating the claim for professional provider 1"
  record=`curl -k http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid | sed -e "s/1234.50/4321.50/g"`
  echo
  echo -e "*****Professional claim for provider 1 after changing value of field totalClaimCharge"
  echo -e $record
  echo 
  # Update the claim
  echo -e "*****Updating the claim for professional provider 1"
  curl -X PUT -H "Content-Type: application/json" -k --data "$record" http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid
  echo
  echo -e "*****Retrieving the updated professional claim"
  # Get the claim
  curl -k https://$(svcIp.KubectlOutput)/api/v1/claims/$clmid
  # Delete the professional claim
  echo -e "*****Deleting the claim for institutional provider 
  curl -X DELETE -k http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid 
  sleep 1

  counter=$((counter + 1))
done
