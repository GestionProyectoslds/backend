using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarradeBusqueda.Models
{
    public class Subcategoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Subcategoria { get; set; }

        public int? ID_Categoria { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre_Subcategoria { get; set; }

        [ForeignKey("ID_Categoria")]
        public virtual CategoriaPrincipal CategoriaPrincipal { get; set; }

        public virtual ICollection<Proyecto> Proyectos { get; set; }
    }
}
