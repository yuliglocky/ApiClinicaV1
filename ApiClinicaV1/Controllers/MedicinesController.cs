using ApiClinicaV1.Dtos;
using ApiClinicaV1.Models;
using ApiClinicaV1.Models.config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase 
    {
        private readonly AppDbContext _context;

        public MedicinesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/medicines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineDto>>> GetMedicines()
        {
            var medicines = await _context.Medicines.ToListAsync();
            return Ok(medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Quantity = m.Quantity
            }));
        }

        // POST: api/medicines
        [HttpPost]
        public async Task<ActionResult<MedicineDto>> PostMedicine([FromBody] MedicineDto medicineDto)
        {
            if (medicineDto == null)
            {
                return BadRequest("MedicineDto is null");
            }

            var medicine = new Medicine
            {
                Id = medicineDto.Id,
                Name = medicineDto.Name,
                Quantity = medicineDto.Quantity
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            medicineDto.Id = medicine.Id; // Set the ID after saving to the database

            return CreatedAtAction(nameof(GetMedicines), new { id = medicineDto.Id }, medicineDto);
        }
    }
}
      
 