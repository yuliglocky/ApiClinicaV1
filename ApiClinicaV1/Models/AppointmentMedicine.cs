using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiClinicaV1.Models
{
    public class AppointmentMedicine
    {
        [Key]
        public int IdAppointmentxMedicines { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        [ForeignKey("Medicines")]
        public int MedicinesId { get; set; }

        public int Quantity { get; set; }

        // Relaciones
        public Appointment Appointment { get; set; }
        public Medicine Medicines { get; set; }
    }
}
