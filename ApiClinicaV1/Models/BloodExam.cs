using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaV1.Models
{
    public class BloodExam
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }

        public int Glucose { get; set; }
        public int RedCells { get; set; }
        public int Hemoglobin { get; set; }
        public int WhiteCells { get; set; }
        public int Platelets { get; set; }
        public int BloodPressure { get; set; }
        public int Cholesterol { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsGood { get; set; }

        // Relaciones
        public User User { get; set; }
    }
}
