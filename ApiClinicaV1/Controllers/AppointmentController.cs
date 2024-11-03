using ApiClinicaV1.Dtos;

using ApiClinicaV1.Services;
using Microsoft.AspNetCore.Mvc;
using ApiClinicaV1.Models;

namespace ApiClinicaV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;

        public AppointmentController(IAppointmentService appointmentService, IUserService userService, IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _doctorService = doctorService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync(appointment.UserId);
            var doctor = await _doctorService.GetDoctorByIdAsync(appointment.DoctorId);

            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                UserId = appointment.UserId,
                UserName = user.Name,
                DoctorId = appointment.DoctorId,
                DoctorServiceName = doctor.ServicioName,
                Reason = appointment.Reason,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes
            };

            return Ok(appointmentDto);
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointment(AppointmentCreateDto appointmentCreateDto)
        {
            // Verificar que el usuario exista
            var user = await _userService.GetUserByIdAsync(appointmentCreateDto.UserId);
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Verificar que el doctor exista
            var doctor = await _doctorService.GetDoctorByIdAsync(appointmentCreateDto.DoctorId);
            if (doctor == null)
            {
                return NotFound("Doctor no encontrado.");
            }

            // Crear la cita
            var appointment = await _appointmentService.CreateAppointmentAsync(new Appointment
            {
                IdUser = appointmentCreateDto.UserId,
                IdDoctor = appointmentCreateDto.DoctorId,
                Reason = appointmentCreateDto.Reason,
                AppointmentDate = appointmentCreateDto.AppointmentDate,
                Status = 0,
                Notes= appointmentCreateDto.Notes
            });

            // Construir el DTO de respuesta
            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                UserId = appointment.UserId,
                UserName = user.Name,
                DoctorId = appointment.DoctorId,
                DoctorServiceName = doctor.ServicioName,
                Reason = appointment.Reason,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes
            };

            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentDto.Id }, appointmentDto);
        }

        [HttpGet("status-0")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsWithStatus0()
        {
            var appointments = await _appointmentService.GetAppointmentsByStatusAsync(0);
            var appointmentDtos = new List<AppointmentDto>();

            foreach (var appointment in appointments)
            {
                var user = await _userService.GetUserByIdAsync(appointment.IdUser);
                var doctor = await _doctorService.GetDoctorByIdAsync(appointment.IdDoctor);
                appointmentDtos.Add(new AppointmentDto
                {
                    Id = appointment.Id,
                    UserId = appointment.IdUser,
                    UserName = user.Name,
                    DoctorId = appointment.IdDoctor,
                    DoctorServiceName = doctor.ServicioName,
                    Reason = appointment.Reason,
                    AppointmentDate = appointment.AppointmentDate,
                    Status = appointment.Status,
                    Notes = appointment.Notes
                });
            }

            return Ok(appointmentDtos);
        }

        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, [FromBody] int status)
        {
         
            // Obtener el AppointmentDto por ID
            var appointmentDto = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointmentDto == null)
            {
                return NotFound("Cita no encontrada.");
            }

            // Verificar si la cita está en estado pendiente (asumiendo que el estado pendiente es 0)
            if (appointmentDto.Status != 0)
            {
                return BadRequest("Solo se pueden actualizar citas pendientes.");
            }

            // Mapear AppointmentDto a Appointment
            var appointment = new Appointment
            {
                Id = appointmentDto.Id,
                IdUser = appointmentDto.UserId,
                IdDoctor = appointmentDto.DoctorId,
                Reason = appointmentDto.Reason,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = status, // Actualizar el estado aquí
                Notes = appointmentDto.Notes
            };

            // Actualizar la cita en la base de datos
            await _appointmentService.UpdateAppointmentAsync(appointment);

            return Ok("El estado de la cita se ha actualizado correctamente.");
        }


        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetApprovedAppointments()
        {
            var appointments = await _appointmentService.GetAppointmentsByStatusesAsync(new[] { 1 });
            var appointmentDtos = new List<AppointmentDto>();

            foreach (var appointment in appointments)
            {
                var user = await _userService.GetUserByIdAsync(appointment.IdUser);
                var doctor = await _doctorService.GetDoctorByIdAsync(appointment.IdDoctor);
                appointmentDtos.Add(new AppointmentDto
                {
                    Id = appointment.Id,
                    UserId = appointment.IdUser,
                    UserName = user.Name,
                    DoctorId = appointment.IdDoctor,
                    DoctorServiceName = doctor.ServicioName,
                    Reason = appointment.Reason,
                    AppointmentDate = appointment.AppointmentDate,
                    Status = appointment.Status,
                    Notes = appointment.Notes
                });
            }

            return Ok(appointmentDtos);
        }

        [HttpGet("finish")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetFinishedAppointments()
        {
            var appointments = await _appointmentService.GetAppointmentsByStatusesAsync(new[] { 1 });
            var appointmentDtos = new List<AppointmentDto>();

            foreach (var appointment in appointments)
            {
                var user = await _userService.GetUserByIdAsync(appointment.IdUser);
                var doctor = await _doctorService.GetDoctorByIdAsync(appointment.IdDoctor);
                appointmentDtos.Add(new AppointmentDto
                {
                    Id = appointment.Id,
                    UserId = appointment.IdUser,
                    UserName = user.Name,
                    DoctorId = appointment.IdDoctor,
                    DoctorServiceName = doctor.ServicioName,
                    Reason = appointment.Reason,
                    AppointmentDate = appointment.AppointmentDate,
                    Status = appointment.Status,
                    Notes = appointment.Notes
                });
            }

            return Ok(appointmentDtos);
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetUserAppointments(int userId)
        {
            var appointments = await _appointmentService.GetUserAppointmentsAsync(userId);
            var appointmentDtos = new List<AppointmentDto>();

            foreach (var appointment in appointments)
            {
                var user = await _userService.GetUserByIdAsync(appointment.IdUser);
                var doctor = await _doctorService.GetDoctorByIdAsync(appointment.IdDoctor);
                appointmentDtos.Add(new AppointmentDto
                {
                    Id = appointment.Id,
                    UserId = appointment.IdUser,
                    UserName = user.Name,
                    DoctorId = appointment.IdDoctor,
                    DoctorServiceName = doctor.ServicioName,
                    Reason = appointment.Reason,
                    AppointmentDate = appointment.AppointmentDate,
                    Status = appointment.Status,
                    Notes = appointment.Notes
                });
            }

            return Ok(appointmentDtos);
        }


        [HttpDelete("user/{userId}/cancel")]
        public async Task<ActionResult> CancelUserAppointments(int userId)
        {
            // Obtener las citas del usuario que están en estado 0
            var appointments = await _appointmentService.GetUserAppointmentsAsync(userId);
            var appointmentsToCancel = appointments.Where(a => a.Status == 0).ToList();

            if (!appointmentsToCancel.Any())
            {
                return NotFound("No hay citas en estado 0 para cancelar.");
            }

            // Cancelar (puedes optar por eliminar o simplemente actualizar el estado)
            foreach (var appointment in appointmentsToCancel)
            {
                appointment.Status = 3; // Asumimos que 2 es el estado de "cancelado"
                await _appointmentService.UpdateAppointmentAsync(appointment);
            }

            return NoContent();
        }



    }
}
