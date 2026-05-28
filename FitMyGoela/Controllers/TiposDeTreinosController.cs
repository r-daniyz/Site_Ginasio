using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitMyGoela.Data;
using FitMyGoela.Models;
using System.Security.Claims;

namespace FitMyGoela.Controllers
{
    public class TiposDeTreinosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TiposDeTreinosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TiposDeTreinos
        public async Task<IActionResult> Index(string pesquisa)
        {
            // Obtem o ID do utilizador logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Se for user novo, aparece dados novos
            var query = _context.TiposDeTreinos
                .Where(t => t.UserId == userId)
                .AsQueryable();


            if (!string.IsNullOrEmpty(pesquisa))
            {
                query = query.Where(t => t.Nome.Contains(pesquisa));
            }


            return View(await query.ToListAsync());
        }

        // GET: TiposDeTreinos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tiposDeTreino = await _context.TiposDeTreinos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tiposDeTreino == null)
            {
                return NotFound();
            }

            return View(tiposDeTreino);
        }

        // GET: TiposDeTreinos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposDeTreinos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,DuracaoMinutos")] TiposDeTreino tiposDeTreino)
        {
            if (ModelState.IsValid)
            {
                // Captura o ID do utilizador logado
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Associa o Tipo de Treino ao utilizador atual
                tiposDeTreino.UserId = userId;

                _context.Add(tiposDeTreino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tiposDeTreino);
        }

        // GET: TiposDeTreinos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tiposDeTreino = await _context.TiposDeTreinos.FindAsync(id);
            if (tiposDeTreino == null)
            {
                return NotFound();
            }
            return View(tiposDeTreino);
        }

        // POST: TiposDeTreinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TiposDeTreino tiposDeTreino)
        {
            if (id != tiposDeTreino.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            tiposDeTreino.UserId = userId;
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tiposDeTreino);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TiposDeTreinoExists(tiposDeTreino.Id))
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
            return View(tiposDeTreino);
        }

        // GET: TiposDeTreinos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tiposDeTreino = await _context.TiposDeTreinos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tiposDeTreino == null)
            {
                return NotFound();
            }

            return View(tiposDeTreino);
        }

        // POST: TiposDeTreinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tiposDeTreino = await _context.TiposDeTreinos.FindAsync(id);

            if (tiposDeTreino != null)
            {
                bool temExercicios = await _context.Exercicios.AnyAsync(e => e.TiposDeTreinoId == id);

                if (temExercicios)
                {
                    TempData["ErroDelete"] = "Não podes apagar este treino porque ele tem exercícios associados! Apaga primeiro os exercícios.";
                    return RedirectToAction(nameof(Index));
                }

                _context.TiposDeTreinos.Remove(tiposDeTreino);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TiposDeTreinoExists(int id)
        {
            return _context.TiposDeTreinos.Any(e => e.Id == id);
        }
    }
}
