#! /bin/bash
svcIpAddress=$2
dataDir=$3
counter=1

echo "***** Starting Functional Test *****"

wget https://github.com/stedolan/jq/releases/download/jq-1.6/jq-linux64
chmod 700 ./jq-linux64
echo "***** Installed jq JSON parser *****"
echo

echo "Claims resources - INSERT - RETRIEVE - UPDATE - RETRIEVE - DELETE"
echo "***** Executing functional tests with institutional and professional claims *****"
echo "***** Attempting total no. of test runs = [$1] *****"
echo

while [ $counter -le $1 ]
do
  ## Get all claims
  echo -e "***** Retrieving all claims, run # = [$counter] *****"
  # curl -i -k https://grclaimsapi.azurewebsites.net/api/claims/
  hcode=$(curl --write-out \\n%{http_code}\\n --silent --output tmp.out http://$svcIpAddress/api/v1/claims/ | awk '{if(NR==2) print $0}')
  if [[ $hcode -ne 200 ]];
  then
    echo "Encountered http status code = $hcode, on getAllClaims Operation. Exiting ...."
    exit 1;
  else
    cat tmp.out | ./jq-linux64 '.'
  fi
  echo

  sleep 1

  ## Add a claim for institutional provider -----------------------------
  echo -e "***** Inserting claim for institutional provider *****"
  clmid=`curl -X POST -H "Content-Type: application/json" -d "@$dataDir/claim01.json" http://$svcIpAddress/api/v1/claims/ | ./jq-linux64 '.claimItemId'`

  if [ -z "$clmid" ]
  then
    echo "Encountered exception inserting an institutional claim (empty).  Exiting ...."
    exit 1
  fi

  # Get & update the claim
  echo -e "***** Retrieving the inserted claim [$clmid] for institutional provider *****"
  record=`curl http://$svcIpAddress/api/v1/claims/$clmid | sed -e "s/1234.50/4321.50/g"`
  if [ -z "$record" ]
  then
    echo "Encountered exception retrieving an inserted institutional claim (empty), exiting ...."
    exit 1
  fi

  echo
  echo -e "***** Institutional claim for provider after changing value of field totalClaimCharge *****"
  echo -e $record
  echo 

  # Update the claim
  echo -e "***** Updating the claim [$clmid] for institutional provider *****"
  hcode=$(curl -X PUT -H "Content-Type: application/json" --write-out %{http_code} --data "$record" http://$svcIpAddress/api/v1/claims/$clmid)
  if [[ $hcode -ne 204 ]];
  then
    echo "Encountered http status code = $hcode, on updateClaim Operation. Exiting ...."
    exit 1;
  fi
  echo

  # Get the claim
  # echo -e "***** Retrieving the updated institutional claim [$clmid] *****"
  # curl -i http://$svcIpAddress/api/v1/claims/$clmid
  # echo

  # Delete the institutional claim
  echo -e "***** Deleting the claim [$clmid] for institutional provider *****" 
  hcode=$(curl -X DELETE --write-out \\n%{http_code}\\n --output tmp.out http://$svcIpAddress/api/v1/claims/$clmid | awk '{if(NR==2) print $0}') 
  if [[ $hcode -ne 200 ]];
  then
    echo "Encountered http status code = $hcode, on deleteClaim Operation. Exiting ...."
    exit 1;
  else
    cat tmp.out | ./jq-linux64 '.'
  fi
  echo
  echo

  sleep 1
  #######################################################################################################
  # Professional claim tests ....
  #######################################################################################################
  sleep 1

  ## Add a claim for professional provider -----------------------------
  echo -e "***** Inserting claim for professional provider *****"
  clmid=`curl -X POST -H "Content-Type: application/json" -d "@$dataDir/claim02.json" http://$svcIpAddress/api/v1/claims/ | ./jq-linux64 '.claimItemId'`

  if [ -z "$clmid" ]
  then
    echo "Encountered exception inserting an professional claim (empty).  Exiting ...."
    exit 1
  fi

  # Get & update the claim
  echo -e "***** Retrieving the inserted claim [$clmid] for professional provider *****"
  record=`curl http://$svcIpAddress/api/v1/claims/$clmid | sed -e "s/1234.50/4321.50/g"`
  if [ -z "$record" ]
  then
    echo "Encountered exception retrieving an inserted professional claim (empty), exiting ...."
    exit 1
  fi

  echo
  echo -e "***** Professional claim for provider after changing value of field totalClaimCharge *****"
  echo -e $record
  echo 

  # Update the claim
  echo -e "***** Updating the claim [$clmid] for professional provider *****"
  hcode=$(curl -X PUT -H "Content-Type: application/json" --write-out %{http_code} --data "$record" http://$svcIpAddress/api/v1/claims/$clmid)
  if [[ $hcode -ne 204 ]];
  then
    echo "Encountered http status code = $hcode, on updateClaim Operation. Exiting ...."
    exit 1;
  fi
  echo

  # Get the claim
  # echo -e "***** Retrieving the updated professional claim [$clmid] *****"
  # curl -i http://$svcIpAddress/api/v1/claims/$clmid
  # echo

  # Delete the institutional claim
  echo -e "***** Deleting the claim [$clmid] for professional provider *****" 
  hcode=$(curl -X DELETE --write-out \\n%{http_code}\\n --output tmp.out http://$svcIpAddress/api/v1/claims/$clmid | awk '{if(NR==2) print $0}') 
  if [[ $hcode -ne 200 ]];
  then
    echo "Encountered http status code = $hcode, on deleteClaim Operation. Exiting ...."
    exit 1;
  else
    cat tmp.out | ./jq-linux64 '.'
  fi
  echo
  echo

  sleep 1
  counter=$((counter + 1))
done
echo "Total no. of test runs completed OK = $((counter - 1))"
