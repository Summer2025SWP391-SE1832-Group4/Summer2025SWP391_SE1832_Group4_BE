using System.Reflection;
using System.Text;
using AutoMapper;
using CinemaBooking.API.Middlewares;
using CinemaBooking.API.Middlewares.LoggingMiddleware;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Settings;
using HIVTreatmentSystem.Application.Services;
using HIVTreatmentSystem.Application.Services.Account;
using HIVTreatmentSystem.Application.Services.Auth;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using HIVTreatmentSystem.Infrastructure.Repositories;
using HIVTreatmentSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseWrapperAttribute>();
});
builder.Services.AddEndpointsApiExplorer();

// Configure JWT Authentication
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<JwtService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings?.SecretKey ?? "68ff49a9-2d29-45e8-860b-753bbd99aea0");

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings?.Issuer ?? "HIVTreatmentSystem.API",
            ValidAudience = jwtSettings?.Audience ?? "HIVTreatmentSystemClient",
            ClockSkew = TimeSpan.Zero,
        };
    });

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DoctorOnly", policy => policy.RequireRole("Doctor"));
    options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Email service
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// Swagger config
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "HIV Treatment System API",
            Version = "v1",
            Description =
                "API for managing HIV treatment records, patients, doctors, and appointments",
            Contact = new OpenApiContact
            {
                Name = "SWP391 Group 4",
                Email = "group4@example.com",
                Url = new Uri("https://github.com/Summer2025SWP391-SE1832-Group4"),
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0 License",
                Url = new Uri(
                    "https://github.com/Summer2025SWP391-SE1832-Group4/Summer2025SWP391_SE1832_Group4_BE/blob/main/LICENSE"
                ),
            },
        }
    );

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

//CORS: Chỉ cho phép frontend từ localhost:5173
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // ➜ Cần nếu FE gửi cookie hoặc Authorization
        }
    );
});

// DbContext & repositories
builder.Services.AddDbContext<HIVDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<ITreatmentRepository, TreatmentRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IExperienceWorkingRepository, ExperienceWorkingRepository>();
builder.Services.AddScoped<IExperienceWorkingService, ExperienceWorkingService>();
builder.Services.AddScoped<
    HIVTreatmentSystem.Application.Interfaces.IPasswordHasher,
    HIVTreatmentSystem.Application.Services.Auth.PasswordHasher
>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<
    HIVTreatmentSystem.Application.Interfaces.IPasswordHasher,
    HIVTreatmentSystem.Application.Services.Auth.PasswordHasher
>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<HIVTreatmentSystem.Domain.Interfaces.IDoctorScheduleRepository, HIVTreatmentSystem.Infrastructure.Repositories.DoctorScheduleRepository>();
builder.Services.AddScoped<HIVTreatmentSystem.Application.Interfaces.IDoctorScheduleService, HIVTreatmentSystem.Application.Services.DoctorScheduleService>();
builder.Services.AddScoped<HIVTreatmentSystem.Domain.Interfaces.ISystemAuditLogRepository, HIVTreatmentSystem.Infrastructure.Repositories.SystemAuditLogRepository>();
builder.Services.AddScoped<HIVTreatmentSystem.Application.Interfaces.ISystemAuditLogService, HIVTreatmentSystem.Application.Services.SystemAuditLogService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Luôn bật Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HIV Treatment System API v1");
    c.RoutePrefix = "swagger";
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    c.EnableFilter();
    c.EnableDeepLinking();
    c.DisplayOperationId();
    c.DisplayRequestDuration();
    c.DefaultModelsExpandDepth(-1);
});

//Sử dụng chính sách CORS mới
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseCustomExceptionHandler();
app.UseCustomLogging();
app.UseAuthentication();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
