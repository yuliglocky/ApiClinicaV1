namespace ApiClinicaV1.Dtos
{
    public class BloodExamDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Glucose { get; set; }
        public int RedCells { get; set; }
        public int Hemoglobin { get; set; }
        public int WhiteCells { get; set; }
        public int Platelets { get; set; }
        public int BloodPressure { get; set; }
        public int Cholesterol { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsGood { get; set; }
        public string UserName { get; set; } 
    }
}
