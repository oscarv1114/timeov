using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using timeov.Common.Models;
using timeov.Functions.Entities;
using timeov.Functions.Functions;
using timeov.Tests.Helpers;
using Xunit;

namespace timeov.Tests.Tests
{
    public class TimeApiTest
    {
        private static readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreateTime_Should_return_200()
        {
            // Arrenge
            MockCloudTableTimes mockTimes = new MockCloudTableTimes(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time todoRequest = TestFactory.GetTimeRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(todoRequest);

            // Act
            IActionResult response = await TimeAPI.CreateTime(request, mockTimes, logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }


        [Fact]
        public async void UpdateTime_Should_Return_200()
        {
            // Arrenge
            MockCloudTableTimes mockTimes = new MockCloudTableTimes(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time timeRequest = TestFactory.GetTimeRequest();
            Guid timeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeId, timeRequest);

            // Act
            IActionResult response = await TimeAPI.UpdateTime(request, mockTimes, timeId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }


        [Fact]
        public async void DeleteTime_Should_Return_200()
        {
            // Arrenge
            MockCloudTableTimes mockTimes = new MockCloudTableTimes(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time timeRequest = TestFactory.GetTimeRequest();
            TimeEntity timeEntity = TestFactory.GetTimeEntity();
            Guid timeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeId, timeRequest);

            // Act
            IActionResult response = await TimeAPI.DeleteTime(request, timeEntity, mockTimes, timeId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }


        [Fact]
        public async void GetAllTime_Should_Return_200()
        {
            // Arrenge
            MockCloudTableTimes mockTimes = new MockCloudTableTimes(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time timeRequest = TestFactory.GetTimeRequest();
            TimeEntity timeEntity = TestFactory.GetTimeEntity();
            Guid timeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest();
            //DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeId, timeRequest);
            //DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeRequest);
            //DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeId);

            // Act
            IActionResult response = await TimeAPI.GetAllTimes(request, mockTimes, logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }


        [Fact]
        public async void GetTimeById_Should_Return_200()
        {
            // Arrenge
            Time timeRequest = TestFactory.GetTimeRequest();
            TimeEntity timeEntity = new TimeEntity();
            Guid timeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeId, timeRequest);

            // Act
            IActionResult response = TimeAPI.GetTimeById(request, timeEntity, timeId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
