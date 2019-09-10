#! /bin/bash
counter=1
echo "Claims resources - INSERT - RETRIEVE - UPDATE - RETRIEVE - DELETE"
echo "***** Executing functional tests with institutional and professional claims *****"
echo "***** Total no. of runs = [$1] *****"
echo

while [ $counter -le $1 ]
do
  ## Get all claims
  echo -e "***** Retrieving all claims, run # = [$counter] *****"
  # curl -i -k https://grclaimsapi.azurewebsites.net/api/claims/
  curl -i http://$(svcIp.KubectlOutput)/api/v1/claims/
  echo

  sleep 1

  ## Add a claim for institutional provider -----------------------------
  echo -e "***** Inserting claim for institutional provider *****"
  clmid=`curl -X POST -H "Content-Type: application/json" -k -d @./claim01.json http://$(svcIp.KubectlOutput)/api/v1/claims/ | jq-linux64 '.claimItemId'`

  # Get & update the claim
  echo -e "***** Retrieving and updating the claim [$clmid] for institutional provider *****"
  record=`curl -k http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid | sed -e "s/1234.50/4321.50/g"`
  echo
  echo -e "***** Institutional claim for provider after changing value of field totalClaimCharge *****"
  echo -e $record
  echo 

  # Update the claim
  echo -e "***** Updating the claim [$clmid] for institutional provider *****"
  curl -X PUT -H "Content-Type: application/json" -k --data "$record" http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid
  echo

  # Get the claim
  echo -e "***** Retrieving the updated institutional claim [$clmid] *****"
  curl -k https://$(svcIp.KubectlOutput)/api/v1/claims/$clmid

  # Delete the institutional claim
  echo -e "***** Deleting the claim [$clmid] for institutional provider *****" 
  curl -X DELETE -k http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid 
  echo

  sleep 1

  ## Add a claim for professional provider ----------------------------
  echo -e "***** Inserting claim for professional provider *****"
  clmid=`curl -X POST -H "Content-Type: application/json" -k -d @./claim02.json https://$(svcIp.KubectlOutput)/api/v1/claims/ | jq-linux64 '.claimItemId'`

  # Get & update the claim
  echo -e "***** Retrieving and updating the claim [$clmid] for professional provider *****"
  record=`curl -k http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid | sed -e "s/1234.50/4321.50/g"`
  echo
  echo -e "***** Professional claim for provider 1 after changing value of field totalClaimCharge *****"
  echo -e $record
  echo

  # Update the claim
  echo -e "***** Updating the claim [$clmid] for professional provider *****"
  curl -X PUT -H "Content-Type: application/json" -k --data "$record" http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid
  echo

  # Get the claim
  echo -e "***** Retrieving the updated professional claim [$clmid] *****"
  curl -k https://$(svcIp.KubectlOutput)/api/v1/claims/$clmid

  # Delete the professional claim
  echo -e "***** Deleting the claim [$clmid] for professional provider *****"
  curl -X DELETE -k http://$(svcIp.KubectlOutput)/api/v1/claims/$clmid 
  echo

  sleep 1

  counter=$((counter + 1))
done
