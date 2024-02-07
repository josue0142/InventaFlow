using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaInventario.Models
{
    public class Almacenes
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100,ErrorMessage = Application.ERRORMESSAGE_FIELD_STRINGLENGHT)]
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = Application.ERRORMESSAGE_FIELD_REQUIRED)]
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

        [InverseProperty("Almacenes")]
        public virtual ICollection<ExistenciasXAlmacenes> ExistenciasXAlmacenes { get; set; }
    }
}