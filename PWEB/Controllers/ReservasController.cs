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
        private readonly RoleManager<IdentityRole> _roleManager;

        public ReservasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(bool? ativas, bool? ordenarAscendentemente)
        {
            ViewData["Title"] = "Lista de Reservas";

            if (ativas != null) 
            {
                if(ordenarAscendentemente != null) // 1 1
                {
                    if (ordenarAscendentemente == true)
                    {
                        return View(await _context.Reservas.Where(c => c.SubscricaoAtiva == ativas)
                            .OrderBy(s => s.Avaliacao)
                            .ToListAsync());
                    } else
                    {
                        return View(await _context.Reservas.Where(c => c.SubscricaoAtiva == ativas)
                            .OrderByDescending(s => s.Avaliacao)
                            .ToListAsync());
                    }
                } else // 1 0
                {
                    return View(await _context.Reservas.Where(c => c.SubscricaoAtiva == ativas)
                        .ToListAsync());
                }
            } else
            {
                if(ordenarAscendentemente != null) // 0 1
                {
                    if (ordenarAscendentemente == true)
                    {
                        return View(await _context.Reservas
                            .OrderBy(s => s.Avaliacao)
                            .ToListAsync());
                    }
                    else
                    {
                        return View(await _context.Reservas
                            .OrderByDescending(s => s.Avaliacao)
                            .ToListAsync());
                    }
                } else // 0 0
                {
                    return View(await _context.Reservas.ToListAsync());
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string TextoAPesquisar)
        {
            ViewData["Title"] = "Lista de Reservas";

            if (TextoAPesquisar != null)
            {
                return View(await _context.Reservas.Where(c => c.Nome.Contains(TextoAPesquisar)).ToListAsync());
            }
            else
            {
                return View(await _context.Reservas.ToListAsync());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataLevantamento,DataEntrega")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reserva);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Reservas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataLevantamento,DataEntrega")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reserva);
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

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }

}