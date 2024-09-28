using CarModel.BusinessLayer;
using CarModel.DataAccessLayer.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarModelManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommissionController : ControllerBase
    {
        private readonly ICommissionBusinessLayer _commissionBusinessLayer;

        public CommissionController(ICommissionBusinessLayer commissionService)
        {
            _commissionBusinessLayer = commissionService;
        }

        [HttpPost("generate-report")]
        public async Task<IActionResult> CalculateCommission([FromBody] CommissionRequest request)
        {
            try
            {
                var commission = await _commissionBusinessLayer.GenerateCommissionReportAsync(request);
                if (commission == null)
                {
                    return BadRequest("Invalid request data");
                }
                return Ok(commission);
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Error generating commission report: {ex.Message}");
                 
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
