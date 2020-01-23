using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GenHttpApiFunctions
{
    public static class GenClaimsHttpApiToSbQueue
    {
        [FunctionName("PostClaimsHttpApiToSbQueue")]
        [return: ServiceBus("%ClaimsPostQueue%", Connection="AzServiceBusConnection")]
        public static string Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "claims")] string data,
            ILogger log)
        {
            log.LogInformation("C# function: PostClaimsHttpApiToSbQueue, received a request");
	    log.LogInformation($"Payload:\n{data}");

	    return data;
        }
    }
}
