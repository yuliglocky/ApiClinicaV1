namespace ApiClinicaV1.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }  // Nuevo campo para el nombre del usuario
        public int DoctorId { get; set; }
        public string DoctorServiceName { get; set; }  // Nuevo campo para el nombre del servicio del doctor
        public string Reason { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }
    }

}