using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarradeBusqueda.Models
{
    public class CategoriaPrincipal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Categoria { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre_Categoria { get; set; }

        public virtual ICollection<Subcategoria> Subcategorias { get; set; }
    }
}
