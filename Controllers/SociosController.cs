using Demo.Models;
using Demo.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

public class SociosController : Controller
{
    private readonly SocioRepositorio _repositorio;
    public SociosController(SocioRepositorio repositorio) => _repositorio = repositorio;

    public async Task<IActionResult> Index() => View(await _repositorio.ListarAsync());

    public IActionResult Create() => View(new Socio());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Socio socio)
    {
        if (!ModelState.IsValid) return View(socio);
        try
        {
            await _repositorio.CrearAsync(socio);
            TempData["Exito"] = "El socio fue registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            ModelState.AddModelError("DNI", "El DNI ya se encuentra registrado.");
        }
        return View(socio);
    }
}
