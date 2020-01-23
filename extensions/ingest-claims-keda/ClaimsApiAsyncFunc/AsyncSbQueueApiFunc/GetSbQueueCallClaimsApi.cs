using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncSbQueueApiFunc
{
    public static class GetSbQueueCallClaimsApi
    {
        static readonly HttpClient client = new HttpClient();

        [FunctionName("GetSbQueueCallClaimsDelApi")]
        public static void Run0([ServiceBusTrigger("%ClaimsDelQueue%", Connection = "AzServiceBusConnection")]string claimItem, ILogger log)
        {
            log.LogInformation($"C# function: GetSbQueueCallClaimsDelApi - received message: {claimItem}");

            Dictionary<String, Object> claimsObj = JsonConvert.DeserializeObject<Dictionary<String,Object>>(claimItem);
	    string claimItemId = claimsObj["claimItemId"].ToString();

	    log.LogInformation($"GetSbQueueCallClaimsDelApi - Claim Item ID: {claimItemId}");
	    deleteClaimsRec(claimItemId, log);
        }

        private static async Task deleteClaimsRec(string claimId, ILogger log)
	{
	   String uriObj = "http://" + 
	     System.Environment.GetEnvironmentVariable("ClaimsApiHost") + 
	     System.Environment.GetEnvironmentVariable("ClaimsApiEndpoint") + 
	     "/" + claimId;
	   log.LogInformation($"deleteClaimsRec - Http Uri : {uriObj}");
	   try {
	     HttpResponseMessage response = await client.DeleteAsync(uriObj);
	     log.LogInformation($"deleteClaimsRec - Http Response: {response}");
	   }
	   catch (HttpRequestException hre)
	   {
	     log.LogError("deleteClaimsRec - An exception occured while calling the Claims Web Api");
	     log.LogError($"deleteClaimsRec - Exception: {hre.Message}");
	   }
	}
    }
}
