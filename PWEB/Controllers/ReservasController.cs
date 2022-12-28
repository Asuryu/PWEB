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
    public class ReservasController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(bool? confirmadas)
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            ViewData["ListaDeVeiculos"] = new SelectList(_context.Veiculos.ToList(), "Id", "Marca");
            ViewData["ListaDeClientes"] = new SelectList(_context.Clientes.ToList(), "Id", "Id");

            if (confirmadas != null)
            {
                return View(await _context.Reservas.Where(c => c.Confirmada == confirmadas)
                            .ToListAsync());
            }
            else
            {
                return View(await _context.Reservas.ToListAsync());
            }
        }


        [HttpPost]
        public async Task<IActionResult> Index(int CategoriaId, int VeiculoId, int ClienteId)
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            ViewData["ListaDeVeiculos"] = new SelectList(_context.Veiculos.ToList(), "Id", "Marca");
            ViewData["ListaDeClientes"] = new SelectList(_context.Clientes.ToList(), "Id", "Id");

            // TODO:
            var resultado = from c in _context.Reservas
                            
                            select c;
            return View(resultado);
        }

        public async Task<IActionResult> Confirm(int? reservaId)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva != null)
            {
                bool isConfirmed = reserva.Confirmada;
                if (isConfirmed == false)
                {
                    reserva.Confirmada = true;
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Deny(int? reservaId)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva != null)
            {
                bool isConfirmed = reserva.Confirmada;
                if (isConfirmed == true)
                {
                    reserva.Confirmada = false;
                    _context.Reservas.Remove(reserva);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

    }

}

