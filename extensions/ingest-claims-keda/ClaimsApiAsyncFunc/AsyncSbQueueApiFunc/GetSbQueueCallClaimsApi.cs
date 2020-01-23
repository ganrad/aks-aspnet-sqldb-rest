using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using Newtonsoft.Json;

namespace AsyncSbQueueApiFunc
{
    public static class GetSbQueueCallClaimsApi
    {
        [FunctionName("GetSbQueueCallClaimsDelApi")]
        public static void Run([ServiceBusTrigger("%ClaimsDelQueue%", Connection = "AzServiceBusConnection")]string claimItem, ILogger log)
        {
            log.LogInformation($"C# function: GetSbQueueCallClaimsDelApi - received message: {claimItem}");

            Dictionary<String, Object> claimsObj = JsonConvert.DeserializeObject<Dictionary<String,Object>>(claimItem);
	    var claimItemId = claimsObj["claimItemId"];

	    log.LogInformation($"GetSbQueueCallClaimsDelApi - Claim Item ID: {claimItemId}");
        }
    }
}
