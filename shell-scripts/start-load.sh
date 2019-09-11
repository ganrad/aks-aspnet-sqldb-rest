#! /bin/bash
svcIpAddress=<Service IP Address>
counter=1

echo "Claims resources - INSERT - RETRIEVE - UPDATE - RETRIEVE - DELETE"
echo "***** Executing functional tests with institutional and professional claims *****"
echo "***** Attempting total no. of test runs = [$1] *****"
echo

while [ $counter -le $1 ]
do
  ## Get all claims
  echo -e "***** Retrieving all claims, run # = [$counter] *****"
  # curl -i -k https://grclaimsapi.azurewebsites.net/api/claims/
  curl -i http://$svcIpAddress/api/v1/claims/
  echo

  sleep 1

  ## Add a claim for institutional provider -----------------------------
  echo -e "***** Inserting claim for institutional provider *****"
  clmid=`curl -X POST -H "Content-Type: application/json" -d "@./test-data/claim01.json" http://$svcIpAddress/api/v1/claims/ | ~/jq/jq-linux64 '.claimItemId'`

  # Get & update the claim
  echo -e "***** Retrieving and updating the claim [$clmid] for institutional provider *****"
  record=`curl http://$svcIpAddress/api/v1/claims/$clmid | sed -e "s/1234.50/4321.50/g"`
  echo
  echo -e "***** Institutional claim for provider after changing value of field totalClaimCharge *****"
  echo -e $record
  echo 

  # Update the claim
  echo -e "***** Updating the claim [$clmid] for institutional provider *****"
  curl -X PUT -H "Content-Type: application/json" --data "$record" http://$svcIpAddress/api/v1/claims/$clmid
  echo

  # Get the claim
  echo -e "***** Retrieving the updated institutional claim [$clmid] *****"
  curl -i http://$svcIpAddress/api/v1/claims/$clmid
  echo

  # Delete the institutional claim
  echo -e "***** Deleting the claim [$clmid] for institutional provider *****" 
  curl -X DELETE http://$svcIpAddress/api/v1/claims/$clmid 
  echo

  sleep 1

  ## Add a claim for professional provider ----------------------------
  echo -e "***** Inserting claim for professional provider *****"
  clmid=`curl -X POST -H "Content-Type: application/json" -d "@./test-data/claim02.json" http://$svcIpAddress/api/v1/claims/ | ~/jq/jq-linux64 '.claimItemId'`

  # Get & update the claim
  echo -e "***** Retrieving and updating the claim [$clmid] for professional provider *****"
  record=`curl http://$svcIpAddress/api/v1/claims/$clmid | sed -e "s/1234.50/4321.50/g"`
  echo
  echo -e "***** Professional claim for provider after changing value of field totalClaimCharge *****"
  echo -e $record
  echo

  # Update the claim
  echo -e "***** Updating the claim [$clmid] for professional provider *****"
  curl -X PUT -H "Content-Type: application/json" --data "$record" http://$svcIpAddress/api/v1/claims/$clmid
  echo

  # Get the claim
  echo -e "***** Retrieving the updated professional claim [$clmid] *****"
  curl -i http://$svcIpAddress/api/v1/claims/$clmid
  echo

  # Delete the professional claim
  echo -e "***** Deleting the claim [$clmid] for professional provider *****"
  curl -X DELETE http://$svcIpAddress/api/v1/claims/$clmid 
  echo

  sleep 1

  counter=$((counter + 1))
done
echo "Total no. of test runs completed OK = $((counter - 1))"
