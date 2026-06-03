using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EstoqueLoja.WPF.Services
{
    public class EstoqueApiService
    {
        private readonly HttpClient _httpClient;

        public EstoqueApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7267/")
            };

            if (!string.IsNullOrWhiteSpace(SessaoUsuario.Token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SessaoUsuario.Token);
            }
        }

        public async Task<List<EstoqueDetalhadoResponseDto>> ListarDetalhadoAsync()
        {
            var resultado = await _httpClient
                .GetFromJsonAsync<List<EstoqueDetalhadoResponseDto>>("api/Estoque/Detalhado");

            return resultado ?? new List<EstoqueDetalhadoResponseDto>();
        }
    }
}
