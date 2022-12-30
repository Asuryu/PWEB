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
using PWEB.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PWEB.Controllers
{
    [Authorize]
    [Authorize(Roles = "Cliente,Funcionario,Gestor")]
    public class ReservasController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> MyReservations()
        {
            ViewData["Title"] = "Lista de Reservas";
            
            return View(await _context.Reservas.ToListAsync()); //TODO: mostrar só as do utilizador loggado
        }

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Index(bool? confirmadas)
        {
            ViewData["Title"] = "Lista de Reservas";
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            ViewData["ListaDeVeiculos"] = new SelectList(_context.Veiculos.ToList(), "Id", "Marca");
            ViewData["ListaDeClientes"] = new SelectList(_context.Clientes.ToList(), "Id", "PrimeiroNome");

            var current_user = await _userManager.GetUserAsync(HttpContext.User);
            Empresa Empresa = await _context.Empresas.FindAsync(current_user.EmpresaId);
            ViewBag.NomeEmpresa = Empresa.Nome;

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

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
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

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
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

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
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

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
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

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
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

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Deliver(int? reservaId)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva != null)
            {
                ViewBag.Veiculo = reserva.Veiculo;
                ViewBag.ReservaId = reserva.Id;
                return View();
            }
            return Problem("Não foi possível encontrar esse veículo.");
        }

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
        [HttpPost]
        public async Task<IActionResult> Deliver(int? reservaId, [Bind("NrKmsVeiculo, Danos, Observacoes")] EntregaViewModel entrega)
        {
            if (ModelState.IsValid)
            {
                var reserva = await _context.Reservas.FindAsync(reservaId);
                 
                EntregaVeiculo novaEntrega = new EntregaVeiculo
                {
                    NrKmsVeiculos = entrega.NrKmsVeiculo,
                    Danos = entrega.Danos,
                    Observacoes = entrega.Observacoes,
                    Funcionario = await _userManager.GetUserAsync(User),
                    Reserva = reserva
                };

                // add new entrega to database
                _context.Entregas.Add(novaEntrega);
                await _context.SaveChangesAsync();

                reserva.EntregaVeiculo = novaEntrega;
                reserva.EntregaVeiculoId = novaEntrega.Id;


                _context.Reservas.Update(reserva);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");

            }
            return View(entrega);
        }

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Receive(int? reservaId)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva != null)
            {
                ViewBag.Veiculo = reserva.Veiculo;
                ViewBag.ReservaId = reserva.Id;
                return View();
            }
            return Problem("Não foi possível encontrar esse veículo.");
        }

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
        [HttpPost]
        public async Task<IActionResult> Receive(int? reservaId, [Bind("NrKmsVeiculo, Danos, Observacoes")] RecolhaViewModel recolha, List<IFormFile> files)
        {

            if (ModelState.IsValid)
            {
                var reserva = await _context.Reservas.FindAsync(reservaId);

                RecolhaVeiculo novaRecolha = new RecolhaVeiculo
                {
                    NrKmsVeiculos = recolha.NrKmsVeiculo,
                    Danos = recolha.Danos,
                    Observacoes = recolha.Observacoes,
                    Funcionario = await _userManager.GetUserAsync(User),
                    Reserva = reserva
                };

                _context.Recolhas.Add(novaRecolha);
                await _context.SaveChangesAsync();

                reserva.RecolhaVeiculo = novaRecolha;
                reserva.RecolhaVeiculoId = novaRecolha.Id;

                foreach (var file in files)
                {
                    var nome = Path.GetFileNameWithoutExtension(file.FileName);
                    var extensao = Path.GetExtension(file.FileName);
                    var fotografia = new Fotografia
                    {
                        Nome = nome,
                        Extensao = extensao,
                        RecolhaVeiculo = novaRecolha
                    };
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        fotografia.Data = dataStream.ToArray();
                    }
                    _context.Fotografias.Add(fotografia);
                    novaRecolha.Fotografias.Add(fotografia);

                }
                _context.Recolhas.Update(novaRecolha);
                _context.Reservas.Update(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(recolha);
        }

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Details(int? reservaId)
        {
            var reserva = await _context.Reservas.FirstOrDefaultAsync(m => m.Id == reservaId);
            var veiculo = await _context.Veiculos.FirstOrDefaultAsync(m => m.Id == reserva.VeiculoId);
            var recolha = await _context.Recolhas.FirstOrDefaultAsync(m => m.Id == reserva.RecolhaVeiculoId);
            var entrega = await _context.Entregas.FirstOrDefaultAsync(m => m.Id == reserva.EntregaVeiculoId);
            var fotos = await _context.Fotografias.Where(m => m.RecolhaVeiculoId == recolha.Id).ToListAsync();
            recolha.Fotografias = fotos;
            var viewModel = new ReservaDetailsViewModel
            {
                Reserva = reserva,
                Cliente = null,
                Veiculo = veiculo,
                Funcionario = null,
                Recolha = recolha,
                Entrega = entrega
            };

            return View(viewModel);
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}
