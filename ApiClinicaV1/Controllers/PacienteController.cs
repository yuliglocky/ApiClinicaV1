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
    public class PacienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PacienteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Paciente/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Dni = user.Dni,
                Cellphone = user.Cellphone,
                Gender = user.Gender,
                Blood = user.Blood,
                Email = user.Email,
                Allergies = user.Allergies,
                Role = user.Role,
                Birthday = user.Birthday,
                Password = user.Password,
                Address = user.Address, 
                IsDonor = user.IsDonor,
             
            };

            return Ok(userDto);
        }

        [HttpPost("registerPaciente")]
        public async Task<ActionResult<UserDto>> PostPaciente([FromBody] UserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si el correo ya está en uso
                var existingUser = await _context.Users.AnyAsync(u => u.Email == dto.Email);
                if (existingUser)
                {
                    return Conflict("El correo electrónico ya está registrado.");
                }

                var existingUserByDni = await _context.Users.AnyAsync(u => u.Dni == dto.Dni);
                if (existingUserByDni)
                {
                    return Conflict("El DNI ya está registrado.");
                }

                // Cifrar la contraseña (si implementas hashing)
                var hashedPassword = HashPassword(dto.Password);

                // Crear una nueva instancia del modelo User
                var user = new User
                {
                    Name = dto.Name,
                    Dni = dto.Dni,
                    Cellphone = dto.Cellphone,
                    Gender = dto.Gender,
                    Blood = dto.Blood,
                    Email = dto.Email,
                    Allergies = dto.Allergies,
                    Role = 1,
                    Password = dto.Password, // Usar la contraseña cifrada
                    Birthday = dto.Birthday,
                    IsDonor = dto.IsDonor,
                    Address = dto.Address,
                  
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                dto.Id = user.Id;
                dto.Role = 1;

                return CreatedAtAction(nameof(GetUser), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                // Agregar un logger para registrar el error
                // _logger.LogError(ex, "Error registrando paciente");
                return StatusCode(500, "Ocurrió un error al registrar el paciente.");
            }
        }

        // POST: api/Paciente/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            // Buscar el usuario por su correo electrónico
            var user = await _context.Users
                .Where(u => u.Email == loginDto.Email && u.Role == 1) // Verificar que sea paciente (rol 1)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("Usuario no encontrado o rol no válido.");
            }

            // Validar la contraseña
            if (user.Password != loginDto.Password)
            {
                return Unauthorized("Contraseña incorrecta.");
            }

            // Si la validación es exitosa, devolver el UserId y su rol
            return Ok(new
            {
                message = "Login exitoso",
                userId = user.Id, // Devuelve el UserId
                role = user.Role,// Opcional: devuelve el rol
                name = user.Name,
                email= user.Email,
            });
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

}



