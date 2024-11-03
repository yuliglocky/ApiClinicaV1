using ApiClinicaV1.Dtos;

using ApiClinicaV1.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiClinicaV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentMedicineController : ControllerBase
    {
        private readonly AppointmentMedicineService _service;

        public AppointmentMedicineController(AppointmentMedicineService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentMedicineDto dto)
        {
            var result = await _service.CreateAppointmentMedicineAsync(dto);

            if (!result)
            {
                return BadRequest("Cita no encontrada o medicamento no disponible.");
            }

            return Ok("Cita y medicamento creados exitosamente.");
        }

        [HttpGet("user/{userId}/attended-appointments")]
        public async Task<IActionResult> GetAttendedAppointmentsByUserId(int userId)
        {
            var attendedAppointments = await _service.GetAttendedAppointmentsByUserIdAsync(userId);

            if (attendedAppointments == null || attendedAppointments.Count == 0)
            {
                return NotFound("No se encontraron citas atendidas para este usuario.");
            }

            return Ok(attendedAppointments);
        }

    }
}