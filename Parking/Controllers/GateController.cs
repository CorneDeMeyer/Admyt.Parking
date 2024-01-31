using Parking.DomainLogic.Interface;
using Microsoft.AspNetCore.Mvc;
using Parking.Domain.Models;

namespace Parking.API.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class GateController(IGateService gateService) : Controller
    {
        private readonly IGateService _gateService = gateService;

        [HttpGet("calculate-fee")] 
        public async Task<IActionResult> Get([FromQuery] string plateText) 
        {
            if (!string.IsNullOrWhiteSpace(plateText))
            {
                var result = await _gateService.CalculateFee(plateText);

                if (result.Errors.Count == 0)
                {
                    return Ok(result.Value);
                }
                else
                {
                    return BadRequest(string.Join("; ", result.Errors));
                }
            }

            return BadRequest($"Could not calculate fee for plate {plateText}, please try again.");
        }

        [HttpPost("gate-events")]
        public async Task<IActionResult> Post([FromBody] GateEvent gateEvent)
        {
            if (gateEvent != null)
            {
                var result = await _gateService.ProcessGateRequest(gateEvent);

                if (result.Errors.Count == 0)
                {
                    return Ok(result.Value);
                }
                else
                {
                    return BadRequest(string.Join("; ", result.Errors));
                }
            }

            return BadRequest($"Could not Process event for gate {gateEvent?.GateId.ToString()} and plate {gateEvent?.PlateText}");
        }
    }
}
