using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceiroRazorTDS.Models;
using FinanceiroRazorTDS.Data;

namespace FinanceiroRazorTDS.Pages.Categorias
{
    public class Criar: PageModel
    {
        private readonly AppDbContext _context;

        public Criar(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CategoriaModel Categoria { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Categorias.Add(Categoria);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
