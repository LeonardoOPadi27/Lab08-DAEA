using Demo.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

public class PrestamosController : Controller
{
    private readonly SocioRepositorio _repositorio;
    public PrestamosController(SocioRepositorio repositorio) => _repositorio = repositorio;

    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
    {
        ViewData["Desde"] = desde?.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = hasta?.ToString("yyyy-MM-dd");
        var rangoInvalido = desde.HasValue && hasta.HasValue && desde > hasta;
        ViewData["RangoInvalido"] = rangoInvalido;
        return View(rangoInvalido
            ? Enumerable.Empty<Demo.Models.PrestamoReporte>()
            : await _repositorio.ReporteAsync(desde, hasta));
    }
}
