using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shorten.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services.Implementations;
using Settings;
using Shorten.Areas.Domain;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("ShortenIdentityDbContextConnection") 
                      ?? throw new InvalidOperationException("Connection string 'ShortenIdentityDbContextConnection' not found.");

// Configuración de los contextos de base de datos
builder.Services.AddDbContext<ShortenIdentityDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ShortenIdentityDbContext>();

builder.Services.AddDbContext<ShortenContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

// Configuración del servicio de correo electrónico
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<Services.Interfaces.IEmailSender, EmailSender>();

// Leer la contraseña SMTP desde la variable de entorno
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
emailSettings.Password = Environment.GetEnvironmentVariable("SMTP_TOKEN");

// Configuración de otros servicios
builder.Services.AddQuickGridEntityFrameworkAdapter();

var app = builder.Build();

// Configuración de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();