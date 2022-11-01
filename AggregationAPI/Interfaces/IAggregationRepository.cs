using AggregationAPI.Models;
using HtmlAgilityPack;

namespace AggregationAPI.Interfaces
{
    public interface IAggregationRepository
    {
        public Task<List<Dataset>> GetAggregatedDatasets();
        public Task<List<Dataset>> AggregateDatasets();
        public Task<HtmlNodeCollection> GetHtmlNodes();
        public Task<List<Dataset>> GetDatasets(HtmlNodeCollection htmlNodes);
        Task<IEnumerable<Dataset>> FilterBy(List<Dataset> datasets, string keyword);
        public Task<List<Dataset>> GroupBy(IEnumerable<Dataset> filteredDatasets);
    }
}
