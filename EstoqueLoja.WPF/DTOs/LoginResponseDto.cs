namespace EstoqueLoja.WPF.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracao { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
