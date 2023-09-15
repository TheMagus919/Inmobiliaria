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
    [Required]
    public int CantidadDeAmbientes { get; set;}
    [Required]
    public string Uso { get; set;} = "";
    [Required]
    public string Direccion { get; set;} = "";
    [Required]
    public string Tipo { get; set;} = "";
    public decimal Longitud { get; set;}
    public decimal Latitud { get; set;}
    public decimal Precio { get; set;}
    [Required]
    public bool Disponible { get; set;}
    [ForeignKey(nameof(IdPropietario))]
    public Propietario? Duenio { get; set;}

    public override string ToString()
        {
            return $"{Duenio.Nombre} {Duenio.Apellido}";
        }
}
}