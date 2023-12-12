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


namespace FinanceiroRazorTDS.Pages.Eventos
{
    public class Criar : PageModel
    {
        private readonly string _apiBaseUrl;
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public EventoModel Evento { get; set; }
        public List<SelectListItem> UsuariosSelectList { get; set; }

        public Criar(AppDbContext context, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5000";
        }

        public async Task OnGetAsync()
        {
            UsuariosSelectList = await _context.Usuarios
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
        var formData = new MultipartFormDataContent();
        
        formData.Add(new StringContent(Evento.Tipo.ToString()), "Tipo");
        formData.Add(new StringContent(Evento.DataConsolidacao.ToString("o")), "DataConsolidacao");
        formData.Add(new StringContent(Evento.Valor.ToString()), "Valor");
        formData.Add(new StringContent(Evento.Status.ToString()), "Status");
        formData.Add(new StringContent(Evento.UsuarioId.ToString()), "UsuarioId");

        if (Request.Form.Files.Count > 0)
        {
            var file = Request.Form.Files[0];
            var streamContent = new StreamContent(file.OpenReadStream());
            formData.Add(streamContent, "FotoPath", file.FileName);
        }

        var url = $"{_apiBaseUrl}/api/Eventos";
        var response = await httpClient.PostAsync(url, formData);

           if (response.IsSuccessStatusCode)
    {
        return RedirectToPage("Eventos/Index");
    }
    else
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, "Erro ao criar evento: " + responseContent);
        return Page();
    }
    }
}
}
