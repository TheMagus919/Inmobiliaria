using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto.Models{
    [Table("Inmuebles")]
public class Inmueble{
    [Key]
    public int IdInmueble { get; set;}

    [Required]
    public int IdPropietario { get; set;}

    [Required(ErrorMessage = "El campo Cantidad De Ambientes es obligatorio")]
    [RegularExpression(@"^\d{1,10}$", ErrorMessage = "Debe tener entre 1 y 10 ambientes")]
    public int CantidadDeAmbientes { get; set;}

    [Required(ErrorMessage = "El campo Uso es obligatorio")]
    public string Uso { get; set;} = "";

    [Required(ErrorMessage = "El campo Direccion es obligatorio")]
    public string Direccion { get; set;} = "";

    [Required(ErrorMessage = "El campo Tipo es obligatorio")]
    public string Tipo { get; set;} = "";

    [Required(ErrorMessage = "El campo Longitud es obligatorio")]
    [RegularExpression(@"^\-?\d{6,9}$", ErrorMessage = "Debe tener entre 6 y 9 digitos, puede comenzar con -")]
    public decimal Longitud { get; set;}

    [Required(ErrorMessage = "El campo Latitud es obligatorio")]
    [RegularExpression(@"^\-?\d{6,9}$", ErrorMessage = "Debe tener entre 6 y 9 digitos, puede comenzar con -")]
    public decimal Latitud { get; set;}

    [Required(ErrorMessage = "El campo Precio es obligatorio")]
    [RegularExpression(@"^\-?\d{6,9}$", ErrorMessage = "Debe tener entre 6 y 9 digitos, puede comenzar con -")]
    public decimal Precio { get; set;}

    [Required(ErrorMessage = "El campo Disponible es obligatorio")]
    public bool Disponible { get; set;}

    [ForeignKey(nameof(IdPropietario))]
    public Propietario? Duenio { get; set;}

    public override string ToString()
        {
            return $"{Duenio.Nombre} {Duenio.Apellido}";
        }
}
}