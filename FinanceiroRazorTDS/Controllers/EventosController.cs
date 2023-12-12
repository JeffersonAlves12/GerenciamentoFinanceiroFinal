using Microsoft.AspNetCore.Mvc;
using FinanceiroRazorTDS.Data;
using FinanceiroRazorTDS.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public EventosController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }
 

   [HttpGet]
    public async Task<ActionResult<IEnumerable<EventoModel>>> GetEventos()
    {
        return await _context.Eventos.ToListAsync();
    }
   [HttpPost]
public async Task<IActionResult> Post([FromForm] EventoModel eventoModel)
{
    if (eventoModel == null || !ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    try
    {
        // Lógica de processamento de arquivos
        if (Request.Form.Files.Count > 0)
        {
            var file = Request.Form.Files[0];
            var fileName = Path.GetFileName(file.FileName);
            // Constrói o caminho relativo com barras normais
            var filePath = "uploads/" + fileName;
            // Constrói o caminho absoluto
            var absoluteFilePath = Path.Combine(_environment.WebRootPath, filePath);

            using (var fileStream = new FileStream(absoluteFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Salva o caminho relativo com barras normais no banco de dados
            eventoModel.FotoPath = filePath.Replace("\\", "/");
        }

        _context.Eventos.Add(eventoModel);
        await _context.SaveChangesAsync();

        return Ok(eventoModel);
    }
    catch (Exception ex)
    {
        // Log the exception message
        // Aqui você pode logar o erro em um arquivo de log ou base de dados
        return StatusCode(500, "Internal server error: " + ex.Message);
    }
}

    // DELETE api/eventos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvento(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null)
        {
            return NotFound();
        }

        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvento(int id, [FromForm] EventoModel eventoModel)
    {
        if (id != eventoModel.Id || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var eventoExistente = await _context.Eventos.FindAsync(id);
        if (eventoExistente == null)
        {
            return NotFound();
        }

        eventoExistente.Tipo = eventoModel.Tipo;
        eventoExistente.DataConsolidacao = eventoModel.DataConsolidacao;
        eventoExistente.Valor = eventoModel.Valor;
        eventoExistente.Status = eventoModel.Status;
        eventoExistente.UsuarioId = eventoModel.UsuarioId;

        if (Request.Form.Files.Count > 0)
        {
            var file = Request.Form.Files[0];
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine("uploads", fileName);
            var absoluteFilePath = Path.Combine(_environment.WebRootPath, filePath);

            if (!string.IsNullOrEmpty(eventoExistente.FotoPath))
            {
                var existingFilePath = Path.Combine(_environment.WebRootPath, eventoExistente.FotoPath);
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);
                }
            }

            using (var fileStream = new FileStream(absoluteFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            eventoExistente.FotoPath = filePath.Replace("\\", "/");
        }

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

}
