using Microsoft.AspNetCore.Mvc;
using FinanceiroRazorTDS.Data;
using FinanceiroRazorTDS.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FinanceiroRazorTDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioModel>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }
           [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        [HttpGet("{id}")]
public async Task<ActionResult<UsuarioModel>> GetUsuario(int id)
{
    var usuario = await _context.Usuarios.FindAsync(id);
    if (usuario == null)
    {
        return NotFound();
    }

    return usuario;
}


        // Outros métodos, como POST, PUT, DELETE, etc.
    }
}
