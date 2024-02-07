using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaInventario.Models
{
    public class Articulos
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = Application.ERRORMESSAGE_FIELD_STRINGLENGHT)]
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        public int Existencia { get; set; }
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        [Display(Name = "Tipo Inventario")]
        public int? IdTipoInventario { get; set; }
        public bool Estado { get; set; }
        [Display(Name = "Costo Unitario")]
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        public decimal CostoUnitario { get; set; }

        [ForeignKey("IdTipoInventario")]
        public virtual TiposInventarios TiposInventarios { get; set; }

        [InverseProperty("Articulos")]
        public virtual ICollection<ExistenciasXAlmacenes> ExistenciasXAlmacenes { get; set; }  
        [InverseProperty("Articulos")]
        public virtual ICollection<Transacciones> Transacciones { get; set; }
    }
}
