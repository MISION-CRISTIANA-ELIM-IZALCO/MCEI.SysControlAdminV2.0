using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuración de autenticación con cookies basada en roles
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
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

                        // Definir tiempos de sesión según el rol
                        TimeSpan sessionDuration = roleClaim switch
                        {
                            "Desarrollador" => TimeSpan.FromHours(5),
                            "Administrador" => TimeSpan.FromHours(5),
                            "Digitador" => TimeSpan.FromHours(3),
                            _ => TimeSpan.FromMinutes(30),            // Otros roles tienen 30 minutos
                        };

                        context.Properties.ExpiresUtc = DateTime.UtcNow.Add(sessionDuration);
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Importante: Primero autenticación
app.UseAuthorization();  // Luego autorización

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
