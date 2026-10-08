using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class Socio
{
    public int SocioId { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe contener solo 8 dígitos.")]
    public string DNI { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
    [StringLength(100)]
    public string? Email { get; set; }
    public bool Activo { get; set; }
}
