using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EstoqueLoja.WPF.Services
{
    public class NotaFiscalApiService
    {
        private readonly HttpClient _http;

        public NotaFiscalApiService()
        {
            _http = new HttpClient { BaseAddress = new Uri("https://localhost:7267/") };
            if (!string.IsNullOrWhiteSpace(SessaoUsuario.Token))
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SessaoUsuario.Token);
        }

        public async Task<NotaFiscalResponseDto?> EmitirAsync(EmitirNotaFiscalDto dto)
        {
            var r = await _http.PostAsJsonAsync("api/NotaFiscal/Emitir", dto);
            if (!r.IsSuccessStatusCode)
                throw new Exception(await r.Content.ReadAsStringAsync());
            return await r.Content.ReadFromJsonAsync<NotaFiscalResponseDto>();
        }

        public async Task<List<NotaFiscalResponseDto>> ListarAsync()
            => await _http.GetFromJsonAsync<List<NotaFiscalResponseDto>>(
                "api/NotaFiscal/Listar") ?? new();

        public async Task<bool> DeletarAsync(int id)
            => (await _http.DeleteAsync($"api/NotaFiscal/{id}/Deletar")).IsSuccessStatusCode;
    }
}
