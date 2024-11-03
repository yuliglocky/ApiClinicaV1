using ApiClinicaV1.Dtos;
using ApiClinicaV1.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MedicalInformacionController : ControllerBase
{
    private readonly  MedicalInformacionServices _medicalInformationService;

    public MedicalInformacionController(MedicalInformacionServices medicalInformationService)
    {
        _medicalInformationService = medicalInformationService;
    }

    [HttpPost]
    public async Task<ActionResult<MedicalInformationDto>> AddMedicalInformation(MedicalInformationDto medicalInfoDto)
    {
        var medicalInfo = await _medicalInformationService.AddMedicalInformation(medicalInfoDto);
        return CreatedAtAction(nameof(GetMedicalInformationByUserId), new { userId = medicalInfo.UserId }, medicalInfo);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<MedicalInformationDto>>> GetMedicalInformationByUserId(int userId)
    {
        var medicalInfos = await _medicalInformationService.GetMedicalInformationByUserId(userId);
        if (medicalInfos == null || !medicalInfos.Any())
        {
            return NotFound($"No medical information found for user with ID {userId}.");
        }

        return Ok(medicalInfos);
    }
}

