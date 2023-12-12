using FinanceiroRazorTDS.Data;
using FinanceiroRazorTDS.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceiroRazorTDS.Pages
{
   public class Index : PageModel
{
    private readonly AppDbContext _context;

    public decimal LucroOuPrejuizoTotal { get; private set; }

    public Index(AppDbContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        var transacoes = await _context.Transacoes.ToListAsync();
        var investimentos = await _context.Investimentos.ToListAsync();

        decimal totalTransacoes = transacoes.Sum(t => t.TipoTransacao == TipoTransacao.Entrada ? t.ValorTransacao : -t.ValorTransacao);
        decimal totalInvestimentos = investimentos.Sum(i => (i.ValorAtual ?? i.ValorInvestido) - i.ValorInvestido);

        LucroOuPrejuizoTotal = totalTransacoes + totalInvestimentos;
    }

    public string FormatarLucroPrejuizo(decimal valor)
    {
        string sinal = valor >= 0 ? "+" : "";
        string classeCss = valor >= 0 ? "valor-positivo" : "valor-negativo";

        return $"<span class='{classeCss}'>{sinal}{valor.ToString("C")}</span>";
    }
}

}
