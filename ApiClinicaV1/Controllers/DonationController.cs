using ApiClinicaV1.Dtos;
using ApiClinicaV1.Services;
using Microsoft.AspNetCore.Mvc;



namespace ApiClinicaV1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly DonationServices _donationService;

        public DonationController(DonationServices donationService)
        {
            _donationService = donationService;
        }


        [HttpPost]
        public async Task<ActionResult<DonationDto>> AddDonation(DonationDto donationDto)
        {
            var donation = await _donationService.AddDonation(donationDto);
            return CreatedAtAction(nameof(GetDonationsByUserId), new { userId = donation.UserId }, donation);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<DonationDto>>> GetDonationsByUserId(int userId)
        {
            var donations = await _donationService.GetDonationsByUserId(userId);
            if (donations == null || !donations.Any())
            {
                return NotFound($"No donations found for user with ID {userId}.");
            }

            return Ok(donations);
        }
    }
}
 