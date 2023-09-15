using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto.Models{
    [Table("Usuarios")]
public class Usuario{
    [Key]
    public int IdUsuario { get; set;}
    [Required]
    public string Nombre { get; set;}
    [Required]
    public string Apellido { get; set;}
    [Required]
    public string Email { get; set;}
    [Required]
    public string Password { get; set;}
    [Required]
    public string Rol { get; set;}
    public string? Avatar { get; set;}
    [NotMapped]
    public IFormFile? AvatarFile {get; set;}
    public override string ToString()
        {
            return $"{Nombre} {Apellido}";
        }
}
}