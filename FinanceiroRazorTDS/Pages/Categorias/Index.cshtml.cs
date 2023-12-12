using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceiroRazorTDS.Models;
using FinanceiroRazorTDS.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FinanceiroRazorTDS.Pages.Categorias
{
    public class Index : PageModel
    {
        private readonly AppDbContext _context;
        private readonly int itensPorPagina = 10;

        public Index(AppDbContext context)
        {
            _context = context;
        }

        public IList<CategoriaModel> Categorias { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FiltroNome { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PaginaAtual { get; set; } = 1;

        public bool TemPaginaAnterior => PaginaAtual > 1;
        public bool TemPaginaSeguinte => Categorias.Count == itensPorPagina;

        public async Task OnGetAsync()
        {
            var query = _context.Categorias.AsNoTracking();

            if (!string.IsNullOrEmpty(FiltroNome))
            {
                query = query.Where(c => c.NomeCategoria.Contains(FiltroNome));
            }

            Categorias = await query
                .Skip((PaginaAtual - 1) * itensPorPagina)
                .Take(itensPorPagina)
                .ToListAsync();
        }
    }
}
