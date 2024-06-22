using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto.Models{
    [Table("Propietarios")]
public class Propietario{
    [Key]
    public int IdPropietario { get; set;}

    [Required(ErrorMessage = "El campo Nombre es obligatorio")]
    public string Nombre { get; set;} = "";

    [Required(ErrorMessage = "El campo Apellido es obligatorio")]
    public string Apellido { get; set;} = "";

    [Required(ErrorMessage = "El campo DNI es obligatorio")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos")]
    public string Dni { get; set;} = "";

    [Required(ErrorMessage = "El campo Telefono es obligatorio")]
    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "El teléfono debe tener entre 7 y 15 dígitos")]
    public string Telefono { get; set;} = "";

    [Required(ErrorMessage = "El campo Email es obligatorio")]
    [EmailAddress(ErrorMessage = "Email no válido")]
    public string Email { get; set;} = "";

        public override string ToString()
        {
            return $"{Nombre} {Apellido}";
        }
    }
}