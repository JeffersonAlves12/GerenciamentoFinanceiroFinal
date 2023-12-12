using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceiroRazorTDS.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FinanceiroRazorTDS.Data;
using System;

namespace FinanceiroRazorTDS.Pages.Investimentos
{
    public class Index : PageModel
    {
        private readonly AppDbContext _context;

        public Index(AppDbContext context)
        {
            _context = context;
        }

        public List<InvestimentoModel> Investimentos { get; set; }
        public List<object> ValoresInvestimentosPorAcao { get; private set; }
        public List<object> ValoresInvestimentosPorTipo { get; private set; }

        public void OnGet()
        {
            Investimentos = _context.Investimentos.ToList();

            ValoresInvestimentosPorAcao = Investimentos
                .GroupBy(i => i.Nome)
                .Select(group => new 
                { 
                    Nome = group.Key, 
                    ValorTotal = group.Sum(i => i.ValorInvestido) 
                })
                .Cast<object>()
                .ToList();

            ValoresInvestimentosPorTipo = Investimentos
                .GroupBy(i => i.Tipo)
                .Select(group => new 
                { 
                    Tipo = group.Key, 
                    ValorTotal = group.Sum(i => i.ValorInvestido) 
                })
                .Cast<object>()
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync(int investimentoId, decimal? novoValorAtual, decimal? valorAporte, string acao)
        {
            var investimento = await _context.Investimentos.FindAsync(investimentoId);
            if (investimento == null)
            {
                return NotFound();
            }

            if (acao == "atualizarValorAtual" && novoValorAtual.HasValue)
            {
                investimento.ValorAtual = novoValorAtual.Value;
            }
            else if (acao == "aportar" && valorAporte.HasValue)
            {
                investimento.ValorInvestido += valorAporte.Value;
            }

            investimento.DataUltimaAtualizacao = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
          public string CalcularLucroOuPrejuizo(InvestimentoModel investimento)
    {
        decimal valorAtual = investimento.ValorAtual ?? investimento.ValorInvestido;
        decimal diferenca = valorAtual - investimento.ValorInvestido;

        string sinal = diferenca >= 0 ? "Lucro: " : "Prejuízo: ";
        return sinal + diferenca.ToString("C");
    }

        public string CalcularDiferencaValor(InvestimentoModel investimento)
        {
            decimal valorAtual = investimento.ValorAtual ?? investimento.ValorInvestido;
            var diferenca = valorAtual - investimento.ValorInvestido;
            return diferenca.ToString("C");
        }

        public string GetNomeTipoInvestimento(string tipoString)
        {
            if (int.TryParse(tipoString, out int tipoIndex) &&
                Enum.IsDefined(typeof(TipoInvestimento), tipoIndex))
            {
                return Enum.GetName(typeof(TipoInvestimento), tipoIndex);
            }

            return "Tipo Desconhecido";
        }

        public string GetValorDiferencaCor(InvestimentoModel investimento)
        {
            decimal valorAtual = investimento.ValorAtual ?? investimento.ValorInvestido;
            var diferenca = valorAtual - investimento.ValorInvestido;
            return diferenca >= 0 ? "text-success" : "text-danger";
        }

        public string FormatarValorAtual(InvestimentoModel investimento)
        {
            decimal valorAtual = investimento.ValorAtual ?? investimento.ValorInvestido;
            return valorAtual.ToString("C");
        }
    }
}
