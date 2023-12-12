using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinanceiroRazorTDS.Models;
using FinanceiroRazorTDS.Data;
using System.Threading.Tasks;
using System.Linq;

namespace FinanceiroRazorTDS.Pages.Eventos
{
    public class Atualizar : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public EventoModel Evento { get; set; }
        public SelectList UsuariosSelectList { get; set; }

        public Atualizar(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Evento = await _context.Eventos.FindAsync(id);
            if (Evento == null)
            {
                return NotFound();
            }

            UsuariosSelectList = new SelectList(_context.Usuarios, "Id", "PrimeiroNome");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                UsuariosSelectList = new SelectList(_context.Usuarios, "Id", "PrimeiroNome");
                return Page();
            }

            var eventoToUpdate = await _context.Eventos.FindAsync(Evento.Id);
            if (eventoToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<EventoModel>(
                eventoToUpdate,
                "evento",   // Prefix for form value.
                e => e.Tipo, e => e.DataConsolidacao, e => e.Valor, e => e.Status, e => e.UsuarioId))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
