using Healtive.Application.Interfaces;
using Healtive.Infrastructure.Configuration;
using Healtive.Infrastructure.Data;
using Healtive.Infrastructure.Repositories.Auth;
using Healtive.Infrastructure.Repositories.Branches;
using Healtive.Infrastructure.Repositories.Dashboard;
using Healtive.Infrastructure.Repositories.Departments;
using Healtive.Infrastructure.Repositories.Hospitals;
using Healtive.Infrastructure.Repositories.HospitalSubscriptions;
using Healtive.Infrastructure.Repositories.SubscriptionPlans;
using Healtive.Infrastructure.Seed;
using Healtive.Infrastructure.Services.Auth;
using Healtive.Infrastructure.Services.Branches;
using Healtive.Infrastructure.Services.Dashboard;
using Healtive.Infrastructure.Services.Departments;
using Healtive.Infrastructure.Services.Hospitals;
using Healtive.Infrastructure.Services.HospitalSubscriptions;
using Healtive.Infrastructure.Services.SubscriptionPlans;
using Healtive.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

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
