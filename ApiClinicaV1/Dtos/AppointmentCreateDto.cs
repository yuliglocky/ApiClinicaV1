namespace ApiClinicaV1.Dtos
{
    public class AppointmentCreateDto
    {
        public int UserId { get; set; } // ID del usuario que solicita la cita
        public int DoctorId { get; set; } // ID del doctor con el que se agenda la cita
        public string Reason { get; set; } // Motivo de la cita
        public DateTime AppointmentDate { get; set; } // Fecha y hora de la cita
        public string Notes { get; set; }
    }
}
