using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto.Models{
    [Table("Contratos")]
public class Contrato{
    [Key]
    public int IdContrato { get; set;}

    [Required(ErrorMessage = "El campo Inmueble es obligatorio")]
    public int IdInmueble { get; set;}

    [Required(ErrorMessage = "El campo Inquilino es obligatorio")]
    public int IdInquilino { get; set;}

    [Required(ErrorMessage = "El campo Fecha Desde es obligatorio")]
    public DateTime? FechaDesde { get; set;}

    [Required(ErrorMessage = "El campo Fecha Hasta es obligatorio")]
    public DateTime? FechaHasta { get; set;}

    public bool Cancelado { get; set;}

    [ForeignKey(nameof(IdInquilino))]
    public Inquilino? Vive { get; set;}
    
    [ForeignKey(nameof(IdInmueble))]
    public Inmueble? Lugar { get; set;}

    public override string ToString()
        {
            return $" {Vive} ";
        }
}
}
