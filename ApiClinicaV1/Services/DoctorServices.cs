using ApiClinicaV1.Dtos;
using ApiClinicaV1.Models.config;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaV1.Services
{
    public interface IDoctorService
    {
        Task<DoctorDto> GetDoctorByIdAsync(int doctorId);
    }

    public class DoctorServices : IDoctorService
    {
        private readonly AppDbContext _context;

        public DoctorServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int doctorId)
        {
            var doctor = await _context.Doctors
                 .Where(d => d.IdDoctor == doctorId)
                .Select(d => new DoctorDto
                {
           Id = d.IdDoctor,
           ServicioId = d.Servicio,
           ServicioName = d.Service.Name 
         })
          .FirstOrDefaultAsync();

           return doctor;

        }
    }
}

