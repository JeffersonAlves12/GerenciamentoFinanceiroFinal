using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinanceiroRazorTDS.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using FinanceiroRazorTDS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace FinanceiroRazorTDS.Pages.Investimentos
{
    public class Criar : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public InvestimentoModel InvestimentoModel { get; set; }

        public Criar(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Investimentos.Add(InvestimentoModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
