using Healtive.Application.Interfaces;
using Healtive.Infrastructure.Configuration;
using Healtive.Infrastructure.Data;
using Healtive.Infrastructure.Repositories.Auth;
using Healtive.Infrastructure.Repositories.Branches;
using Healtive.Infrastructure.Repositories.Dashboard;
using Healtive.Infrastructure.Repositories.Departments;
using Healtive.Infrastructure.Repositories.Doctors;
using Healtive.Infrastructure.Repositories.DoctorSpecializations;
using Healtive.Infrastructure.Repositories.Hospitals;
using Healtive.Infrastructure.Repositories.HospitalSubscriptions;
using Healtive.Infrastructure.Repositories.Patients;
using Healtive.Infrastructure.Repositories.Roles;
using Healtive.Infrastructure.Repositories.Staff;
using Healtive.Infrastructure.Repositories.SubscriptionPlans;
using Healtive.Infrastructure.Repositories.Appointments;
using Healtive.Infrastructure.Seed;
using Healtive.Infrastructure.Services.Appointments;
using Healtive.Infrastructure.Services.Auth;
using Healtive.Infrastructure.Services.Branches;
using Healtive.Infrastructure.Services.Dashboard;
using Healtive.Infrastructure.Services.Departments;
using Healtive.Infrastructure.Services.Doctors;
using Healtive.Infrastructure.Services.DoctorSpecializations;
using Healtive.Infrastructure.Services.Hospitals;
using Healtive.Infrastructure.Services.HospitalSubscriptions;
using Healtive.Infrastructure.Services.Patients;
using Healtive.Infrastructure.Services.Roles;
using Healtive.Infrastructure.Services.Staff;
using Healtive.Infrastructure.Services.SubscriptionPlans;
using Healtive.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Dapper;
using Healtive.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("HealtiveFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthorization();
// Add services to the container.

builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IHospitalRepository, HospitalRepository>();
builder.Services.AddScoped<IHospitalService, HospitalService>();
builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
builder.Services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
builder.Services.AddScoped<IHospitalSubscriptionRepository, HospitalSubscriptionRepository>();
builder.Services.AddScoped<IHospitalSubscriptionService, HospitalSubscriptionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IDoctorSpecializationRepository, DoctorSpecializationRepository>();
builder.Services.AddScoped<IDoctorSpecializationService, DoctorSpecializationService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDoctorDepartmentRepository,  DoctorDepartmentRepository>();
builder.Services.AddScoped<IDoctorDepartmentService, DoctorDepartmentService>();
builder.Services.AddScoped<IDoctorSpecializationMappingRepository,DoctorSpecializationMappingRepository>();
builder.Services.AddScoped<IDoctorSpecializationMappingService, DoctorSpecializationMappingService>();
builder.Services.AddScoped<IDoctorAvailabilityRepository, DoctorAvailabilityRepository>();
builder.Services.AddScoped<IDoctorAvailabilityService, DoctorAvailabilityService>();
builder.Services.AddScoped<IDoctorLeaveRepository, DoctorLeaveRepository>();
builder.Services.AddScoped<IDoctorLeaveService, DoctorLeaveService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorDashboardService, DoctorDashboardService>();
builder.Services.AddScoped<IDoctorDashboardRepository, DoctorDashboardRepository>();
builder.Services.AddScoped<IDoctorPatientRepository, DoctorPatientRepository>();
builder.Services.AddScoped<IDoctorPatientService, DoctorPatientService>();
builder.Services.AddScoped<IDoctorPatientMedicalHistoryRepository, DoctorPatientMedicalHistoryRepository>();
builder.Services.AddScoped<IDoctorPatientMedicalHistoryService, DoctorPatientMedicalHistoryService>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider
        .GetRequiredService<IDatabaseSeeder>();

    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("HealtiveFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.MapControllers();

app.Run();
