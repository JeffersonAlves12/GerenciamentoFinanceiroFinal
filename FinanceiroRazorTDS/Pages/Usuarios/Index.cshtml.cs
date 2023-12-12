using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceiroRazorTDS.Models;
using FinanceiroRazorTDS.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceiroRazorTDS.Pages.Usuarios
{
    public class Index : PageModel
    {
        private readonly AppDbContext _context;

        public Index(AppDbContext context)
        {
            _context = context;
        }

        public IList<UsuarioModel> Usuarios { get;set; }

        public async Task OnGetAsync()
        {
            Usuarios = await _context.Usuarios.AsNoTracking().ToListAsync();
        }
    }
}
