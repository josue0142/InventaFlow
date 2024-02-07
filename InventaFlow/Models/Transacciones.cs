using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaInventario.Models
{
    public class Transacciones
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = Application.ERRORMESSAGE_FIELD_STRINGLENGHT)]
        [Display(Name = "Tipo Trasacción")]
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        public string TipoTrasaccion { get; set; }
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        [Display(Name = "Artículo")]
        public int? IdArticulo { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        public int Cantidad { get; set; }
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        [Range(1,300000,ErrorMessage = Application.ERRORMESSAGE_FIELD_RANGE)]
        public decimal Monto { get; set; }

        [ForeignKey("IdArticulo")]
        public virtual Articulos Articulos { get; set; }

    }
}