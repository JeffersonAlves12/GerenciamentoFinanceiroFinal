using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FinanceiroRazorTDS.Models;

namespace FinanceiroRazorTDS.Pages.Eventos
{
    public class Index : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiBaseUrl;
        public decimal TotalPagos { get; set; }
        public decimal TotalNaoPagos { get; set; }

        public List<EventoModel> Eventos { get; set; }

        public Index(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5000";
        }

        public async Task OnGetAsync()
        {
            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{_apiBaseUrl}/api/Eventos");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Eventos = JsonConvert.DeserializeObject<List<EventoModel>>(jsonResponse);
            }
            else
            {
                // Tratar erro ou definir Eventos como uma lista vazia
                Eventos = new List<EventoModel>();
            }
                        TotalPagos = CalcularTotalPorStatus(StatusEvento.Paga);
                         TotalNaoPagos = CalcularTotalPorStatus(StatusEvento.EmAtraso) + CalcularTotalPorStatus(StatusEvento.Pendente);

        }

       
       public string GetStatusCor(StatusEvento status)
    {
        return status switch
        {
            StatusEvento.Paga => "pago",
            StatusEvento.EmAtraso => "ematraso",
            StatusEvento.Pendente => "pendente",
            _ => ""
        };
    }

       private decimal CalcularTotalPorStatus(StatusEvento status)
        {
            return Eventos.Where(e => e.Status == status).Sum(e => e.Valor);
        }
    }
}
