namespace EstoqueLoja.WPF.Models
{
    public class SessaoUsuario
    {
        public static string Token { get; set; } = string.Empty;
        public static string Usuario { get; set; } = string.Empty;
        public static string Role { get; set; } = string.Empty;
        public static DateTime Expira { get; set; }

        public static bool IsAdmin => Role == "Admin";

        public static void Limpar()
        {
            Token = string.Empty;
            Usuario = string.Empty;
            Role = string.Empty;
            Expira = DateTime.MinValue;
        }
    }
}
