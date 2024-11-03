using Microsoft.AspNetCore.Mvc;

using ApiClinicaV1.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ApiClinicaV1.Models.config;
using ApiClinicaV1.Dtos;

namespace ApiClinicaV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Servicio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetServicios()
        {
            var servicios = await _context.Servicios
                .Select(s => new ServicioDto
                {
                    Id = s.IdServicios,
                    Name = s.Name
                })
                .ToListAsync();

            return Ok(servicios);
        }

        // POST: api/Servicio
        [HttpPost]
        public async Task<ActionResult<Servicio>> CreateServicio(ServicioDto servicioDto)
        {
            var servicio = new Servicio
            {
                Name = servicioDto.Name,
                Description = servicioDto.Description,
            };

            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServicios), new { id = servicio.IdServicios}, servicio);
        }
    }
}
