using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto.Models{
    [Table("Inquilinos")]
public class Inquilino{
    [Key]
    public int IdInquilino { get; set;}
    [Required]
    public string Nombre { get; set;} = "";
    [Required]
    public string Apellido { get; set;} = "";
    [Required]
    public string Dni { get; set;} = "";
    public string Telefono { get; set;} = "";
    [Required, EmailAddress]
    public string Email { get; set;} = "";
    public override string ToString()
        {
            return $"{Nombre} {Apellido}";
        }
}
}