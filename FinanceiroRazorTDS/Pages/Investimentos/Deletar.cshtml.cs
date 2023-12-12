using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinanceiroRazorTDS.Data;
using FinanceiroRazorTDS.Models;

namespace FinanceiroRazorTDS.Pages.Investimentos
{
    public class DeletarModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public InvestimentoModel InvestimentoModel { get; set; }

        public String ErrorMessage { get; set; }

        public DeletarModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investimento = await _context.Investimentos.FirstOrDefaultAsync(i => i.Id == id);

            if (investimento == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = String.Format("A exclusão do investimento com ID {0} falhou. Tente novamente", id);
            }

            InvestimentoModel = investimento;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var investimentoToRemove = await _context.Investimentos.FindAsync(id);

            if (investimentoToRemove == null)
            {
                return NotFound();
            }

            try
            {
                _context.Investimentos.Remove(investimentoToRemove);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Investimentos/Index");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("./Deletar", new { id, saveChangesError = true });
            }
        }
    }
}
