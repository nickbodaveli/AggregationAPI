using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AggregationAPI.Models;
using AggregationAPI.UnitTests.System.Helpers;
using AggregationAPI.Repository;
using AggregationAPI.Interfaces;
using AggregationAPI.Context;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace AggregationAPI.UnitTests.System.Services
{
    public class TestAggregationService
    {
        [Fact]
        public async Task Get_Datasets_Result()
        {
            // Arrange
            var mockUsersService = new Mock<IAggregationRepository>();

            mockUsersService
               .Setup(service => service.GetAggregatedDatasets())
               .ReturnsAsync(MockData.MockDatasets());

            var result = await mockUsersService.Object.GetAggregatedDatasets();

            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task Get_Empty_Datasets_Result()
        {
            // Arrange
            var mockUsersService = new Mock<IAggregationRepository>();

            mockUsersService
               .Setup(service => service.GetAggregatedDatasets())
               .ReturnsAsync(new List<Dataset> { });

            var result = await mockUsersService.Object.GetAggregatedDatasets();


            Assert.Equal(0, result.Count);
        }


        [Fact]
        public async Task Datasets_To_Be_Of_Type_Dataset()
        {
            var mockUsersService = new Mock<IAggregationRepository>();
            mockUsersService
                .Setup(service => service.GetAggregatedDatasets())
                .ReturnsAsync(MockData.MockDatasets());

            var result = await mockUsersService.Object.GetAggregatedDatasets();

            result.Should().BeOfType<List<Dataset>>();
        }


        [Fact]
        public async Task HtmlNode_For_Datasets_IsNull()
        {
            var mockUsersService = new Mock<IAggregationRepository>();

            string nodes = "//a";

            var doc = new HtmlDocument();

            HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes(nodes);

            var result = await mockUsersService.Object.GetDatasets(htmlNodes);

            result.Should().BeNull();
        }

        [Fact]
        public async Task HtmlNodes_IsNotNull()
        {
            var mockUsersService = new Mock<IAggregationRepository>();

            var result = await mockUsersService.Object.GetHtmlNodes();

            Assert.Null(result);
        }


        [Fact]
        public async Task HtmlNodes_IsNull()
        {
            var mockUsersService = new Mock<IAggregationRepository>();

            string nodes = "//a";

            var doc = new HtmlDocument();

            HtmlNodeCollection htmlNodes = doc.DocumentNode.SelectNodes(nodes);


            mockUsersService
                .Setup(service => service.GetHtmlNodes())
                .ReturnsAsync(
                    htmlNodes
                );

            var result = await mockUsersService.Object.GetHtmlNodes();

            Assert.Null(result);
        }
    }
}
