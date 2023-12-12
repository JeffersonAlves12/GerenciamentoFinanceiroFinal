using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinanceiroRazorTDS.Data;
using FinanceiroRazorTDS.Models;

namespace FinanceiroRazorTDS.Pages.Categorias
{
    public class Deletar : PageModel
    {
        private readonly AppDbContext _context;
        [BindProperty]
        public CategoriaModel CategoriaModel { get; set; }
        public String ErrorMessage { get; set; }

        public Deletar(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(long? id, bool? saveChangesError = false)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = String.Format("A exclusão da categoria com ID {0} falhou. Tente novamente", id);
            }

            CategoriaModel = categoria;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            var categoriaToRemove = await _context.Categorias.FindAsync(id);

            if (categoriaToRemove == null)
            {
                return NotFound();
            }

            try
            {
                _context.Categorias.Remove(categoriaToRemove);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Categorias/Index");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("./Deletar", new { id, saveChangesError = true });
            }
        }
    }
}
