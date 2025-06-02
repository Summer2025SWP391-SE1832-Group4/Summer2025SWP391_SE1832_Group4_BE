using System.Reflection;
using System.Text;
using AutoMapper;
using HIVTreatmentSystem.API.Mappers;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Settings;
using HIVTreatmentSystem.Application.Services.Account;
using HIVTreatmentSystem.Application.Services.Auth;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using HIVTreatmentSystem.Infrastructure.Repositories;
using HIVTreatmentSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Filters;
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

// Add JWT Authentication
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

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DoctorOnly", policy => policy.RequireRole("Doctor"));
    options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Configure Email Service
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// Configure Swagger
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
                Name = "Use under Academic License",
                Url = new Uri("https://example.com/license"),
            },
        }
    );

    // Enable XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Add JWT Authentication support in Swagger UI
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

// Add CORS with policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:3000", // React client
                    "http://localhost:4200" // Angular client
                // Production client
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
    );

    // For development environment - allow any origin
    options.AddPolicy(
        "DevCorsPolicy",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

// Add DbContext
builder.Services.AddDbContext<HIVDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register application services
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<ITreatmentRepository, TreatmentRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AccountMapper));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HIV Treatment System API v1");
        c.RoutePrefix = "swagger"; // Set Swagger UI at the app's root
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.EnableFilter();
        c.EnableDeepLinking();
        c.DisplayOperationId(); // Display operation IDs in Swagger UI
        c.DisplayRequestDuration(); // Show request duration in Swagger UI
        c.DefaultModelsExpandDepth(-1); // Hide the models by default
    });
    // Use the development CORS policy in development environment
    app.UseCors("DevCorsPolicy");
}
else
{
    // Use the production CORS policy in production environment
    app.UseCors("CorsPolicy");
    app.UseHsts();

    // Enable Swagger in production but with basic auth
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "api-docs/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/api-docs/v1/swagger.json", "HIV Treatment System API v1");
        c.RoutePrefix = "api-docs";
        c.EnableFilter();
        c.DefaultModelsExpandDepth(-1);
        c.EnableDeepLinking();
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

// Add authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
