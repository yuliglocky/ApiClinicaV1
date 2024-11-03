using ApiClinicaV1.Dtos;
using ApiClinicaV1.Models;
using ApiClinicaV1.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiClinicaV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloodExamenController : ControllerBase
    {
        private readonly IBloodExamService _bloodExamService;

        public BloodExamenController(IBloodExamService bloodExamService)
        {
            _bloodExamService = bloodExamService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBloodExam([FromBody] BloodExamDto bloodExamDto)
        {
            if (bloodExamDto == null)
            {
                return BadRequest("Invalid blood exam data.");
            }

            var createdExam = await _bloodExamService.CreateBloodExamAsync(bloodExamDto);
            return CreatedAtAction(nameof(GetBloodExams), new { id = createdExam.Id }, createdExam);
        }

        [HttpGet]
        public async Task<IActionResult> GetBloodExams()
        {
            var bloodExams = await _bloodExamService.GetBloodExamsAsync();
            return Ok(bloodExams);
        }
    }
}