namespace EstoqueLoja.WPF.Helpers
{
    public static class SessaoUsuario
    {
        public static string Token { get; set; } = string.Empty;
        public static string Usuario { get; set; } = string.Empty;
        public static string Role { get; set; } = string.Empty;
        public static DateTime Expiracao { get; set; }

        public static bool EstaLogado => !string.IsNullOrWhiteSpace(Token);
        public static bool IsAdmin => Role == "Admin";
    }
}
