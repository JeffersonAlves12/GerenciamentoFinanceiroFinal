using Microsoft.AspNetCore.Mvc;
using FinanceiroRazorTDS.Data;
using FinanceiroRazorTDS.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace FinanceiroRazorTDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransacaoController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/transacao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransacaoModel>>> GetTransacoes()
        {
            return await _context.Transacoes.ToListAsync();
        }

        // GET api/transacao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransacaoModel>> GetTransacao(long id)
        {
                //Mostrar a categoria e o usuario que fez a transacao
                var transacao = await _context.Transacoes.FindAsync(id);

                             


            if (transacao == null)
            {
                return NotFound();
            }

            return transacao;
        }

        // POST api/transacao
        [HttpPost]
        public async Task<ActionResult<TransacaoModel>> PostTransacao([FromBody] TransacaoModel transacao)
        {
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransacao), new { id = transacao.TransacaoId }, transacao);
        }

        // PUT api/transacao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransacao(long id, [FromBody] TransacaoModel transacaoModel)
        {
            if (id != transacaoModel.TransacaoId)
            {
                return BadRequest();
            }

            var transacaoExistente = await _context.Transacoes.FindAsync(id);
            if (transacaoExistente == null)
            {
                return NotFound();
            }

            transacaoExistente.NomeTransacao = transacaoModel.NomeTransacao;
            transacaoExistente.ValorTransacao = transacaoModel.ValorTransacao;
            transacaoExistente.TipoTransacao = transacaoModel.TipoTransacao;
            transacaoExistente.DataTransacao = transacaoModel.DataTransacao;
            transacaoExistente.Descricao = transacaoModel.Descricao;
            transacaoExistente.UsuarioId = transacaoModel.UsuarioId;

            // Aqui você pode adicionar lógica para atualizar as categorias, se necessário.

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Transacoes.Any(t => t.TransacaoId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/transacao/5
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteTransacao(long id)
{
    var transacao = await _context.Transacoes.FindAsync(id);
    if (transacao == null)
    {
        return NotFound();
    }

    _context.Transacoes.Remove(transacao);
    await _context.SaveChangesAsync();

    return NoContent(); 
}
    }
}
