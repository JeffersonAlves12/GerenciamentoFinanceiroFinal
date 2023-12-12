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
    public class Deletar : PageModel
    {
        private readonly string _apiBaseUrl;
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public TransacaoModel TransacaoModel { get; set; }
        public string ErrorMessage { get; set; }

        public Deletar(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5000";
        }

        public async Task<IActionResult> OnGetAsync(long? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{_apiBaseUrl}/api/transacao/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            TransacaoModel = JsonConvert.DeserializeObject<TransacaoModel>(responseContent);

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Falha ao deletar. Tente Novamente";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.DeleteAsync($"{_apiBaseUrl}/api/transacao/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Transacoes/Index");
            }
            else
            {
                // Log the error or handle it as per your requirement
                return RedirectToAction("./Deletar", new { id, saveChangesError = true });
            }
        }
    }
}
