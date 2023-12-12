using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinanceiroRazorTDS.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using FinanceiroRazorTDS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinanceiroRazorTDS.Pages.Transacoes
{
    public class Criar : PageModel
    {
        private readonly string _apiBaseUrl;
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public TransacaoModel Transacao { get; set; }
        public List<SelectListItem> CategoriaOptions { get; set; }
        public List<SelectListItem> UsuarioOptions { get; set; }

        [BindProperty]
        public long[] SelectedCategoriaIds { get; set; }

        public Criar(AppDbContext context, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5000";
        }

        public async Task OnGetAsync()
        {
            CategoriaOptions = await _context.Categorias
                .Select(c => new SelectListItem
                {
                    Value = c.CategoriaId.ToString(),
                    Text = c.NomeCategoria
                }).ToListAsync();

            UsuarioOptions = await _context.Usuarios
                .OrderBy(u => u.PrimeiroNome)
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.PrimeiroNome} {u.UltimoNome}"
                }).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = _clientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(Transacao);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{_apiBaseUrl}/api/transacao";

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao criar transação.");
                return Page();
            }
        }
    }
}
