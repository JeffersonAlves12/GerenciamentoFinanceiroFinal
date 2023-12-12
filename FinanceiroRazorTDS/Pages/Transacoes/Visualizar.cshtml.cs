using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using FinanceiroRazorTDS.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace FinanceiroRazorTDS.Pages.Transacoes
{
    public class Visualizar : PageModel
    {
        private readonly string _apiBaseUrl;
        private readonly IHttpClientFactory _clientFactory;

        public TransacaoModel TransacaoModel { get; private set; }

        public Visualizar(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5000";
        }

        public async Task<IActionResult> OnGetAsync(long? id)
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

            return Page();
        }
    }
}
