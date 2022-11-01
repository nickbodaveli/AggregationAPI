using AggregationAPI.Context;
using AggregationAPI.Interfaces;
using System.Net.Http;
using System.Net;
using AggregationAPI.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Formats.Asn1;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using System;
using AggregationAPI.Controllers;
using System.Text;

namespace AggregationAPI.Repository
{
    public class AggregationRepository : IAggregationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly WebClient _webClient;
        private readonly HttpClient _httpClient;
        private readonly HtmlDocument _htmlDocument;
        private readonly ILogger<AggregationRepository> _logger;
        private readonly IConfiguration _configuration;

        public AggregationRepository(ApplicationDbContext context, 
            WebClient webClient, 
            HttpClient httpClient, 
            HtmlDocument htmlDocument,
            ILogger<AggregationRepository> logger,
            IConfiguration configuration
            )
        {
            _context = context;
            _webClient = webClient;
            _httpClient = httpClient;
            _htmlDocument = htmlDocument;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<List<Dataset>> GetAggregatedDatasets()
        {
            var result = await _context.Datasets.ToListAsync();
            _logger.LogInformation("Selecting Datasets ...");
            if (result != null)
            {
                _logger.LogInformation("Datasets have successfully selected");
                return result;
            }

            return null;
        }

        public async Task<List<Dataset>> AggregateDatasets()
        {
            var htmlNodes = await GetHtmlNodes();

            if (htmlNodes != null)
            {
                try
                {
                    var datasets = await GetDatasets(htmlNodes);

                    var filterByName = await FilterBy(datasets, "Butas");

                    var groupByRegion = await GroupBy(filterByName);

                    _context.Datasets.AddRange(groupByRegion);

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Grouped Datasets have successfully stored in Database");

                    return groupByRegion;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return null;
        }

        public async Task<HtmlNodeCollection> GetHtmlNodes()
        {
            _logger.LogInformation("Sending Request ...");
            string url = _configuration["Keys:SendUrl"];
            _webClient.Encoding = System.Text.Encoding.UTF8;
            string html = _webClient.DownloadString(url);
            if (!string.IsNullOrEmpty(html))
            {
                _htmlDocument.LoadHtml(html);
                if (_htmlDocument != null)
                {
                    try
                    {
                        string nodes = _configuration["Keys:HtmlNodes"];

                        HtmlNodeCollection htmlNodes = _htmlDocument.DocumentNode.SelectNodes(nodes);
                        _logger.LogInformation("Recieved Response : ");
                        return htmlNodes;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return null;
        }

        public async Task<List<Dataset>> GetDatasets(HtmlNodeCollection htmlNodes)
        {
            if(htmlNodes != null)
            {
                try
                {
                    var allDatasets = new List<Dataset>();

                    var nodesCount = htmlNodes.Count;

                    for (int i = nodesCount - 2; i <= nodesCount - 1; i++)
                    {
                        string previewId = htmlNodes[i].Attributes["id"].Value;

                        var url = $"{_configuration["Keys:PreviewUrl"]}/{previewId}";
                        _logger.LogInformation($"Response from preview id with {previewId}");

                        var request = await _httpClient.GetAsync(url);
                        var _response = request.Content.ReadAsStringAsync().Result;

                        var datasetObject = JsonConvert.DeserializeObject<List<List<string>>>(_response);

                        for (int j = 1; j <= datasetObject.Count - 1; j++)
                        {
                            allDatasets.Add(
                                    new Dataset()
                                    {
                                        Region = datasetObject[j][0],
                                        ObjName = datasetObject[j][1],
                                        ObjType = datasetObject[j][2],
                                        ObjNumber = int.Parse(datasetObject[j][3]),
                                        PPlus = (datasetObject[j][4] == null) ? 0 : double.Parse(datasetObject[j][4]),
                                        PlTime = DateTime.Parse(datasetObject[j][5]),
                                        PMinus = (datasetObject[j][6] == null) ? 0 : double.Parse(datasetObject[j][6]),
                                    }
                                );
                            _logger.LogInformation($"Adding Datasets To List with Object at : {j}" );
                        }
                    }

                    return allDatasets;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return null;
        }

        public async Task<IEnumerable<Dataset>> FilterBy(List<Dataset> datasets, string keyword)
        {
            _logger.LogInformation($"Started Datasets filtering By Butas");
            var filterByButas = datasets.Where(objName => objName.ObjName == keyword);

            if (filterByButas != null)
            {
                _logger.LogInformation($"Filter has finished successfully");
                return filterByButas;
            }

            return null;
        }

        public async Task<List<Dataset>> GroupBy(IEnumerable<Dataset> filteredDatasets)
        {
            try
            {
                _logger.LogInformation($"Started Datasets Grouping By Region");
                var groupByRegion = filteredDatasets.GroupBy(item => item.Region)
                                 .Select(grouping => grouping.FirstOrDefault())
                                 .OrderBy(item => item.Region)
                                 .ToList();
                _logger.LogInformation($"Group has finished successfully");
                return groupByRegion;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
