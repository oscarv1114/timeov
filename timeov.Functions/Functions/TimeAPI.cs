using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using timeov.Common.Models;
using timeov.Common.Responses;
using timeov.Functions.Entities;

namespace timeov.Functions.Functions
{
    public static class TimeAPI
    {
        [FunctionName(nameof(CreateTime))]
        public static async Task<IActionResult> CreateTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "time")] HttpRequest req,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            ILogger log)
        {
            log.LogInformation("Recieved a new register time.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Time time = JsonConvert.DeserializeObject<Time>(requestBody);

            if (time?.employeeId <= 0)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "The request must have a employee id."
                });
            }

            if (time?.dateTime < DateTime.MinValue || time?.dateTime > DateTime.MaxValue)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "The request must have a date time valid."
                }); ;
            }


            if (time?.type < 0 || time?.type > 1)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "The request must have a register type valid."
                });
            }

            TimeEntity timeEntity = new TimeEntity
            {
                employeeId = time.employeeId,
                dateTime = time.dateTime,
                type = time.type,
                isConsolidated = false,
                PartitionKey = "TIME",
                RowKey = Guid.NewGuid().ToString(),
                ETag = "*"
            };


            TableOperation addOperation = TableOperation.Insert(timeEntity);
            await timeTable.ExecuteAsync(addOperation);

            string message = "New register stored in table.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEntity
            });
        }

        [FunctionName(nameof(UpdateTime))]
        public static async Task<IActionResult> UpdateTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "time/{id}")] HttpRequest req,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            string id,
            ILogger log)
        {
            log.LogInformation($"Update for register {id}, Received.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Time time = JsonConvert.DeserializeObject<Time>(requestBody);

            //validate id

            TableOperation findOperation = TableOperation.Retrieve<TimeEntity>("TIME", id);
            TableResult findResult = await timeTable.ExecuteAsync(findOperation);

            if (findResult.Result == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Register not found."
                }); ;
            }

            //update
            TimeEntity timeEntity = (TimeEntity)findResult.Result;

            if (!string.IsNullOrEmpty(time.employeeId.ToString())
                || !string.IsNullOrEmpty(time.dateTime.ToString())
                || !string.IsNullOrEmpty(time.type.ToString()))
            {
                timeEntity.employeeId = time.employeeId;
                timeEntity.dateTime = time.dateTime;
                timeEntity.type = time.type;
            }


            TableOperation addOperation = TableOperation.Replace(timeEntity);
            await timeTable.ExecuteAsync(addOperation);

            string message = $"register {id}, updated in table.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEntity
            });
        }


        [FunctionName(nameof(GetAllTimes))]
        public static async Task<IActionResult> GetAllTimes(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time")] HttpRequest req,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            ILogger log)
        {
            log.LogInformation("Get all register received.");

            TableQuery<TimeEntity> query = new TableQuery<TimeEntity>();
            TableQuerySegment<TimeEntity> times = await timeTable.ExecuteQuerySegmentedAsync(query, null);


            string message = "Retrieved all register.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = times
            });
        }


        [FunctionName(nameof(GetTimeById))]
        public static IActionResult GetTimeById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time/{id}")] HttpRequest req,
            [Table("time", "TIME", "{id}", Connection = "AzureWebJobsStorage")] TimeEntity timeEntity,
            string id,
            ILogger log)
        {
            log.LogInformation($"Get register by id: {id}, received.");

            if (timeEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Register not found."
                }); ;
            }

            string message = $"Register {timeEntity.RowKey}, retrieved.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEntity
            });
        }


        [FunctionName(nameof(DeleteTime))]
        public static async Task<IActionResult> DeleteTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "time/{id}")] HttpRequest req,
            [Table("time", "TIME", "{id}", Connection = "AzureWebJobsStorage")] TimeEntity timeEntity,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            string id,
            ILogger log)
        {
            log.LogInformation($"Delete register: {id}, received.");

            if (timeEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Register not found."
                }); ;
            }

            await timeTable.ExecuteAsync(TableOperation.Delete(timeEntity));
            string message = $"Register {timeEntity.RowKey}, deleted.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEntity
            });
        }

    }
}
