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
using PWEB_AulasP_2223.Data;
using PWEB_AulasP_2223.Models;
using PWEB_AulasP_2223.ViewModels;

namespace PWEB_AulasP_2223.Controllers
{
    public class FuncionariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FuncionariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            //var current_user = await _userManager.GetUserAsync(HttpContext.User);
            //var users = await _userManager.Users.ToListAsync();
            var users = await _context.Gestores.ToListAsync();
            List<GestoresViewModel> listaUtilizadores = new List<GestoresViewModel>();
            foreach (var user in users)
            {
                Console.WriteLine(user.ApplicationUser.PrimeiroNome);
                GestoresViewModel userRolesViewModel = new GestoresViewModel();
                userRolesViewModel.PrimeiroNome = "asdsadas";
                userRolesViewModel.UltimoNome = "asdsadas";
                userRolesViewModel.Email = "asdsadas";
                userRolesViewModel.UserId = "asdsadas";
                userRolesViewModel.NomeEmpresa = "asdsadas";
                listaUtilizadores.Add(userRolesViewModel);
            }
            return View(listaUtilizadores);
        }

        // GET: Categorias/Create
        public IActionResult CreateFuncionario()
        {
            return View();
        }
        // GET: Categorias/Create
        public IActionResult CreateGestor()
        {
            return View();
        }

    }
}
