using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PWEB.Data;
using PWEB.Models;
using PWEB_AulasP_2223.Models;
using System;
using System.Numerics;

namespace PWEB_AulasP_2223.Data
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
        public static async Task CriaDadosIniciais(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            //Adicionar default Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Cliente.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Funcionario.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Gestor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrador.ToString()));

            //Adicionar Default User - Admin
            var defaultUser = new ApplicationUser
            {
                UserName = "admin@localhost.com",
                Email = "admin@localhost.com",
                PrimeiroNome = "Administrador",
                UltimoNome = "Local",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {

                /**var admin = new Administrador
                {
                    ApplicationUser = defaultUser
                };**/
                //context.Add(admin);
                await context.SaveChangesAsync();
                await userManager.CreateAsync(defaultUser, "Is3C..00");
                await userManager.AddToRoleAsync(defaultUser, Roles.Administrador.ToString());
            }
        }
    }
}