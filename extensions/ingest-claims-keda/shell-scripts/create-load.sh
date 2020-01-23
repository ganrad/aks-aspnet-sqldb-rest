#! /bin/bash
# Author : Ganesh Radhakrishnan @Microsoft
# Description: Call the Claims Web API with test data.
# Invoke this script as follows:
# - create-load.sh <no. of test runs> <API end-point/hostname> <directory location of test data>
#

if [ $# -ne 3 ]; then
  echo -e " Usage:"
  echo -e "\t create-load.sh <no. of test runs> <hostname:port> <location of test data file>"
  exit 1
fi

# Claims API Service hostname
svcIpAddress=$2

# Full directory path to the claims JSON data file on local system
dataFile=$3

counter=1

echo "***** Starting Load Test *****"

echo "***** Attempting total no. of test runs = [$1] *****"
echo
while [ $counter -le $1 ]
do
  echo "================================================================"
  echo "************* Executing [run # = $counter] ***************"
  echo "================================================================"

  ## Add a claim for institutional provider -----------------------------
  echo -e "***** Inserting claim for provider *****"
  code=`curl -X POST -H "Content-Type: application/json" --connect-timeout 5 -s -o /dev/null -w "%{http_code}" -d "@$dataFile" http://$svcIpAddress/api/claims/`

  echo "HTTP Response code=$code"
  if [ $code -ne "200" ]
  then
    echo "Encountered exception inserting a claim.  Exiting ...."
    exit 1
  fi

  counter=$((counter + 1))
done
echo "Total no. of test runs completed OK = $((counter - 1))"
