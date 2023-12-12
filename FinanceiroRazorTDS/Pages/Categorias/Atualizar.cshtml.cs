using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinanceiroRazorTDS.Models;
using System.Threading.Tasks;
using FinanceiroRazorTDS.Data;

namespace FinanceiroRazorTDS.Pages.Categorias
{
    public class Atualizar : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public CategoriaModel Categoria { get; set; }

        public Atualizar(AppDbContext context)
        {
            _context = context;
        }

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

        public async Task<IActionResult> OnPostAsync(long id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var categoriaToUpdate = await _context.Categorias.FindAsync(id);

            if (categoriaToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<CategoriaModel>(
                categoriaToUpdate,
                "categoria", // Prefixo para o bind
                c => c.NomeCategoria, c => c.IconeCategoria, c => c.DescricaoCategoria))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
