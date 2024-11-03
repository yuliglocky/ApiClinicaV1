using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaV1.Models
{
    public class Doctor
    {
        [Key]
        public int IdDoctor { get; set; }

        [ForeignKey("Service")]
        public int Servicio { get; set; }

        // Relaciones
        public Servicio Service { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}