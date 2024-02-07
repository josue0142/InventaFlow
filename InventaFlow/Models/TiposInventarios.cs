using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Models
{
	public class TiposInventarios
	{
		[Key]
		public int Id { get; set; }
        [StringLength(100, ErrorMessage = Application.ERRORMESSAGE_FIELD_STRINGLENGHT)]
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        public string Descripcion { get; set; }
		public bool Estado { get; set; }

        [InverseProperty("TiposInventarios")]
        [Display(Name = "Tipo Inventario")]
        public virtual ICollection<Articulos> Articulos { get; set; }
    }
}
