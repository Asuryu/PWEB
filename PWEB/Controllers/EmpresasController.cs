using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWEB.Data;
using PWEB.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PWEB.Controllers
{
    public class EmpresasController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmpresasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(bool? disponivel)
        {
            ViewData["Title"] = "Lista de Empresas";
            return View(await _context.Empresas.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Avaliacao,SubscricaoAtiva")] Empresa empresa)
        {

            //ModelState.Remove(nameof(empresa.Gestores));
            //ModelState.Remove(nameof(empresa.Funcionarios));
            //ModelState.Remove(nameof(empresa.Veiculos));

            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();

                var defaultUser = new ApplicationUser
                {
                    UserName = "gestor@" + empresa.Nome + ".com",
                    Email = "gestor@" + empresa.Nome + ".com",
                    PrimeiroNome = "Gestor",
                    UltimoNome = empresa.Nome,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultUser, "ISEC_PWeb_2022!");
                    await _userManager.AddToRoleAsync(defaultUser, "Gestor");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

    }
}

