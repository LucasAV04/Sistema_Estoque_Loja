using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EstoqueLoja.WPF.Services
{
    public class ProdutoApiService
    {
        private readonly HttpClient _httpClient;

        public ProdutoApiService()
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
        public async Task<bool> AtualizarAsync(int id, ProdutoCreateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Produtos/{id}/Atualizar", dto);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<ProdutoResponseDto>> BuscarAsync(string? nome, string? refProduto)
        {
            var url = "api/Produtos/Buscar";

            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(nome))
                query.Add($"nome={Uri.EscapeDataString(nome)}");

            if (!string.IsNullOrWhiteSpace(refProduto))
                query.Add($"Ref={Uri.EscapeDataString(refProduto)}");

            if (query.Count > 0)
                url += "?" + string.Join("&", query);

            var produtos = await _httpClient.GetFromJsonAsync<List<ProdutoResponseDto>>(url);

            return produtos ?? new List<ProdutoResponseDto>();
        }

        public async Task<bool> CriarAsync(ProdutoCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Produtos/Adicionar", dto);
            return response.IsSuccessStatusCode;
        }
    }
}
