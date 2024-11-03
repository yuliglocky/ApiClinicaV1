namespace ApiClinicaV1.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dni { get; set; }
        public string Cellphone { get; set; }
        public int Gender { get; set; }
        public int Blood { get; set; }
        public string Email { get; set; }
        public string Allergies { get; set; }

        public string Password  { get; set; }
        public int Role { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public bool IsDonor { get; set; }
    }
}
