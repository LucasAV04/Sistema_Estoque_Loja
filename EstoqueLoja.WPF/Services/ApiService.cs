using EstoqueLoja.WPF.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace EstoqueLoja.WPF.Services
{
    public class ApiService
    {
   
        private const string BaseUrl = "https://localhost:7267";

        private static readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl),
            Timeout = TimeSpan.FromSeconds(15)
        };


      
        public async Task<LoginResponse> LoginAsync(string usuario, string senha)
        {
            var payload = new { Usuario = usuario, Senha = senha };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("/Auth/Login", content);

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
               
                try
                {
                    var erro = JsonConvert.DeserializeAnonymousType(body, new { mensagem = "" });
                    throw new HttpRequestException(erro?.mensagem ?? "Erro desconhecido.");
                }
                catch (JsonException)
                {
                    throw new HttpRequestException($"Erro {(int)response.StatusCode}: {body}");
                }
            }

            var resultado = JsonConvert.DeserializeObject<LoginResponse>(body)
                ?? throw new HttpRequestException("Resposta inválida da API.");

            return resultado;
        }
    }
}
