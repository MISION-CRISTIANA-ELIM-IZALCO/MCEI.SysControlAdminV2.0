using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using static MCEI.SysControlAdmin.Core.Utils.UtilsRegion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuraci�n de autenticaci�n con cookies basada en roles
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(static options =>
    {
        options.LoginPath = new PathString("/User/Login");
        options.AccessDeniedPath = new PathString("/Home/Index");
        options.SlidingExpiration = true;

        options.Events = new CookieAuthenticationEvents
        {
            OnSigningIn = async context =>
            {
                var principal = context.Principal;
                if (principal != null)
                {
                    var identity = (ClaimsIdentity)principal.Identity!;
                    if (identity != null)
                    {
                        var roleClaim = identity.FindFirst(ClaimTypes.Role)?.Value;

                        // Definir tiempos de sesi�n seg�n el rol
                        TimeSpan sessionDuration = roleClaim switch
                        {
                            "Desarrollador" => TimeSpan.FromHours(5),
                            "Administrador" => TimeSpan.FromHours(5),
                            "Digitador" => TimeSpan.FromHours(3),
                            _ => TimeSpan.FromMinutes(30),            // Otros roles tienen 30 minutos
                        };

                        // Usar la zona horaria personalizada definida en UtilsRegion
                        var fechaZona = DateTime.Now.GetFechaZonaHoraria();

                        // Establecer la expiraci�n en UTC a partir de esa fecha
                        context.Properties.ExpiresUtc = fechaZona.Add(sessionDuration).ToUniversalTime();
                        context.Properties.IsPersistent = true; // Para asegurar que la cookie persista el tiempo asignado
                    }
                }
                await Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Primero autenticaci�n
app.UseAuthorization();  // Luego autorizaci�n

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configuraci�n de Rotativa (Reportes HTML a PDF)
IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../wwwroot/lib/Rotativa");

app.Run();
