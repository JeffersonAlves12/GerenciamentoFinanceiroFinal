using Microsoft.AspNetCore.Mvc;
using FinanceiroRazorTDS.Data;
using FinanceiroRazorTDS.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FinanceiroRazorTDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Categoria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaModel>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        // Outros métodos, como POST, PUT, DELETE, etc.
    }
}
