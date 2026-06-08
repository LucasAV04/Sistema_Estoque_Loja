using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace EstoqueLoja.WPF.Services
{
    public class VendaApiService
    {
        private readonly HttpClient _httpClient;

        public VendaApiService()
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

        public async Task<VendaResponseDto?> FinalizarAsync(List<VendaItemDto> itens)
        {
            var payload = new { Itens = itens };
            var response = await _httpClient.PostAsJsonAsync("api/Vendas/Finalizar", payload);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                throw new Exception(erro);
            }

            return await response.Content.ReadFromJsonAsync<VendaResponseDto>();
        }

        public async Task<List<VendaResponseDto>> ListarAsync()
        {
            var resultado = await _httpClient
                .GetFromJsonAsync<List<VendaResponseDto>>("api/Vendas/Listar");
            return resultado ?? new();
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Vendas/{id}/Deletar");
            return response.IsSuccessStatusCode;
        }
    }
}
