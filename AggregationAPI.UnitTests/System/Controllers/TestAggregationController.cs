using AggregationAPI.Controllers;
using AggregationAPI.Interfaces;
using AggregationAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AggregationAPI.UnitTests.System.Controllers
{
    public class TestAggregationController
    {
        [Fact]
        public async Task Get_Success_ReturnsStatusCode200()
        {
            // Arrange
            var mockUsersService = new Mock<IAggregationRepository>();

            mockUsersService
               .Setup(service => service.GetAggregatedDatasets())
               .ReturnsAsync(MockData.MockDatasets());

            var sut = new AggregationController(mockUsersService.Object);

            var result = (OkObjectResult)await sut.GetAggregatedDatasets();

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Get_EmptyResult()
        {
            var mockUsersService = new Mock<IAggregationRepository>();
            mockUsersService
                .Setup(service => service.GetAggregatedDatasets())
                .ReturnsAsync(new List<Dataset>());

            var sut = new AggregationController(mockUsersService.Object);

            var result = await sut.GetAggregatedDatasets();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Dataset>>(objectResult.Value);
            var modelCount = model.Count();
            Assert.Equal(0, modelCount);
        }
    }
}
