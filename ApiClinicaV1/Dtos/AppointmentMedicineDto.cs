namespace ApiClinicaV1.Dtos
{
    public class AppointmentMedicineDto
    {
        public int Id { get; set; }

        public int UserId{ get; set; }
        public int AppointmentId { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}
