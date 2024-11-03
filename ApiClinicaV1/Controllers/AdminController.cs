using ApiClinicaV1.Dtos;
using ApiClinicaV1.Models.config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Admin/{id}
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
                IsDonor = user.IsDonor,
            
            };

            return Ok(userDto);
        }

        // POST: api/Admin/registerAdmin
        [HttpPost("registerAdmin")]
        public async Task<ActionResult<UserDto>> PostAdmin(UserDto dto)
        {
            // Verificar si el correo ya está en uso
            var existingUser = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (existingUser)
            {
                return Conflict("El correo electrónico ya está registrado.");
            }

            // Verificar si el DNI ya está en uso
            var existingUserByDni = await _context.Users.AnyAsync(u => u.Dni == dto.Dni);
            if (existingUserByDni)
            {
                return Conflict("El DNI ya está registrado.");
            }

            // Crear una nueva instancia del modelo User, asignando los valores del DTO y el rol predeterminado



            // Devolver la respuesta HTTP 201 con el nuevo recurso creado
            return CreatedAtAction(nameof(GetUser), new { id = dto.Id }, dto);
        }

        // POST: api/Admin/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            // Buscar el usuario por su correo electrónico
            var user = await _context.Users
                .Where(u => u.Email == loginDto.Email && u.Role == 3 ) // Verificar que sea admin (rol 3)
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

            // Si la validación es exitosa, devolver la URL con su ID y su rol de admin
            var redirectUrl = $"/api/Admin/{user.Id}/role/{user.Role}";

            return Ok(new { message = "Login exitoso", url = redirectUrl });
        }
    }
}


