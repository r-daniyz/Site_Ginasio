using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitMyGoela.Data;
using FitMyGoela.Models;
using FitMyGoela.ViewModels;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FitMyGoela.Controllers
{
    [Authorize]
    public class ExerciciosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX - Com Filtros, Paginação e Filtro de Utilizador
        public async Task<IActionResult> Index(string pesquisa, int? tiposdetreinoId, int page = 1)
        {
            // Obtém ID do utilizador que está logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int pageSize = 5;

            //  Filtra os EXERCÍCIOS pelo utilizador
            var exerciciosQuery = _context.Exercicios
                .Where(e => e.UserId == userId)
                .Include(p => p.TiposDeTreino)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pesquisa))
            {
                exerciciosQuery = exerciciosQuery.Where(p => p.Nome.Contains(pesquisa));
            }

            if (tiposdetreinoId.HasValue)
            {
                exerciciosQuery = exerciciosQuery.Where(p => p.TiposDeTreinoId == tiposdetreinoId);
            }

            int totalExercicios = await exerciciosQuery.CountAsync();

            var exercicios = await exerciciosQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new ExercicioFiltroViewModel
            {
                Exercicios = exercicios,

                // Filtrar os TIPOS DE TREINO pelo utilizador logado
                TiposDeTreinos = await _context.TiposDeTreinos
                    .Where(t => t.UserId == userId)
                    .ToListAsync(),

                Pesquisa = pesquisa,
                TiposDeTreinoId = tiposdetreinoId,
                PaginaAtual = page,
                TotalPaginas = (int)Math.Ceiling(totalExercicios / (double)pageSize)
            };

            return View(viewModel);
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios
                .Include(e => e.TiposDeTreino)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exercicio == null) return NotFound();

            return View(exercicio);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["TiposDeTreinoId"] = new SelectList(_context.TiposDeTreinos.Where(t => t.UserId == userId), "Id", "Nome");
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exercicio exercicio, IFormFile? foto)
        {
            // Captura o ID do utilizador logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            exercicio.UserId = userId; 

           
            ModelState.Remove("UserId");
            ModelState.Remove("TiposDeTreino"); 

            if (ModelState.IsValid)
            {
                if (foto != null && foto.Length > 0)
                {
                    exercicio.Imagem = await SalvarFicheiro(foto);
                }

                _context.Add(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

          
            ViewData["TiposDeTreinoId"] = new SelectList(_context.TiposDeTreinos.Where(t => t.UserId == userId), "Id", "Nome", exercicio.TiposDeTreinoId);
            return View(exercicio);
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null) return NotFound();

            // Pega se o ID do utilizador logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Se o exercício não pertencer ao user, bloqueia o acesso
            if (exercicio.UserId != userId) return Forbid();

            // Filtra para mostrar apenas os tipos de treino deste utilizador
            ViewData["TiposDeTreinoId"] = new SelectList(
                _context.TiposDeTreinos.Where(t => t.UserId == userId),
                "Id",
                "Nome",
                exercicio.TiposDeTreinoId
            );

            return View(exercicio);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exercicio exercicio, IFormFile? foto)
        {
            if (id != exercicio.Id) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            exercicio.UserId = userId;

            ModelState.Remove("UserId");
            ModelState.Remove("TiposDeTreino");

            if (ModelState.IsValid)
            {
                try
                {
                    var exercicioOriginal = await _context.Exercicios
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);

                    if (foto != null && foto.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(exercicioOriginal?.Imagem))
                        {
                            var caminhoAntigo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", exercicioOriginal.Imagem);
                            if (System.IO.File.Exists(caminhoAntigo)) System.IO.File.Delete(caminhoAntigo);
                        }

                        exercicio.Imagem = await SalvarFicheiro(foto);
                    }
                    else
                    {
                        exercicio.Imagem = exercicioOriginal?.Imagem;
                    }

                    _context.Update(exercicio);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExercicioExists(exercicio.Id)) return NotFound();
                    else throw;
                }
            }

            ViewData["TiposDeTreinoId"] = new SelectList(_context.TiposDeTreinos.Where(t => t.UserId == userId), "Id", "Nome", exercicio.TiposDeTreinoId);
            return View(exercicio);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios
                .Include(e => e.TiposDeTreino)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exercicio == null) return NotFound();

            return View(exercicio);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio != null)
            {
          
                if (!string.IsNullOrEmpty(exercicio.Imagem))
                {
                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", exercicio.Imagem);
                    if (System.IO.File.Exists(caminho)) System.IO.File.Delete(caminho);
                }

                _context.Exercicios.Remove(exercicio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // FUNÇÃO AUXILIAR
        private async Task<string> SalvarFicheiro(IFormFile ficheiro)
        {
            var pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");
            if (!Directory.Exists(pastaDestino)) Directory.CreateDirectory(pastaDestino);

            string nomeUnico = Guid.NewGuid().ToString() + Path.GetExtension(ficheiro.FileName);
            var caminhoCompleto = Path.Combine(pastaDestino, nomeUnico);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await ficheiro.CopyToAsync(stream);
            }
            return nomeUnico;
        }

        private bool ExercicioExists(int id) => _context.Exercicios.Any(e => e.Id == id);
    }
}