using FinanceiroRazorTDS.Data;
using FinanceiroRazorTDS.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceiroRazorTDS.Pages.Calendarios
{
    public class Index : PageModel
    {
        private readonly AppDbContext _context;

        public Index(AppDbContext context)
        {
            _context = context;
        }

        public List<Week> Weeks { get; set; } = new List<Week>();
        public string CurrentMonthYear { get; set; }
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }
        public string NextMonth { get; set; }
        public string PreviousMonth { get; set; }

        public class Day
        {
            public DateTime Date { get; set; }
            public bool IsCurrentMonth { get; set; }
            public List<TransacaoModel> Transacoes { get; set; } = new List<TransacaoModel>();
            public List<InvestimentoModel> Investimentos { get; set; } = new List<InvestimentoModel>(); // Nova propriedade

        }

        public class Week
        {
            public List<Day> Days { get; set; } = new List<Day>();
        }

        public async Task OnGetAsync(int? month)
        {
            DateTime now = DateTime.Now;
            if (month.HasValue)
            {
                now = new DateTime(now.Year, month.Value, 1);
            }
            CurrentMonth = now.Month;
            CurrentYear = now.Year;
            CurrentMonthYear = now.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
            NextMonth = (new DateTime(CurrentYear, CurrentMonth, 1).AddMonths(1).Month).ToString();
            PreviousMonth = (new DateTime(CurrentYear, CurrentMonth, 1).AddMonths(-1).Month).ToString();

            var firstDayOfMonth = new DateTime(CurrentYear, CurrentMonth, 1);
            var daysToSubtract = (int)firstDayOfMonth.DayOfWeek;
            var startDate = firstDayOfMonth.AddDays(-daysToSubtract);

            // Carregar transações do mês atual
            var transacoesDoMes = await _context.Transacoes
                .Where(t => t.DataTransacao.Year == CurrentYear && t.DataTransacao.Month == CurrentMonth)
                .ToListAsync();

                  var investimentosDoMes = await _context.Investimentos
        .Where(i => i.DataDeCompra.Year == CurrentYear && i.DataDeCompra.Month == CurrentMonth)
        .ToListAsync();

            for (int week = 0; week < 6; week++) // 6 weeks to cover all days and empty cells
            {
                var weekDays = new Week();
                for (int day = 0; day < 7; day++) // 7 days in a week
                {
                    var currentDay = startDate.AddDays(week * 7 + day);
                    var dayModel = new Day
                    {
                        Date = currentDay,
                        IsCurrentMonth = currentDay.Month == CurrentMonth,
                        Transacoes = transacoesDoMes.Where(t => t.DataTransacao.Date == currentDay.Date).ToList(),
                                        Investimentos = investimentosDoMes.Where(i => i.DataDeCompra.Date == currentDay.Date).ToList() // Adicionando investimentos

                    };
                    weekDays.Days.Add(dayModel);
                }
                Weeks.Add(weekDays);
                if (weekDays.Days[6].Date.Month != CurrentMonth && week > 3) // Break if last day of week is not in current month and month had more than 4 weeks
                {
                    break;
                }
            }
        }
    }
}
