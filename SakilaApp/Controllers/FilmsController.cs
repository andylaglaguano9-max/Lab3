using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SakilaApp.Data;
using SakilaApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace SakilaApp.Controllers
{
    [Authorize]
    public class FilmsController : Controller
    {
        private readonly SakilaContext _context;

        public FilmsController(SakilaContext context)
        {
            _context = context;
        }

        // GET: Films

        /* --- EVIDENCIA: CÓDIGO ANTERIOR (Join Básico) ---
        public async Task<IActionResult> Index()
        {
            var query = await _context.Films
                .Join(_context.Languages,
                    film => film.LanguageId,
                    language => language.LanguageId,
                    (film, language) => new { film, language })
                .Where(x => x.language.Name != "English")
                .OrderBy(x => x.film.Title)
                .ToListAsync();

            var peliculas = query.Select(x => 
            {
                x.film.Language = x.language;
                return x.film;
            }).ToList();

            return View(peliculas);
        }
        ------------------------------------------------- */

        /* --- EVIDENCIA: CÓDIGO ANTERIOR (Reto Action) ---
        public async Task<IActionResult> Index()
        {
            var peliculas = await _context.Films
                .Join(_context.FilmCategories,
                    film => film.FilmId,
                    filmCategory => filmCategory.FilmId,
                    (film, filmCategory) => new { film, filmCategory })
                .Join(_context.Categories,
                    temp => temp.filmCategory.CategoryId,
                    category => category.CategoryId,
                    (temp, category) => new { temp.film, category })
                .Where(x => x.category.Name == "Action")
                .OrderBy(x => x.film.Title)
                .Select(x => x.film)
                .ToListAsync();

            return View(peliculas);
        }
        ------------------------------------------------- */

        /* --- EVIDENCIA: CÓDIGO ANTERIOR (Reto Drama Top 10) ---
        public async Task<IActionResult> Index()
        {
            var query = await _context.Films
                .Include(f => f.Language) 
                .Join(_context.FilmCategories,
                    film => film.FilmId,
                    filmCategory => filmCategory.FilmId,
                    (film, filmCategory) => new { film, filmCategory })
                .Join(_context.Categories,
                    temp => temp.filmCategory.CategoryId,
                    category => category.CategoryId,
                    (temp, category) => new { temp.film, temp.filmCategory, category })
                .Where(x => x.category.Name == "Drama")
                .OrderByDescending(x => x.film.Length)
                .Take(10)
                .ToListAsync();

            var peliculas = query.Select(x => 
            {
                x.filmCategory.Category = x.category;
                x.film.FilmCategories.Add(x.filmCategory);
                return x.film;
            }).ToList();

            return View(peliculas);
        }
        ------------------------------------------------- */

        /* --- EVIDENCIA: CÓDIGO ANTERIOR (Reto 3 Joins - Comedy) ---
        public async Task<IActionResult> Index()
        {
            var query = await _context.Films
                .Join(_context.Languages,
                    film => film.LanguageId,
                    language => language.LanguageId,
                    (film, language) => new { film, language })
                .Join(_context.FilmCategories,
                    temp => temp.film.FilmId,
                    filmCategory => filmCategory.FilmId,
                    (temp, filmCategory) => new { temp.film, temp.language, filmCategory })
                .Join(_context.Categories,
                    temp => temp.filmCategory.CategoryId,
                    category => category.CategoryId,
                    (temp, category) => new { temp.film, temp.language, temp.filmCategory, category })
                .Where(x => x.language.Name == "English" && x.category.Name == "Comedy")
                .OrderBy(x => x.film.Title)
                .ToListAsync();

            var peliculas = query.Select(x => 
            {
                // Asignaciones manuales para que no salga "N/A" ni la celda en blanco
                x.film.Language = x.language;
                x.filmCategory.Category = x.category;
                x.film.FilmCategories.Add(x.filmCategory);
                
                return x.film;
            }).ToList();

            return View(peliculas);
        }
        ------------------------------------------------- */

        /* --- EVIDENCIA: CÓDIGO ANTERIOR (Reto Empieza con A) ---
        public async Task<IActionResult> Index()
        {
            var query = await _context.Films
                .Join(_context.Languages,
                    film => film.LanguageId,
                    language => language.LanguageId,
                    (film, language) => new { film, language })
                .Where(x => x.language.Name == "English" && EF.Functions.ILike(x.film.Title, "A%"))
                .OrderBy(x => x.film.Title)
                .ToListAsync();

            var peliculas = query.Select(x => 
            {
                // Asignamos el idioma para la vista
                x.film.Language = x.language;
                return x.film;
            }).ToList();

            return View(peliculas);
        }
        ------------------------------------------------- */

        /* --- EVIDENCIA: CÓDIGO ANTERIOR (Reto 5 primeras películas) ---
        // NUEVO RETO: Las 5 primeras películas ordenadas por precio (RentalRate)
        public async Task<IActionResult> Index()
        {
            var peliculas = await _context.Films
                .Include(f => f.Language) // Mapeamos el idioma para que la vista no quede en blanco
                .OrderByDescending(f => f.RentalRate)
                .Take(5)
                .ToListAsync();

            return View(peliculas);
        }
        ------------------------------------------------- */

        /* --- EVIDENCIA: CÓDIGO ANTERIOR (Buscador por Título) ---
        // Método Index con buscador
        public async Task<IActionResult> Index(string? buscar)
        {
            var consulta = _context.Films.Include(f => f.Language).AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(f => f.Title.Contains(buscar));
            }

            var peliculas = await consulta
                .OrderBy(f => f.Title)
                .ToListAsync();

            ViewBag.Buscar = buscar;
            return View(peliculas);
        }
        ------------------------------------------------- */

        /* --- EVIDENCIA: CÓDIGO ANTERIOR (Buscador Título y Duración) ---
        // Método Index con buscador por Título y Duración Mínima
        public async Task<IActionResult> Index(string? buscar, int? duracionMinima)
        {
            var consulta = _context.Films.Include(f => f.Language).AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(f => f.Title.Contains(buscar));
            }

            if (duracionMinima.HasValue)
            {
                consulta = consulta.Where(f => f.Length >= duracionMinima.Value);
            }

            var peliculas = await consulta
                .OrderBy(f => f.Title)
                .ToListAsync();

            ViewBag.Buscar = buscar;
            ViewBag.DuracionMinima = duracionMinima;

            return View(peliculas);
        }
        ------------------------------------------------- */

        // --- HISTORICAL ACTIONS FOR EVIDENCIA ---
        
        // 1. Join Básico (No Inglés, ordenado por Título)
        public async Task<IActionResult> IndexJoinBasico()
        {
            var query = await _context.Films
                .Join(_context.Languages,
                    film => film.LanguageId,
                    language => language.LanguageId,
                    (film, language) => new { film, language })
                .Where(x => x.language.Name != "English")
                .OrderBy(x => x.film.Title)
                .ToListAsync();

            var peliculas = query.Select(x => 
            {
                x.film.Language = x.language;
                return x.film;
            }).ToList();

            ViewBag.PaginaActual = 1;
            ViewBag.TotalPaginas = 1;
            return View("Index", peliculas);
        }

        // 2. Reto Action (Categoría Action, ordenado por Título)
        public async Task<IActionResult> IndexAction()
        {
            var peliculas = await _context.Films
                .Join(_context.FilmCategories,
                    film => film.FilmId,
                    filmCategory => filmCategory.FilmId,
                    (film, filmCategory) => new { film, filmCategory })
                .Join(_context.Categories,
                    temp => temp.filmCategory.CategoryId,
                    category => category.CategoryId,
                    (temp, category) => new { temp.film, category })
                .Where(x => x.category.Name == "Action")
                .OrderBy(x => x.film.Title)
                .Select(x => x.film)
                .ToListAsync();

            ViewBag.PaginaActual = 1;
            ViewBag.TotalPaginas = 1;
            return View("Index", peliculas);
        }

        // 3. Reto Drama Top 10 (Categoría Drama, ordenado por duración descendente, top 10)
        public async Task<IActionResult> IndexDramaTop10()
        {
            var query = await _context.Films
                .Include(f => f.Language) 
                .Join(_context.FilmCategories,
                    film => film.FilmId,
                    filmCategory => filmCategory.FilmId,
                    (film, filmCategory) => new { film, filmCategory })
                .Join(_context.Categories,
                    temp => temp.filmCategory.CategoryId,
                    category => category.CategoryId,
                    (temp, category) => new { temp.film, temp.filmCategory, category })
                .Where(x => x.category.Name == "Drama")
                .OrderByDescending(x => x.film.Length)
                .Take(10)
                .ToListAsync();

            var peliculas = query.Select(x => 
            {
                x.filmCategory.Category = x.category;
                x.film.FilmCategories.Add(x.filmCategory);
                return x.film;
            }).ToList();

            ViewBag.PaginaActual = 1;
            ViewBag.TotalPaginas = 1;
            return View("Index", peliculas);
        }

        // 4. Reto 3 Joins - Comedy (Idioma Inglés y Categoría Comedy, ordenado por Título)
        public async Task<IActionResult> IndexComedy()
        {
            var query = await _context.Films
                .Join(_context.Languages,
                    film => film.LanguageId,
                    language => language.LanguageId,
                    (film, language) => new { film, language })
                .Join(_context.FilmCategories,
                    temp => temp.film.FilmId,
                    filmCategory => filmCategory.FilmId,
                    (temp, filmCategory) => new { temp.film, temp.language, filmCategory })
                .Join(_context.Categories,
                    temp => temp.filmCategory.CategoryId,
                    category => category.CategoryId,
                    (temp, category) => new { temp.film, temp.language, temp.filmCategory, category })
                .Where(x => x.language.Name == "English" && x.category.Name == "Comedy")
                .OrderBy(x => x.film.Title)
                .ToListAsync();

            var peliculas = query.Select(x => 
            {
                x.film.Language = x.language;
                x.filmCategory.Category = x.category;
                x.film.FilmCategories.Add(x.filmCategory);
                return x.film;
            }).ToList();

            ViewBag.PaginaActual = 1;
            ViewBag.TotalPaginas = 1;
            return View("Index", peliculas);
        }

        // 5. Reto Empieza con A (Idioma Inglés, Título empieza con A, ordenado por Título)
        public async Task<IActionResult> IndexEmpiezaConA()
        {
            var query = await _context.Films
                .Join(_context.Languages,
                    film => film.LanguageId,
                    language => language.LanguageId,
                    (film, language) => new { film, language })
                .Where(x => x.language.Name == "English" && EF.Functions.ILike(x.film.Title, "A%"))
                .OrderBy(x => x.film.Title)
                .ToListAsync();

            var peliculas = query.Select(x => 
            {
                x.film.Language = x.language;
                return x.film;
            }).ToList();

            ViewBag.PaginaActual = 1;
            ViewBag.TotalPaginas = 1;
            return View("Index", peliculas);
        }

        // 6. Reto 5 primeras películas (Top 5 con mayor precio de alquiler)
        public async Task<IActionResult> IndexTop5Rental()
        {
            var peliculas = await _context.Films
                .Include(f => f.Language)
                .OrderByDescending(f => f.RentalRate)
                .Take(5)
                .ToListAsync();

            ViewBag.PaginaActual = 1;
            ViewBag.TotalPaginas = 1;
            return View("Index", peliculas);
        }

        // 7. Buscador por Título
        public async Task<IActionResult> IndexBuscadorTitulo(string? buscarString)
        {
            var consulta = _context.Films.Include(f => f.Language).AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscarString))
            {
                consulta = consulta.Where(f => f.Title.Contains(buscarString));
            }

            var peliculas = await consulta
                .OrderBy(f => f.Title)
                .ToListAsync();

            ViewBag.Buscar = buscarString;
            ViewBag.PaginaActual = 1;
            ViewBag.TotalPaginas = 1;
            return View("Index", peliculas);
        }

        // 8. Buscador Título y Duración
        public async Task<IActionResult> IndexBuscadorTituloDuracion(string? buscarString, int? duracionMinima)
        {
            var consulta = _context.Films.Include(f => f.Language).AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscarString))
            {
                consulta = consulta.Where(f => f.Title.Contains(buscarString));
            }

            if (duracionMinima.HasValue)
            {
                consulta = consulta.Where(f => f.Length >= duracionMinima.Value);
            }

            var peliculas = await consulta
                .OrderBy(f => f.Title)
                .ToListAsync();

            ViewBag.Buscar = buscarString;
            ViewBag.DuracionMinima = duracionMinima;
            ViewBag.PaginaActual = 1;
            ViewBag.TotalPaginas = 1;

            return View("Index", peliculas);
        }

        // --- CÓDIGO ACTUAL ACTIVO ---
        // Método Index con buscador (Título y Duración) y Paginación
        public async Task<IActionResult> Index(string? buscar, int? duracionMinima, int pagina = 1)
        {
            int tamanioPagina = 10;

            var consulta = _context.Films.Include(f => f.Language).AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(f => f.Title.Contains(buscar));
            }

            if (duracionMinima.HasValue)
            {
                consulta = consulta.Where(f => f.Length >= duracionMinima.Value);
            }

            int totalRegistros = await consulta.CountAsync();

            var peliculas = await consulta
                .OrderBy(f => f.Title)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();

            ViewBag.Buscar = buscar;
            ViewBag.DuracionMinima = duracionMinima;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanioPagina);

            return View(peliculas);
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .Include(f => f.Language)
                .Include(f => f.OriginalLanguage)
                .FirstOrDefaultAsync(m => m.FilmId == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create — Solo Administrador y Supervisor
        [Authorize(Roles = "Administrador,Supervisor")]
        public IActionResult Create()
        {
            ViewData["LanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageId");
            ViewData["OriginalLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageId");
            return View();
        }

        // POST: Films/Create — Solo Administrador y Supervisor
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Supervisor")]
        public async Task<IActionResult> Create([Bind("FilmId,Title,Description,ReleaseYear,LanguageId,OriginalLanguageId,RentalDuration,RentalRate,Length,ReplacementCost,LastUpdate,SpecialFeatures,Fulltext")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageId", film.LanguageId);
            ViewData["OriginalLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageId", film.OriginalLanguageId);
            return View(film);
        }

        // GET: Films/Edit/5 — Solo Administrador y Supervisor
        [Authorize(Roles = "Administrador,Supervisor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            ViewData["LanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageId", film.LanguageId);
            ViewData["OriginalLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageId", film.OriginalLanguageId);
            return View(film);
        }

        // POST: Films/Edit/5 — Solo Administrador y Supervisor
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Supervisor")]
        public async Task<IActionResult> Edit(int id, [Bind("FilmId,Title,Description,ReleaseYear,LanguageId,OriginalLanguageId,RentalDuration,RentalRate,Length,ReplacementCost,LastUpdate,SpecialFeatures,Fulltext")] Film film)
        {
            if (id != film.FilmId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.FilmId))
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
            ViewData["LanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageId", film.LanguageId);
            ViewData["OriginalLanguageId"] = new SelectList(_context.Languages, "LanguageId", "LanguageId", film.OriginalLanguageId);
            return View(film);
        }

        // GET: Films/Delete/5 — Solo Administrador
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .Include(f => f.Language)
                .Include(f => f.OriginalLanguage)
                .FirstOrDefaultAsync(m => m.FilmId == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5 — Solo Administrador
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film != null)
            {
                _context.Films.Remove(film);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.FilmId == id);
        }
    }
}
