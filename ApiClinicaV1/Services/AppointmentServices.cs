namespace ApiClinicaV1.Services
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using global::ApiClinicaV1.Dtos;
    using global::ApiClinicaV1.Models.config;
    using global::ApiClinicaV1.Models;

    public interface IAppointmentService
    {
        Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId);
        Task<AppointmentDto> CreateAppointmentAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusesAsync(IEnumerable<int> statuses);
        Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(int userId);
        Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(int status);
        Task UpdateAppointmentAsync(Appointment appointment);
    
        Task CancelAppointmentsByUserAsync(int userId, int status);
    }

    public class AppointmentServices : IAppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Where(a => a.Id == appointmentId)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    UserId = a.IdUser,
                    DoctorId = a.IdDoctor,
                    Reason = a.Reason,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    Notes = a.Notes
                })
                .FirstOrDefaultAsync();

            return appointment;
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(Appointment appointment)
        {
            // Agregar la cita al contexto
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos

            // Mapear la cita a un AppointmentDto para devolver
            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                UserId = appointment.IdUser,
                DoctorId = appointment.IdDoctor,
                Reason = appointment.Reason,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes
            };

            return appointmentDto; // Retornar el DTO de la cita creada
        }
        public
            async Task<IEnumerable<Appointment>> GetAppointmentsByStatusesAsync(IEnumerable<int> statuses)
        {
            return await _context.Appointments
                .Where(a => statuses.Contains(a.Status))
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetUserAppointmentsAsync(int userId)
        {
            return await _context.Appointments
                .Where(a => a.IdUser == userId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(int status)
        {
            return await _context.Appointments
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task CancelAppointmentsByUserAsync(int userId, int status)
        {
            // Obtener las citas del usuario que están en estado 0
            var appointments = await GetUserAppointmentsAsync(userId);
            var appointmentsToCancel = appointments.Where(a => a.Status == status).ToList();

            foreach (var appointment in appointmentsToCancel)
            {
                appointment.Status = 4; // Asumimos que 2 es el estado de "cancelado"
                await UpdateAppointmentAsync(appointment);
            }
        }
    }
}

