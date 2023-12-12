using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceiroRazorTDS.Data;
using FinanceiroRazorTDS.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroRazorTDS.Pages.Categorias
{
    public class Visualizar : PageModel
    {
        private readonly AppDbContext _context;

        public Visualizar(AppDbContext context)
        {
            _context = context;
        }

        public CategoriaModel Categoria { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Categoria = await _context.Categorias.FirstOrDefaultAsync(m => m.CategoriaId == id);

            if (Categoria == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
