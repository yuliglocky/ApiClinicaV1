using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiClinicaV1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dni { get; set; }
        public string Cellphone { get; set; }
        public int Gender { get; set; }
        public int Blood { get; set; }
        public string Email { get; set; }
        public string Allergies { get; set; }
        public int Role { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public bool IsDonor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relaciones
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Donation> Donations { get; set; }


        [JsonIgnore] // Evita el ciclo al no serializar esta propiedad
        public ICollection<BloodExam> BloodExams { get; set; }
        public MedicalInformation MedicalInfo { get; set; }
    }
}
