using AggregationAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AggregationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregationController : ControllerBase
    {
        private readonly IAggregationRepository _aggregationRepository;

        public AggregationController(IAggregationRepository aggregationRepository)
        {
            _aggregationRepository = aggregationRepository;
        }

        [HttpGet("GetAggregatedDatasets")]
        public async Task<IActionResult> GetAggregatedDatasets()
        {
            var aggregatedDatasets = await _aggregationRepository.GetAggregatedDatasets();
            if (aggregatedDatasets != null) return Ok(aggregatedDatasets);
            else
                return NotFound();
        }

        [HttpPost("AggregateDatasets")]
        public async Task<IActionResult> AggregateDatasets()
        {
            var aggregateDatasets = await _aggregationRepository.AggregateDatasets();
            if (aggregateDatasets != null) return Ok(aggregateDatasets);
            else
                return NotFound();
        }
    }
}
