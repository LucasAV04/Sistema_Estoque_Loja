using Application.DTOs;
using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EstoqueLoja.WPF.Services
{
    public class HistoricoApiService
    {
        private readonly HttpClient _http;

        public HistoricoApiService()
        {
            _http = new HttpClient { BaseAddress = new Uri("https://localhost:7267/") };
            if (!string.IsNullOrWhiteSpace(SessaoUsuario.Token))
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SessaoUsuario.Token);
        }

        public async Task<List<MovimentacaoEstoqueResponseDto>> ListarTodosAsync()
            => await _http.GetFromJsonAsync<List<MovimentacaoEstoqueResponseDto>>(
                "api/Movimentacoes/ListarTodos") ?? new();
    }
}