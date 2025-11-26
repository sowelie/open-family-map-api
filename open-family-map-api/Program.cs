using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenFamilyMapAPI.Core.Data;
using OpenFamilyMapAPI.Repositories;
using OpenFamilyMapAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "OPENFAMILYMAP_");

// Add services to the container.
builder.Services.AddDbContext<OpenFamilyMapContext>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<LocationDetailRepository>();
builder.Services.AddTransient<InitializationService>();
builder.Services.AddTransient<IJWTService, JWTService>();
builder.Services.AddHostedService<InitializationService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        if (!builder.Environment.IsDevelopment())
        {
            options.RequireHttpsMetadata = true;
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"] ?? "https://localhost:7089",
            ValidAudience = builder.Configuration["JWT:Audience"] ?? "https://localhost:7089",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers()
    .RequireAuthorization();

app.Run();