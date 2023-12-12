using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceiroRazorTDS.Models;
using FinanceiroRazorTDS.Data;

namespace FinanceiroRazorTDS.Pages.Usuarios
{
    public class Cadastrar : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public UsuarioModel Usuario { get; set; }

        public Cadastrar(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Add(Usuario);
            await _context.SaveChangesAsync();

            // Redireciona para a página de confirmação ou lista de usuários
            return RedirectToPage("./Index");
        }
    }
}
