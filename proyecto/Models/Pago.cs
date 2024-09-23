using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace proyecto.Models{
    [Table("Pagos")]
public class Pago{
    [Key]
    public int IdPago { get; set;}

    [Required]
    public int NumeroDePago { get; set;}

    [Required]   
    public DateTime FechaDePago { get; set;}

    [Required]
    public int IdContrato{ get; set;}

    [Required]
    public decimal Importe { get; set;}
    
    [ForeignKey(nameof(IdContrato))]
    public Contrato? Contrato { get; set;}
}
}