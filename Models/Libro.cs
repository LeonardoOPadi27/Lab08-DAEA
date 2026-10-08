using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class Libro
{
    public int LibroId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    [StringLength(20, ErrorMessage = "El ISBN no puede superar los 20 caracteres.")]
    public string ISBN { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un autor.")]
    [Display(Name = "Autor")]
    public int AutorId { get; set; }

    [Range(0, 9999, ErrorMessage = "Los ejemplares deben ser un número igual o mayor que cero.")]
    [Display(Name = "Ejemplares")]
    public int Ejemplares { get; set; }

    public bool Activo { get; set; }
    public string? AutorNombre { get; set; }
}
