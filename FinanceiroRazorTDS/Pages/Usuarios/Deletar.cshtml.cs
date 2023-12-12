using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FinanceiroRazorTDS.Data;
using FinanceiroRazorTDS.Models;

namespace FinanceiroRazorTDS.Pages.Usuarios
{
    public class Deletar : PageModel
    {
        private readonly AppDbContext _context;
        [BindProperty]
        public UsuarioModel UsuarioModel { get; set; }
        public String ErrorMessage { get; set; }

        public Deletar(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = String.Format("A exclusão do usuário com ID {0} falhou. Tente novamente", id);
            }

            UsuarioModel = usuario;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var usuarioToRemove = await _context.Usuarios.FindAsync(id);

            if (usuarioToRemove == null)
            {
                return NotFound();
            }

            try
            {
                _context.Usuarios.Remove(usuarioToRemove);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Usuarios/Index");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("./Deletar", new { id, saveChangesError = true });
            }
        }
    }
}
