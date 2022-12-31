using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using PWEB.ViewModels;
using PWEB_AulasP_2223.Data;
using PWEB_AulasP_2223.Models;
using PWEB_AulasP_2223.ViewModels;

namespace PWEB_AulasP_2223.Controllers
{
    [Authorize]
    [Authorize(Roles = "Gestor")]
    public class GestoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GestoresController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            var current_user = await _userManager.GetUserAsync(HttpContext.User);
            //var users = await _userManager.Users.ToListAsync();
            var users = await _context.Gestores.ToListAsync();
            List<GestoresViewModel> listaUtilizadores = new List<GestoresViewModel>();
            foreach (var user in users)
            {
                Empresa Empresa = await _context.Empresas.FindAsync(user.EmpresaId);
                if (Empresa == null || user.EmpresaId != current_user.EmpresaId) continue;
                Console.WriteLine(user.EmpresaId);
                Console.WriteLine(user.Empresa.Nome);
                GestoresViewModel userRolesViewModel = new GestoresViewModel();
                userRolesViewModel.PrimeiroNome = user.PrimeiroNome;
                userRolesViewModel.UltimoNome = user.UltimoNome;
                userRolesViewModel.Email = user.Email;
                userRolesViewModel.UserId = user.Id;
                userRolesViewModel.NomeEmpresa = Empresa.Nome;
                userRolesViewModel.LockedAccount = await _userManager.IsLockedOutAsync(user);
                userRolesViewModel.CanDelete = current_user.Id != user.Id;
                ViewBag.NomeEmpresa = Empresa.Nome;
                listaUtilizadores.Add(userRolesViewModel);
            }
            return View(listaUtilizadores);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            List<string> CargosPossiveis = new List<string>();
            CargosPossiveis.Add("Gestor");
            CargosPossiveis.Add("Funcionario");
            ViewData["CargosPossiveis"] = new SelectList(CargosPossiveis.ToList());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("PrimeiroNome,UltimoNome,DataNascimento,CargoNaEmpresa")]NovoMembroViewModel novoMembro)
        {

            Console.WriteLine(novoMembro.PrimeiroNome);
            Console.WriteLine(novoMembro.UltimoNome);
            Console.WriteLine(novoMembro.DataNascimento);
            Console.WriteLine(novoMembro.CargoNaEmpresa);

            if (ModelState.IsValid)
            {

                var current_user = await _userManager.GetUserAsync(HttpContext.User);
                var empresa = await _context.Empresas.FindAsync(current_user.EmpresaId);

                var email = novoMembro.PrimeiroNome.ToLower() + "_" + novoMembro.UltimoNome.ToLower() + "@" + empresa.Nome.ToLower() + ".com";
                var defaultUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    PrimeiroNome = novoMembro.PrimeiroNome,
                    UltimoNome = novoMembro.UltimoNome,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    EmpresaId = current_user.EmpresaId,
                    Empresa = empresa,
                    CargoNaEmpresa = novoMembro.CargoNaEmpresa
                };
                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultUser, "ISEC_PWeb_2022!");
                    await _userManager.AddToRoleAsync(defaultUser, novoMembro.CargoNaEmpresa);
                    empresa.GestoresFuncionarios.Add(defaultUser);
                    if (novoMembro.CargoNaEmpresa == "Gestor")
                    {
                        _context.Gestores.Add(defaultUser);
                    } else if(novoMembro.CargoNaEmpresa == "Funcionario")
                    {
                        _context.Funcionarios.Add(defaultUser);
                    }
                    //await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(novoMembro);
        }

        public async Task<IActionResult> Enable(string? userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                bool isLockedOut = await _userManager.IsLockedOutAsync(user);
                Console.WriteLine(isLockedOut);
                if (isLockedOut)
                {
                    await _userManager.SetLockoutEnabledAsync(user, false);
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.MinValue);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Disable(string? userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                bool isLockedOut = await _userManager.IsLockedOutAsync(user);
                Console.WriteLine(isLockedOut);
                if (!isLockedOut)
                {
                    await _userManager.SetLockoutEnabledAsync(user, true);
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);
                }
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(string? userId)
        {
            
            // get user
            var user = await _userManager.FindByIdAsync(userId);

            var entregas = await _context.Entregas.Where(e => e.FuncionarioId == userId).ToListAsync();
            var recolhas = await _context.Recolhas.Where(r => r.FuncionarioId == userId).ToListAsync();
            
            if (entregas.Count == 0 && recolhas.Count == 0)
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction("Index");
            }

            return Problem("O utilizador não pode ser eliminado porque tem entregas ou recolhas associadas.");

        }

    }

}
