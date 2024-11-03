using ApiClinicaV1.Dtos;
using ApiClinicaV1.Models;
using ApiClinicaV1.Models.config;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaV1.Services
{
    public class DonationServices
    {
        private readonly AppDbContext _context;


        public DonationServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DonationDto> AddDonation(DonationDto donationDto)
        {
            var donation = new Donation
            {
                UserId = donationDto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            donationDto.Id = donation.Id;
            donationDto.CreatedAt = donation.CreatedAt;

            return donationDto;
        }

        public async Task<IEnumerable<DonationDto>> GetDonationsByUserId(int userId)
        {
            return await _context.Donations
                .Where(d => d.UserId == userId)
                .Select(d => new DonationDto
                {
                    Id = d.Id,
                    UserId = d.UserId,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();
        }
    }
}
}
