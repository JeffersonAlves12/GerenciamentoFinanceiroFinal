using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceiroRazorTDS.Models;
using System.Threading.Tasks;
using FinanceiroRazorTDS.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroRazorTDS.Pages.Investimentos
{
    public class Atualizar: PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public InvestimentoModel Investimento { get; set; }

        public Atualizar(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Investimento = await _context.Investimentos.FirstOrDefaultAsync(m => m.Id == id);

            if (Investimento == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var investimentoToUpdate = await _context.Investimentos.FindAsync(id);

            if (investimentoToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<InvestimentoModel>(
                investimentoToUpdate,
                "investimento", // Prefixo para o BindProperty
                i => i.Nome, i => i.Tipo, i => i.ValorInvestido, i => i.DataDeCompra, i => i.ValorAtual))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
