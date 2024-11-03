namespace ApiClinicaV1.Dtos
{
    public class AttendedAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string AppointmentDate { get; set; } // O DateTime, según tus necesidades
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
    }
}
