using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;
using timeov.Common.Responses;
using timeov.Functions.Entities;

namespace timeov.Functions.Functions
{
    public static class ConsolidatedAPI
    {

        [FunctionName(nameof(consolidateProcess))]
        public static async Task<IActionResult> consolidateProcess(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "consolidated")] HttpRequest req,
            [Table("consolidated", Connection = "AzureWebJobsStorage")] CloudTable consolidatedTable,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            ILogger log)
        {

            TableQuery<TimeEntity> query = new TableQuery<TimeEntity>();
            TableQuerySegment<TimeEntity> times = await timeTable.ExecuteQuerySegmentedAsync(query, null);

            foreach (TimeEntity time in times.Results)
            {
                //time.employeeId
                Console.WriteLine("employeeId: {0}", time.employeeId);
                //Console.WriteLine(time.employeeId);
                //Debug.WriteLine;
                //Console.WriteLine("Hola Mundo Desde C# Consola");
            }


            /*TableOperation addOperation = TableOperation.Insert(ConsolidatedEntity);
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
