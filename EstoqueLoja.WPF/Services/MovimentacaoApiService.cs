using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EstoqueLoja.WPF.Services
{
    public class MovimentacaoApiService
    {
        private readonly HttpClient _http;

        public MovimentacaoApiService()
        {
            _http = new HttpClient { BaseAddress = new Uri("https://localhost:7267/") };
            if (!string.IsNullOrWhiteSpace(SessaoUsuario.Token))
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SessaoUsuario.Token);
        }

        public async Task<bool> RegistrarAsync(MovimentacaoEstoqueDto dto)
        {
            var r = await _http.PostAsJsonAsync("api/Movimentacoes/Registrar", dto);
            if (!r.IsSuccessStatusCode)
                throw new Exception(await r.Content.ReadAsStringAsync());
            return true;
        }
    }
}
