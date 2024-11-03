using ApiClinicaV1.Dtos;
using ApiClinicaV1.Models;
using ApiClinicaV1.Models.config;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaV1.Services
{
    public class MedicalInformacionServices

    {

        private readonly AppDbContext _context;


        public MedicalInformacionServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalInformationDto> AddMedicalInformation(MedicalInformationDto medicalInfoDto)
        {
            var medicalInfo = new MedicalInformation
            {
                IdUser = medicalInfoDto.UserId,
                Weight = medicalInfoDto.Weight,
                Height = medicalInfoDto.Height,
                Temperature = medicalInfoDto.Temperature,
                BloodPressure = medicalInfoDto.BloodPressure,
                HeartRate = medicalInfoDto.HeartRate
            };

            _context.MedicalInformations.Add(medicalInfo);
            await _context.SaveChangesAsync();

            medicalInfoDto.Id = medicalInfo.Id;
            return medicalInfoDto;
        }

        public async Task<IEnumerable<MedicalInformationDto>> GetMedicalInformationByUserId(int userId)
        {
            return await _context.MedicalInformations
                .Where(m => m.IdUser == userId)
                .Select(m => new MedicalInformationDto
                {
                    Id = m.Id,
                    UserId = m.IdUser,
                    Weight = m.Weight,
                    Height = m.Height,
                    Temperature = m.Temperature,
                    BloodPressure = m.BloodPressure,
                    HeartRate = m.HeartRate
                })
                .ToListAsync();
        }
    }
}
