using Demo.Models;
using Demo.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.Controllers;

public class LibrosController : Controller
{
    private readonly LibroRepositorio _repositorio;
    public LibrosController(LibroRepositorio repositorio) => _repositorio = repositorio;

    public async Task<IActionResult> Index(string? buscar)
    {
        ViewData["Buscar"] = buscar;
        return View(await _repositorio.ListarAsync(buscar));
    }

    public async Task<IActionResult> Details(int id)
    {
        var libro = await _repositorio.ObtenerPorIdAsync(id);
        return libro is null ? NotFound() : View(libro);
    }

    public async Task<IActionResult> Create()
    {
        await CargarAutoresAsync();
        return View(new Libro { Ejemplares = 1 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }
        try
        {
            await _repositorio.CrearAsync(libro);
            TempData["Exito"] = "El libro fue registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            ModelState.AddModelError("ISBN", "No se pudo registrar el libro. Verifique que el ISBN no esté repetido.");
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var libro = await _repositorio.ObtenerPorIdAsync(id);
        if (libro is null) return NotFound();
        await CargarAutoresAsync(libro.AutorId);
        return View(libro);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Libro libro)
    {
        if (id != libro.LibroId) return NotFound();
        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }
        try
        {
            await _repositorio.ActualizarAsync(libro);
            TempData["Exito"] = "Los cambios del libro fueron guardados.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            ModelState.AddModelError("ISBN", "No se pudo actualizar el libro. Verifique que el ISBN no esté repetido.");
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var libro = await _repositorio.ObtenerPorIdAsync(id);
        return libro is null ? NotFound() : View(libro);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repositorio.EliminarAsync(id);
        TempData["Exito"] = "El libro fue retirado del catálogo.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarAutoresAsync(int? seleccionado = null) =>
        ViewBag.Autores = new SelectList(await _repositorio.ListarAutoresAsync(), "AutorId", "Nombre", seleccionado);
}
