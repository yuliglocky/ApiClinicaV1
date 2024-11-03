using ApiClinicaV1.Models;
using ApiClinicaV1.Models.config;

using ApiClinicaV1.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra servicios personalizados
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<IDoctorService, DoctorServices>();
builder.Services.AddScoped<IAppointmentService, AppointmentServices>();
builder.Services.AddScoped<IBloodExamService, BloodExamServices>();
builder.Services.AddScoped<AppointmentMedicineService>();
builder.Services.AddScoped<DonationServices>();
builder.Services.AddScoped<MedicalInformacionServices>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
