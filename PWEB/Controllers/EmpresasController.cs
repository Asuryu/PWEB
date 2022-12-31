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
    [Authorize]
    [Authorize(Roles = "Administrador")]
    public class EmpresasController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmpresasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(bool? ativas, bool? ordenarAscendentemente)
        {
            ViewData["Title"] = "Lista de Empresas";

            if (ativas != null) 
            {
                if(ordenarAscendentemente != null) // 1 1
                {
                    if (ordenarAscendentemente == true)
                    {
                        return View(await _context.Empresas.Where(c => c.SubscricaoAtiva == ativas)
                            .OrderBy(s => s.Avaliacao)
                            .ToListAsync());
                    } else
                    {
                        return View(await _context.Empresas.Where(c => c.SubscricaoAtiva == ativas)
                            .OrderByDescending(s => s.Avaliacao)
                            .ToListAsync());
                    }
                } else // 1 0
                {
                    return View(await _context.Empresas.Where(c => c.SubscricaoAtiva == ativas)
                        .ToListAsync());
                }
            } else
            {
                if(ordenarAscendentemente != null) // 0 1
                {
                    if (ordenarAscendentemente == true)
                    {
                        return View(await _context.Empresas
                            .OrderBy(s => s.Avaliacao)
                            .ToListAsync());
                    }
                    else
                    {
                        return View(await _context.Empresas
                            .OrderByDescending(s => s.Avaliacao)
                            .ToListAsync());
                    }
                } else // 0 0
                {
                    return View(await _context.Empresas.ToListAsync());
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string TextoAPesquisar)
        {
            if (string.IsNullOrWhiteSpace(TextoAPesquisar))
                return View();
            else
            {
                var resultado = from c in _context.Empresas
                                where c.Nome.Contains(TextoAPesquisar)
                                select c;
                return View(resultado);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Avaliacao,SubscricaoAtiva")] Empresa empresa)
        {

            ModelState.Remove(nameof(empresa.GestoresFuncionarios));
            ModelState.Remove(nameof(empresa.Veiculos));
            //ModelState.Remove(nameof(empresa.Reservas));

            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();

                var defaultUser = new ApplicationUser
                {
                    UserName = "gestor@" + empresa.Nome.ToLower() + ".com",
                    Email = "gestor@" + empresa.Nome.ToLower() + ".com",
                    PrimeiroNome = "Gestor",
                    UltimoNome = empresa.Nome,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    EmpresaId = empresa.Id,
                    Empresa = empresa,
                    CargoNaEmpresa = "Gestor"
                };
                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultUser, "ISEC_PWeb_2022!");
                    await _userManager.AddToRoleAsync(defaultUser, "Gestor");
                    empresa.GestoresFuncionarios.Add(defaultUser);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Avaliacao,SubscricaoAtiva")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(empresa.GestoresFuncionarios));
            ModelState.Remove(nameof(empresa.Veiculos));
            //ModelState.Remove(nameof(empresa.Reservas));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
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
            return View(empresa);
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

                // delete all users from this company (funcionario and gestor)
                var users = await _userManager.GetUsersInRoleAsync("Gestor");
                foreach (var user in users)
                {
                    if (user.EmpresaId == empresa.Id)
                    {
                        await _userManager.DeleteAsync(user);
                    }
                }
                users = await _userManager.GetUsersInRoleAsync("Funcionario");
                foreach (var user in users)
                {
                    if (user.EmpresaId == empresa.Id)
                    {
                        await _userManager.DeleteAsync(user);
                    }
                }

                _context.Empresas.Remove(empresa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresas.Any(e => e.Id == id);
        }
    }

}

