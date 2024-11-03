using ApiClinicaV1.Dtos;
using ApiClinicaV1.Models;
using ApiClinicaV1.Models.config;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaV1.Services
{
    public class AppointmentMedicineService
    {
        private readonly AppDbContext _context;

        public AppointmentMedicineService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAppointmentMedicineAsync(AppointmentMedicineDto dto)
        {
            // Verificar si el usuario tiene la cita aprobada
            var appointment = await _context.Appointments
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == dto.AppointmentId && a.IdUser == dto.UserId && a.Status == 1); // Estado 1 representa aprobado

            if (appointment == null)
            {
                return false; // Cita no encontrada para el usuario o no está aprobada
            }

            // Verificar si el medicamento existe y tiene suficiente cantidad
            var medicine = await _context.Medicines.FindAsync(dto.MedicineId);
            if (medicine == null || medicine.Quantity < dto.Quantity)
            {
                return false; // Medicamento no encontrado o cantidad insuficiente
            }

            // Crear la relación AppointmentMedicine
            var appointmentMedicine = new AppointmentMedicine
            {
                AppointmentId = dto.AppointmentId,
                MedicinesId = dto.MedicineId,
                Quantity = dto.Quantity
            };

            // Agregar a la base de datos
            _context.AppointmentMedicines.Add(appointmentMedicine);

            // Disminuir la cantidad del medicamento
            medicine.Quantity -= dto.Quantity;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<AttendedAppointmentDto>> GetAttendedAppointmentsByUserIdAsync(int userId)
        {
            var attendedAppointments = await _context.AppointmentMedicines
                .Include(am => am.Appointment)  // Incluir la cita
                .Include(am => am.Medicines)    // Incluir el medicamento
                .Where(am => am.Appointment.IdUser == userId) // Filtrar por UserId
                .Select(am => new AttendedAppointmentDto
                {
                    AppointmentId = am.AppointmentId,
                    AppointmentDate = am.Appointment.AppointmentDate.ToString("yyyy-MM-dd"), // Ajustar según el formato deseado
                    MedicineId = am.MedicinesId,
                    MedicineName = am.Medicines.Name, // Asumiendo que tu modelo Medicine tiene una propiedad Name
                    Quantity = am.Quantity
                })
                .ToListAsync();

            return attendedAppointments;
        }

    }
}