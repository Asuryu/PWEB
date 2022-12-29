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


        public async Task<IActionResult> MyReservations()
        {
            ViewData["Title"] = "Lista de Reservas";
            
            return View(await _context.Reservas.ToListAsync()); //TODO: mostrar só as do utilizador loggado
        }

        public async Task<IActionResult> Index(bool? confirmadas)
        {
            ViewData["Title"] = "Lista de Reservas";
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            ViewData["ListaDeVeiculos"] = new SelectList(_context.Veiculos.ToList(), "Id", "Marca");
            ViewData["ListaDeClientes"] = new SelectList(_context.Clientes.ToList(), "Id", "PrimeiroNome");

            if (confirmadas != null)
            {
                return View(await _context.Reservas.Where(c => c.Confirmada == confirmadas)
                            .ToListAsync());
            }
            else
            {
                return View(await _context.Reservas.ToListAsync());
            }

            return View(await _context.Reservas.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(int CategoriaId, int VeiculoId, int ClienteId)
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            ViewData["ListaDeVeiculos"] = new SelectList(_context.Veiculos.ToList(), "Id", "Marca");
            ViewData["ListaDeClientes"] = new SelectList(_context.Clientes.ToList(), "Id", "PrimeiroNome");

            var resultado = from c in _context.Reservas
                            where c.Veiculo.CategoriaId == CategoriaId && c.VeiculoId == VeiculoId && c.ClienteId == ClienteId
                            select c;
            return View(resultado);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empresas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Empresas'  is null.");
            }
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
            {
                if(empresa.Veiculos.Count > 0)
                {
                    return Problem("Não é possível eliminar a empresa pois esta contém veículos.");
                }
                _context.Empresas.Remove(empresa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}
