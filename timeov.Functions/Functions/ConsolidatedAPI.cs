using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using timeov.Functions.Entities;
using timeov.Common.Responses;

namespace timeov.Functions.Functions
{
    public static class ConsolidatedAPI
    {

        [FunctionName(nameof(consolidatedProcess))]
        public static async Task<IActionResult> consolidatedProcess(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "consolidated")] HttpRequest req,
            [Table("consolidated", Connection = "AzureWebJobsStorage")] CloudTable consolidatedTable,
            ILogger log)
        {
            /*
            log.LogInformation("Recieved a new register time.");
            
            TableQuery<TimeEntity> query = new TableQuery<TimeEntity>().Where("isConsolidated=false");
            TableQuerySegment<TimeEntity> times = await consolidatedTable.ExecuteQuerySegmentedAsync(query, null);


            

            TableOperation addOperation = TableOperation.Insert(ConsolidatedEntity);
            await consolidatedTable.ExecuteAsync(addOperation);

            string message = "New register stored in table.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEntity
            });*/
            return null;
        }

        [FunctionName(nameof(GetAllconsolidatedByDate))]
        public static async Task<IActionResult> GetAllconsolidatedByDate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "consolidated/{date}")] HttpRequest req,
            [Table("consolidated", Connection = "AzureWebJobsStorage")] CloudTable consolidatedTable,
            ILogger log)
        {
            log.LogInformation("Get all consolidated received.");

            TableQuery<ConsolidatedEntity> query = new TableQuery<ConsolidatedEntity>();
            TableQuerySegment<ConsolidatedEntity> consolidated = await consolidatedTable.ExecuteQuerySegmentedAsync(query, null);


            string message = "Retrieved all consolidated.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = consolidated
            });
        }


    }
}
