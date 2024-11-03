using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaV1.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }

        [ForeignKey("Doctor")]
        public int IdDoctor { get; set; }

        public string Reason { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsValid { get; set; }

        // Relaciones
        public User User { get; set; }
        public Doctor Doctor { get; set; }
        public ICollection<AppointmentMedicine> AppointmentMedicines { get; set; }
    }
}
