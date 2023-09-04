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
    [Required]
    public int IdInmueble { get; set;}
    [Required]
    public int IdInquilino { get; set;}
    [Required]
    public DateTime FechaDesde { get; set;}
    [Required]
    public DateTime FechaHasta { get; set;}

}
}
