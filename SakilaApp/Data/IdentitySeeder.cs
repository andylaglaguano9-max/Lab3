using Microsoft.AspNetCore.Identity;

namespace SakilaApp.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // --- Crear los 4 roles requeridos ---
        string[] roles = { "Administrador", "Supervisor", "Operador", "Consulta" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // --- Datos de los usuarios de prueba ---
        var usuarios = new[]
        {
            new { Email = "admin@espe.edu.ec",      Password = "Admin123*",      Rol = "Administrador" },
            new { Email = "supervisor@espe.edu.ec",  Password = "Super123*",      Rol = "Supervisor"     },
            new { Email = "operador@espe.edu.ec",    Password = "Oper123*",       Rol = "Operador"       },
            new { Email = "consulta@espe.edu.ec",    Password = "Consulta123*",   Rol = "Consulta"       },
        };

        foreach (var datos in usuarios)
        {
            var usuario = await userManager.FindByEmailAsync(datos.Email);

            if (usuario == null)
            {
                // Usuario no existe: crearlo
                usuario = new IdentityUser
                {
                    UserName = datos.Email,
                    Email = datos.Email,
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(usuario, datos.Password);
                if (!resultado.Succeeded)
                {
                    foreach (var error in resultado.Errors)
                    {
                        Console.WriteLine($"[Seeder] Error creando {datos.Email}: {error.Description}");
                    }
                    continue;
                }
            }
            else
            {
                // Usuario ya existe: forzar reset de contraseña para asegurar que sea la correcta
                var token = await userManager.GeneratePasswordResetTokenAsync(usuario);
                var resetResult = await userManager.ResetPasswordAsync(usuario, token, datos.Password);
                if (!resetResult.Succeeded)
                {
                    foreach (var error in resetResult.Errors)
                    {
                        Console.WriteLine($"[Seeder] Error reseteando contraseña de {datos.Email}: {error.Description}");
                    }
                }

                // Asegurar que el email esté confirmado
                if (!usuario.EmailConfirmed)
                {
                    usuario.EmailConfirmed = true;
                    await userManager.UpdateAsync(usuario);
                }
            }

            // Asignar rol si el usuario aún no lo tiene
            if (!await userManager.IsInRoleAsync(usuario, datos.Rol))
            {
                await userManager.AddToRoleAsync(usuario, datos.Rol);
            }
        }

        Console.WriteLine("[Seeder] Roles y usuarios verificados correctamente.");
    }
}
