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

        public IActionResult Index()
        {
            // Obter lista de todas as localizações dos veículos
            var locations = _context.Veiculos
                .Select(v => v.Localizacao)
                .Distinct()
                .ToList();
            ViewData["Locations"] = new SelectList(locations);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<IActionResult> Search([Bind("Location,PickupDateAndTime,ReturnDateAndTime")] VehicleSearchViewModel search)
        {
            ViewData["Veiculos"] = new SelectList(_context.Veiculos, "Id", "Marca");

            Console.WriteLine(search.Location);
            Console.WriteLine(search.PickupDateAndTime);
            Console.WriteLine(search.ReturnDateAndTime);

            if (search.PickupDateAndTime > search.ReturnDateAndTime)
                ModelState.AddModelError("PickupDateAndTime", "A data de inicio não pode ser maior que a data de fim");

            ModelState.Remove(nameof(search.Veiculos));

            if (ModelState.IsValid)
            {
                var veiculos = await _context.Veiculos
                    .Include(v => v.Categoria)
                    .Include(v => v.Empresa)
                    .Include(v => v.Reservas)
                    .Where(v => v.Localizacao == search.Location)
                    .ToListAsync();

                search.Veiculos = veiculos;

                return View("Search", search);
            }

            return View("search", search);
        }
    }
}