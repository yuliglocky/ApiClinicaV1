using ApiClinicaV1.Dtos;
using ApiClinicaV1.Models.config;
using ApiClinicaV1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace ApiClinicaV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Doctor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetDoctor(int id)
        {
            var doctor = await _context.Users
                .Where(u => u.Role == 2) // Verificar que el rol sea el de doctor
                .FirstOrDefaultAsync(u => u.Id == id);

            if (doctor == null)
            {
                return NotFound();
            }

            var doctorDto = new UserDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Dni = doctor.Dni,
                Cellphone = doctor.Cellphone,
                Gender = doctor.Gender,
                Blood = doctor.Blood,
                Email = doctor.Email,
                Allergies = doctor.Allergies,
                Role = doctor.Role,
                Birthday = doctor.Birthday,
                Password = doctor.Password,
                Address = doctor.Address,
                IsDonor = doctor.IsDonor
            };

            return Ok(doctorDto);
        }

        [HttpPost("registerDoctor")]
        public async Task<ActionResult<UserDto>> PostDoctor([FromBody] UserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si el correo ya está en uso
                var existingDoctor = await _context.Users.AnyAsync(u => u.Email == dto.Email);
                if (existingDoctor)
                {
                    return Conflict("El correo electrónico ya está registrado.");
                }

                var existingDoctorByDni = await _context.Users.AnyAsync(u => u.Dni == dto.Dni);
                if (existingDoctorByDni)
                {
                    return Conflict("El DNI ya está registrado.");
                }

                // Cifrar la contraseña
                var hashedPassword = HashPassword(dto.Password);

                // Crear una nueva instancia del modelo User para el doctor
                var doctor = new User
                {
                    Name = dto.Name,
                    Dni = dto.Dni,
                    Cellphone = dto.Cellphone,
                    Gender = dto.Gender,
                    Blood = dto.Blood,
                    Email = dto.Email,
                    Allergies = dto.Allergies,
                    Role = 2, // Asignar rol de doctor
                    Password = hashedPassword, // Usar la contraseña cifrada
                    Birthday = dto.Birthday,
                    IsDonor = dto.IsDonor,
                    Address = dto.Address
                };

                _context.Users.Add(doctor);
                await _context.SaveChangesAsync();

                dto.Id = doctor.Id;
                dto.Role = 2;

                return CreatedAtAction(nameof(GetDoctor), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                // Agregar un logger para registrar el error
                // _logger.LogError(ex, "Error registrando doctor");
                return StatusCode(500, "Ocurrió un error al registrar el doctor.");
            }
        }

        // POST: api/Doctor/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            // Buscar el doctor por su correo electrónico y rol
            var doctor = await _context.Users
                .Where(u => u.Email == loginDto.Email && u.Role == 2) // Verificar que sea doctor (rol 2)
                .FirstOrDefaultAsync();

            if (doctor == null)
            {
                return NotFound("Doctor no encontrado o rol no válido.");
            }

            // Validar la contraseña
            if (doctor.Password != HashPassword(loginDto.Password))
            {
                return Unauthorized("Contraseña incorrecta.");
            }

            // Si la validación es exitosa, devolver el UserId y su rol
            return Ok(new
            {
                message = "Login exitoso",
                userId = doctor.Id, // Devuelve el UserId
                role = doctor.Role // Opcional: devuelve el rol
            });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        // GET: api/Doctor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Service)
                .Select(d => new DoctorDto
                {
                    Id = d.IdDoctor,
                    ServicioId= d.Servicio,
                    ServicioName = d.Service.Name
                })
                .ToListAsync();

            return Ok(doctors);
        }

        // POST: api/Doctor
        [HttpPost]
        public async Task<ActionResult<Doctor>> CreateDoctor(DoctorDto doctorDto)
        {
            var doctor = new Doctor
            {
                Servicio = doctorDto.ServicioId,
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctors), new { id = doctor.IdDoctor }, doctor);
        }
    }
}
   
