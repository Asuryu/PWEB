using System;
using Microsoft.AspNetCore.Identity;
using PWEB.Models;

namespace PWEB.Data
{
    public enum Roles
    {
        Cliente,
        Funcionario,
        Gestor,
        Administrador
    }

    public static class Inicializacao
    {
        public static async Task CriaDadosIniciais(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Adicionar default Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Cliente.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Funcionario.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Gestor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrador.ToString()));

            //Adicionar Default User - Admin
            var defaultUser = new ApplicationUser
            {
                UserName = "admin@isec.pt",
                Email = "admin@isec.pt",
                PrimeiroNome = "Administrador",
                UltimoNome = "Local",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            await userManager.CreateAsync(defaultUser, "PWEB_ADMIN_2022");
            await userManager.AddToRoleAsync(defaultUser, Roles.Administrador.ToString());
        }
    }
}

