using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaV1.Models
{
    public class MedicalInformation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }

        public decimal Weight { get; set; }
        public int Height { get; set; }
        public int Temperature { get; set; }
        public int BloodPressure { get; set; }
        public int HeartRate { get; set; }

        // Relaciones
        public User User { get; set; }
    }
}
