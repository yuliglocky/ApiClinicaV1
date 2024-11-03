using System.ComponentModel.DataAnnotations;

namespace ApiClinicaV1.Models
{
    public class Servicio
    {
        [Key]
        public int IdServicios { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Relaciones
        public ICollection<Doctor> Doctors { get; set; }
    }
}
