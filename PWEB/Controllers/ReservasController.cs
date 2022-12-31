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

            var current_user = await _userManager.GetUserAsync(HttpContext.User);
            return View(await _context.Reservas
                .Where(c => c.ClienteId == current_user.Id)
                .ToListAsync());
        }

        [Authorize]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> ReservationsHistory()
        {
            ViewData["Title"] = "Lista de Reservas";

            var current_user = await _userManager.GetUserAsync(HttpContext.User);
            return View(await _context.Reservas.Where(c => c.ClienteId == current_user.Id).ToListAsync());
        }

        [Authorize]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> DetailsClient(int? reservaId)
        {
            var reserva = await _context.Reservas.FirstOrDefaultAsync(m => m.Id == reservaId);
            var veiculo = await _context.Veiculos.FirstOrDefaultAsync(m => m.Id == reserva.VeiculoId);
            var recolha = await _context.Recolhas.FirstOrDefaultAsync(m => m.Id == reserva.RecolhaVeiculoId);
            var entrega = await _context.Entregas.FirstOrDefaultAsync(m => m.Id == reserva.EntregaVeiculoId);
            if (recolha != null)
            {
                var fotos = await _context.Fotografias.Where(m => m.RecolhaVeiculoId == recolha.Id).ToListAsync();
                recolha.Fotografias = fotos;
            }
            // get cliente object from reserva.clienteId
            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == reserva.ClienteId);

            var viewModel = new ReservaDetailsViewModel
            {
                Reserva = reserva,
                Cliente = cliente,
                Veiculo = veiculo,
                Funcionario = null,
                Recolha = recolha,
                Entrega = entrega
            };

            return View(viewModel);
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
                return View(await _context.Reservas.Where(c => c.Confirmada == confirmadas && c.EmpresaId == Empresa.Id)
                            .ToListAsync());
            }
            else
            {
                return View(await _context.Reservas.Where(c => c.EmpresaId == Empresa.Id).ToListAsync());
            }

        }

        [Authorize]
        [Authorize(Roles = "Funcionario,Gestor")]
        [HttpPost]
        public async Task<IActionResult> Index(int CategoriaId, int VeiculoId, string ClienteId)
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            ViewData["ListaDeVeiculos"] = new SelectList(_context.Veiculos.ToList(), "Id", "Marca");
            ViewData["ListaDeClientes"] = new SelectList(_context.Clientes.ToList(), "Id", "PrimeiroNome");

            var current_user = await _userManager.GetUserAsync(HttpContext.User);
            Empresa Empresa = await _context.Empresas.FindAsync(current_user.EmpresaId);
            ViewBag.NomeEmpresa = Empresa.Nome;

            var resultado = from c in _context.Reservas
                            where (c.Veiculo.CategoriaId == CategoriaId || c.VeiculoId == VeiculoId || c.ClienteId == ClienteId) && c.EmpresaId == Empresa.Id
                            select c;
            return View(resultado);
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
                if (isConfirmed != true)
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
            if (recolha != null)
            {
                var fotos = await _context.Fotografias.Where(m => m.RecolhaVeiculoId == recolha.Id).ToListAsync();
                recolha.Fotografias = fotos;
            }
            // get cliente object from reserva.clienteId
            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == reserva.ClienteId);

            var viewModel = new ReservaDetailsViewModel
            {
                Reserva = reserva,
                Cliente = cliente,
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
