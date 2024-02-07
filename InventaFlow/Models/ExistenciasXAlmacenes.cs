using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaInventario.Models
{
    public class ExistenciasXAlmacenes
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        [Display(Name = "Almacén")]
        public int? IdAlmacen { get; set; }
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        [Display(Name = "Artículo")]
        public int? IdArticulo { get; set; }
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        public int Cantidad { get; set; }

        [ForeignKey("IdAlmacen")]
        public virtual Almacenes Almacenes { get; set; }
        [ForeignKey("IdArticulo")]
        public virtual Articulos Articulos { get; set; }
    }
}