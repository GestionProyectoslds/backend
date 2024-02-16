using Microsoft.EntityFrameworkCore;
using BarradeBusqueda.Models;

namespace BarradeBusqueda.Data
{
    public class BarradeBusquedaContext : DbContext
    {
        public BarradeBusquedaContext(DbContextOptions<BarradeBusquedaContext> options)
            : base(options)
        {
        }

        public DbSet<CategoriaPrincipal> CategoriasPrincipales { get; set; }
        public DbSet<Subcategoria> Subcategorias { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
    }
}
