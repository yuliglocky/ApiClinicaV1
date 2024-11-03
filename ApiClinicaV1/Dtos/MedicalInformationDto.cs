namespace ApiClinicaV1.Dtos
{
    public class MedicalInformationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Weight { get; set; }
        public int Height { get; set; }
        public int Temperature { get; set; }
        public int BloodPressure { get; set; }
        public int HeartRate { get; set; }
    }
}
