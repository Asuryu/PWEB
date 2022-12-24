using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
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

            ModelState.Remove(nameof(empresa.Gestores));
            ModelState.Remove(nameof(empresa.Funcionarios));
            ModelState.Remove(nameof(empresa.Veiculos));

            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

    }
}

