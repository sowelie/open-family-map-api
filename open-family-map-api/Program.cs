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
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<LocationDetailRepository>();
builder.Services.AddSingleton<InitializationService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
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

builder.Services.AddTransient(provider =>
{
    //resolve another classes from DI
    var config = provider.GetService<IConfiguration>();

    //pass any parameters
    return new OpenFamilyMapContext(config!);
});

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

// run database migrations at startup
var db = app.Services.GetRequiredService<OpenFamilyMapContext>();
await db.Database.MigrateAsync();

// trigger the initialization service
var init = app.Services.GetRequiredService<InitializationService>();
await init.Initialize();

app.Run();