using ApiClinicaV1.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
namespace ApiClinicaV1.Models.config
{


    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentMedicine> AppointmentMedicines { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<MedicalInformation> MedicalInformations { get; set; }
        public DbSet<BloodExam> BloodExams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Service)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.Servicio);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.IdUser);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.IdDoctor);

            modelBuilder.Entity<AppointmentMedicine>()
                .HasOne(am => am.Appointment)
                .WithMany(a => a.AppointmentMedicines)
                .HasForeignKey(am => am.AppointmentId);

            modelBuilder.Entity<AppointmentMedicine>()
                .HasOne(am => am.Medicines)
                .WithMany()
                .HasForeignKey(am => am.MedicinesId);

            modelBuilder.Entity<BloodExam>()
                .HasOne(b => b.User)
                .WithMany(u => u.BloodExams)
                .HasForeignKey(b => b.IdUser);

            modelBuilder.Entity<MedicalInformation>()
                .HasOne(m => m.User)
                .WithOne(u => u.MedicalInfo)
                .HasForeignKey<MedicalInformation>(m => m.IdUser);

            modelBuilder.Entity<Donation>()
                .HasOne(d => d.User)
                .WithMany(u => u.Donations)
                .HasForeignKey(d => d.UserId);
        }
    }
}
