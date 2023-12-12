using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceiroRazorTDS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceiroRazorTDS.Data;

namespace FinanceiroRazorTDS.Pages.Transacoes
{
    public class Atualizar : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public TransacaoModel Transacao { get; set; }

        public List<SelectListItem> CategoriaOptions { get; set; }
        public List<SelectListItem> UsuarioOptions { get; set; }
        public long[] SelectedCategoriaIds { get; set; }

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

            Transacao = await _context.Transacoes
                .Include(t => t.Categorias)
                .FirstOrDefaultAsync(m => m.TransacaoId == id);

            if (Transacao == null)
            {
                return NotFound();
            }

            CategoriaOptions = await _context.Categorias
                .Select(c => new SelectListItem
                {
                    Value = c.CategoriaId.ToString(),
                    Text = c.NomeCategoria
                }).ToListAsync();

            UsuarioOptions = await _context.Usuarios
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.PrimeiroNome} {u.UltimoNome}"
                }).ToListAsync();

            SelectedCategoriaIds = Transacao.Categorias.Select(c => c.CategoriaId).ToArray();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var transacaoToUpdate = await _context.Transacoes
                .Include(t => t.Categorias)
                .FirstOrDefaultAsync(t => t.TransacaoId == id);

            if (transacaoToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<TransacaoModel>(
                transacaoToUpdate,
                "transacao",
                t => t.NomeTransacao, t => t.ValorTransacao, t => t.TipoTransacao, t => t.DataTransacao, t => t.Descricao))
            {
                if (SelectedCategoriaIds != null && SelectedCategoriaIds.Length > 0)
                {
                    transacaoToUpdate.Categorias.Clear();
                    var selectedCategorias = await _context.Categorias
                        .Where(c => SelectedCategoriaIds.Contains(c.CategoriaId))
                        .ToListAsync();
                    
                    foreach (var categoria in selectedCategorias)
                    {
                        transacaoToUpdate.Categorias.Add(categoria);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
