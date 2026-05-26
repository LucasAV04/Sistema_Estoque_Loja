using System.Net.Http;
using System.Net.Http.Json;
using EstoqueLoja.WPF.DTOs;

namespace EstoqueLoja.WPF.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7267/")
            };
        }

        public async Task<LoginResponseDto?> LoginAsync(string usuario, string senha)
        {
            var request = new LoginRequestDto
            {
                Usuario = usuario,
                Senha = senha
            };
            var response = await _httpClient.PostAsJsonAsync("/Auth/Login", request);
            
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        }
    }

}
