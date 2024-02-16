using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarradeBusqueda.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BarradeBusqueda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusquedaController : ControllerBase
    {
        private readonly BarradeBusquedaContext _context;

        public BusquedaController(BarradeBusquedaContext context)
        {
            _context = context;
        }

        // Buscar en todas las entidades: CategoriasPrincipales, Subcategorias y Proyectos
        [HttpGet("{termino}")]
        public async Task<IActionResult> Buscar(string termino)
        {
            var categorias = await _context.CategoriasPrincipales
                .Where(c => c.Nombre_Categoria.Contains(termino))
                .ToListAsync();

            var subcategorias = await _context.Subcategorias
                .Where(s => s.Nombre_Subcategoria.Contains(termino))
                .ToListAsync();

            var proyectos = await _context.Proyectos
                .Where(p => p.Nombre_Proyecto.Contains(termino))
                .ToListAsync();

            var resultados = new
            {
                Categorias = categorias,
                Subcategorias = subcategorias,
                Proyectos = proyectos
            };

            if (!categorias.Any() && !subcategorias.Any() && !proyectos.Any())
            {
                return NotFound("No se encontraron resultados.");
            }

            return Ok(resultados);
        }
    }
}
