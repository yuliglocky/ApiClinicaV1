using System.ComponentModel.DataAnnotations;

namespace ApiClinicaV1.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }
}
