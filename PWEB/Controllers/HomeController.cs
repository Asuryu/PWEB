using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWEB.Data;
using PWEB_AulasP_2223.Data;
using PWEB_AulasP_2223.Models;
using System.Diagnostics;
using PWEB_AulasP_2223.ViewModels;
using PWEB.Models;
using Microsoft.CodeAnalysis;

namespace PWEB_AulasP_2223.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, 
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Index()
        {
            // Obter lista de todas as localizações dos veículos
            var locations = _context.Veiculos
                .Select(v => v.Localizacao)
                .Distinct()
                .ToList();
            ViewData["Locations"] = new SelectList(locations);
            
            var categories = _context.Categorias
                .Select(c => c.Nome)
                .Distinct()
                .ToList();
            ViewData["Categories"] = new SelectList(categories);
            
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Veiculos == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }
            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Id == veiculo.EmpresaId);
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == veiculo.CategoriaId);
            veiculo.Empresa = empresa;
            veiculo.Categoria = categoria;

            return View(veiculo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<IActionResult> Search([Bind("Location,Category,PickupDateAndTime,ReturnDateAndTime")] VehicleSearchViewModel search)
        {
            ViewData["Categorias"] = new SelectList(_context.Categorias, "Id", "Nome");
            ViewData["Empresas"] = new SelectList(_context.Empresas, "Id", "Nome");
            
            ModelState.Remove(nameof(search.Veiculos));

            ViewData["Categories"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            ViewData["Empresas"] = new SelectList(_context.Empresas.ToList(), "Id", "Nome");

            if (search.PickupDateAndTime > search.ReturnDateAndTime)
                return Problem("Pickup date must be before return date");

            if (ModelState.IsValid)
            {
                var vehicles = await _context.Veiculos
                    .Where(v => v.Localizacao == search.Location)
                    .Where(v => v.Categoria.Nome == search.Category)
                    .Where(v => v.Reservas
                        .All(r => r.DataLevantamento > search.ReturnDateAndTime ||
                                  r.DataEntrega < search.PickupDateAndTime))
                    .ToListAsync();

                vehicles.ForEach(v => v.Categoria = _context.Categorias.Find(v.CategoriaId));
                vehicles.ForEach(v => v.Empresa = _context.Empresas.Find(v.EmpresaId));

                search.Veiculos = vehicles;

                return View("Search", search);
            }

            return Problem("Modelo inválido");
        }

        [Authorize]
        public IActionResult Search()
        {

            ViewData["Categories"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            ViewData["Empresas"] = new SelectList(_context.Empresas.ToList(), "Id", "Nome");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Fixe(int? CategoriaId, int? EmpresaId, [Bind("Location,Category,PickupDateAndTime,ReturnDateAndTime")] VehicleSearchViewModel search)
        {

            ViewData["Categories"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            ViewData["Empresas"] = new SelectList(_context.Empresas.ToList(), "Id", "Nome");

            ModelState.Remove(nameof(search.Veiculos));
            Console.WriteLine(CategoriaId);
            Console.WriteLine(EmpresaId);

            if (search.PickupDateAndTime > search.ReturnDateAndTime)
                return Problem("Pickup date must be before return date");

            if (ModelState.IsValid)
            {
                if(CategoriaId != null)
                {
                    var vehicles = await _context.Veiculos
                    .Where(v => v.Localizacao == search.Location)
                    .Where(v => v.CategoriaId == CategoriaId)
                    .Where(v => v.Reservas
                        .All(r => r.DataLevantamento > search.ReturnDateAndTime ||
                                  r.DataEntrega < search.PickupDateAndTime))
                    .ToListAsync();

                    vehicles.ForEach(v => v.Categoria = _context.Categorias.Find(v.CategoriaId));
                    vehicles.ForEach(v => v.Empresa = _context.Empresas.Find(v.EmpresaId));

                    search.Veiculos = vehicles;

                    return View("Search", search);
                }

                if (EmpresaId != null)
                {
                    var vehicles = await _context.Veiculos
                    .Where(v => v.Localizacao == search.Location)
                    .Where(v => v.Categoria.Nome == search.Category)
                    .Where(v => v.EmpresaId == EmpresaId)
                    .Where(v => v.Reservas
                        .All(r => r.DataLevantamento > search.ReturnDateAndTime ||
                                  r.DataEntrega < search.PickupDateAndTime))
                    .ToListAsync();

                    vehicles.ForEach(v => v.Categoria = _context.Categorias.Find(v.CategoriaId));
                    vehicles.ForEach(v => v.Empresa = _context.Empresas.Find(v.EmpresaId));

                    search.Veiculos = vehicles;

                    return View("Search", search);
                }

                var vehicless = await _context.Veiculos
                    .Where(v => v.Localizacao == search.Location)
                    .Where(v => v.Categoria.Nome == search.Category)
                    .Where(v => v.Reservas
                        .All(r => r.DataLevantamento > search.ReturnDateAndTime ||
                                  r.DataEntrega < search.PickupDateAndTime))
                    .ToListAsync();

                vehicless.ForEach(v => v.Categoria = _context.Categorias.Find(v.CategoriaId));
                vehicless.ForEach(v => v.Empresa = _context.Empresas.Find(v.EmpresaId));

                search.Veiculos = vehicless;

                return View("Search", search);


            }

            return Problem("Modelo inválido");
        }
    }
}