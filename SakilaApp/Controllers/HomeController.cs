using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SakilaApp.Models;
using SakilaApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SakilaApp.Controllers;

public class HomeController : Controller
{
    private readonly SakilaContext _context;

    public HomeController(SakilaContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Reto 2: Mostrar las 10 películas más largas.
    public async Task<IActionResult> Reto2()
    {
        var peliculas = await _context.Films
            .OrderByDescending(f => f.Length)
            .Take(10)
            .ToListAsync();
        return View(peliculas);
    }

    // Reto 3: Mostrar películas cuyo título contenga una palabra ingresada.
    public async Task<IActionResult> Reto3(string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return View(new List<Film>());
        }

        var peliculas = await _context.Films
            .Where(f => EF.Functions.ILike(f.Title, $"%{keyword}%"))
            .ToListAsync();
        return View(peliculas);
    }

    // Reto 9: Mostrar la cantidad total de clientes registrados.
    public async Task<IActionResult> Reto9()
    {
        var cantidad = await _context.Customers.CountAsync();
        ViewBag.CantidadTotal = cantidad;
        return View();
    }

    // Reto 16: Mostrar actores cuyo apellido empiece con una letra determinada.
    public async Task<IActionResult> Reto16(string initial)
    {
        if (string.IsNullOrEmpty(initial))
        {
            return View(new List<Actor>());
        }

        var actores = await _context.Actors
            .Where(a => EF.Functions.ILike(a.LastName, $"{initial}%"))
            .ToListAsync();
        return View(actores);
    }

    // Reto 17: Mostrar las primeras 10 películas ordenadas alfabéticamente.
    public async Task<IActionResult> Reto17()
    {
        var peliculas = await _context.Films
            .OrderBy(f => f.Title)
            .Take(10)
            .ToListAsync();
        return View(peliculas);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult PanelAdministrador()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
