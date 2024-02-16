using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarradeBusqueda.Models
{
    public class Proyecto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Proyecto { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre_Proyecto { get; set; }

        [StringLength(int.MaxValue)]
        public string Descripcion { get; set; }

        public int? ID_Categoria { get; set; }

        public int? ID_Subcategoria { get; set; }

        [Required]
        public DateTime Fecha_Creacion { get; set; }

        public DateTime? Fecha_Actualizacion { get; set; }

        [ForeignKey("ID_Categoria")]
        public virtual CategoriaPrincipal CategoriaPrincipal { get; set; }

        [ForeignKey("ID_Subcategoria")]
        public virtual Subcategoria Subcategoria { get; set; }
    }
}
