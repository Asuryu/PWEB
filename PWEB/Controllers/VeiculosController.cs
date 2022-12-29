﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWEB.Data;
using PWEB.Models;
using PWEB_AulasP_2223.Data;
using PWEB_AulasP_2223.Models;

namespace PWEB_AulasP_2223.Controllers
{
    public class VeiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index(bool? ativos, bool? ordenarAscendentemente)
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");

            if (ativos != null)
            {
                if (ordenarAscendentemente != null) // 1 1
                {
                    if (ordenarAscendentemente == true)
                    {
                        return View(await _context.Veiculos.Where(c => c.Ativo == ativos)
                            .OrderBy(s => s.Custo)
                            .ToListAsync());
                    }
                    else
                    {
                        return View(await _context.Veiculos.Where(c => c.Ativo == ativos)
                            .OrderByDescending(s => s.Custo)
                            .ToListAsync());
                    }
                }
                else // 1 0
                {
                    return View(await _context.Veiculos.Where(c => c.Ativo == ativos)
                        .ToListAsync());
                }
            }
            else
            {
                if (ordenarAscendentemente != null) // 0 1
                {
                    if (ordenarAscendentemente == true)
                    {
                        return View(await _context.Veiculos
                            .OrderBy(s => s.Custo)
                            .ToListAsync());
                    }
                    else
                    {
                        return View(await _context.Veiculos
                            .OrderByDescending(s => s.Custo)
                            .ToListAsync());
                    }
                }
                else // 0 0
                {
                    return View(await _context.Veiculos.ToListAsync());
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string TextoAPesquisar, int CategoriaId)
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");

            if (string.IsNullOrWhiteSpace(TextoAPesquisar))
                return View(_context.Veiculos.Where(c => c.CategoriaId == CategoriaId));
            else
            {
                var resultado = from c in _context.Veiculos
                                where c.Marca.Contains(TextoAPesquisar) || c.Modelo.Contains(TextoAPesquisar) || c.Localizacao.Contains(TextoAPesquisar) || c.Estado.Contains(TextoAPesquisar) && c.CategoriaId == CategoriaId
                                select c;
                return View(resultado);
            }
        }

        // GET: Categorias/Details/5
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

            return View(veiculo);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome");
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Marca,Modelo,Localizacao,Estado,Custo,CategoriaId,Ativo")] Veiculo veiculo)
        {
            
            ModelState.Remove(nameof(veiculo.Empresa));
            ModelState.Remove(nameof(veiculo.Categoria));
            ModelState.Remove(nameof(veiculo.Reservas));

            if (ModelState.IsValid)
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(veiculo);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Veiculos == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Nome", veiculo.CategoriaId);
            return View(veiculo);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Marca,Modelo,Localizacao,Estado,Custo,CategoriaId,Ativo")] Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(veiculo.Categoria));
            ModelState.Remove(nameof(veiculo.Empresa));
            ModelState.Remove(nameof(veiculo.Reservas));

            if (ModelState.IsValid)
            {
                Console.WriteLine(ModelState.Keys.Count());
                try
                {
                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.Id))
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
            return View(veiculo);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(veiculo);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Veiculos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Veiculos'  is null.");
            }
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo != null)
            {
                // TODO: verificar se o veiculo não tem reservas
                var resultado = from c in _context.Reservas
                                where c.VeiculoId == veiculo.Id
                                select c;
                if(resultado.Count() > 0)
                {
                    return Problem("O veículo não pôde ser removido pois existem reservas para o mesmo.");
                }
                _context.Veiculos.Remove(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoExists(int id)
        {
          return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}
