using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiClinicaV1.Dtos;
using ApiClinicaV1.Models;
using ApiClinicaV1.Models.config;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaV1.Services
{
    public interface IBloodExamService
    {
        Task<BloodExam> CreateBloodExamAsync(BloodExamDto bloodExamDto); // Cambié el parámetro a BloodExamDto
        Task<List<BloodExamDto>> GetBloodExamsAsync();
        bool CheckIfBloodExamIsGood(BloodExamDto bloodExamDto);
    }

    public class BloodExamServices : IBloodExamService
    {
        private readonly AppDbContext _context;

        public BloodExamServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BloodExam> CreateBloodExamAsync(BloodExamDto bloodExamDto)
        {
            var bloodExam = new BloodExam
            {
                IdUser = bloodExamDto.UserId,
                Glucose = bloodExamDto.Glucose,
                RedCells = bloodExamDto.RedCells,
                Hemoglobin = bloodExamDto.Hemoglobin,
                WhiteCells = bloodExamDto.WhiteCells,
                Platelets = bloodExamDto.Platelets,
                BloodPressure = bloodExamDto.BloodPressure,
                Cholesterol = bloodExamDto.Cholesterol,
                CreatedAt = DateTime.UtcNow, // Establecer la fecha de creación
                IsGood = CheckIfBloodExamIsGood(bloodExamDto) // Lógica para determinar si es bueno
            };

            _context.BloodExams.Add(bloodExam);
            await _context.SaveChangesAsync();
            return bloodExam;
        }


        public bool CheckIfBloodExamIsGood(BloodExamDto bloodExamDto)
        {
            bool isGood = true;

            // Verificación de glucosa
            if (bloodExamDto.Glucose < 70 || bloodExamDto.Glucose > 100)
            {
                isGood = false;
            }

            // Verificación de colesterol
            if (bloodExamDto.Cholesterol >= 200)
            {
                isGood = false;
            }

            // Verificación de hemoglobina (puedes ajustar según el género)
            // Suponiendo que IdUser puede usarse para obtener información del usuario
            var user = _context.Users.Find(bloodExamDto.UserId); // Obtener usuario por Id
            if (user != null)
            {
                if ((user.Gender == 1 && (bloodExamDto.Hemoglobin < 13.5 || bloodExamDto.Hemoglobin > 17.5)) ||
                    (user.Gender == 2 && (bloodExamDto.Hemoglobin < 12.0 || bloodExamDto.Hemoglobin > 15.5)))
                {
                    isGood = false;
                }
            }

            // Verificación de presión arterial (debe ser un formato específico, asumiendo que se guarda como un entero)
            // Considerando que BloodPressure almacena un valor de "sistolica/diastolica", como 120/80
            var bloodPressureParts = bloodExamDto.BloodPressure.ToString().Split('/');
            if (bloodPressureParts.Length == 2)
            {
                if (int.TryParse(bloodPressureParts[0], out int systolic) && int.TryParse(bloodPressureParts[1], out int diastolic))
                {
                    if (systolic >= 120 || diastolic >= 80)
                    {
                        isGood = false;
                    }
                }
            }
            else
            {
                isGood = false; // Formato incorrecto
            }

            return isGood;
        }
        public async Task<List<BloodExamDto>> GetBloodExamsAsync()
        {
            return await _context.BloodExams
                .Include(be => be.User) // Cargar la relación User
                .Select(be => new BloodExamDto
                {
                    Id = be.Id,
                    UserId = be.IdUser,
                    UserName = be.User.Name,
                    Glucose = be.Glucose,
                    RedCells = be.RedCells,
                    Hemoglobin = be.Hemoglobin,
                    WhiteCells = be.WhiteCells,
                    Platelets = be.Platelets,
                    BloodPressure = be.BloodPressure,
                    Cholesterol = be.Cholesterol,
                    CreatedAt = be.CreatedAt,
                    IsGood = be.IsGood,
                })
                .ToListAsync();
        }
    }
}