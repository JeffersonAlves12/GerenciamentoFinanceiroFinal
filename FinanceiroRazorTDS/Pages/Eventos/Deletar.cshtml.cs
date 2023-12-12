using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceiroRazorTDS.Models;
using FinanceiroRazorTDS.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroRazorTDS.Pages.Eventos
{
    public class Deletar : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public EventoModel EventoModel { get; set; }

        public string ErrorMessage { get; set; }

        public Deletar(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            EventoModel = await _context.Eventos.FindAsync(id);

            if (EventoModel == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var eventoToDelete = await _context.Eventos.FindAsync(EventoModel.Id);

            if (eventoToDelete == null)
            {
                return NotFound();
            }

            try
            {
                _context.Eventos.Remove(eventoToDelete);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException /* ex */)
            {
                // Log the error (uncomment ex variable name and write a log.)
                ErrorMessage = "Não foi possível excluir o evento. Tente novamente, e se o problema persistir " +
                               "contate o administrador do sistema.";
                return Page();
            }
        }
    }
}
