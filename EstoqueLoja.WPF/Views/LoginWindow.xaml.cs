
using EstoqueLoja.WPF.Helpers;
using EstoqueLoja.WPF.Services;
using System.Windows;

namespace EstoqueLoja.WPF.Views
{
    public partial class LoginWindow : Window
    {
        private bool isAdmin = false;
        private readonly AuthApiService _authApiService = new();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void TabUsuario_Checked(object sender, RoutedEventArgs e)
        {
            isAdmin = false;
            if (AdminWarning == null)
                return;
            AdminWarning.Visibility = Visibility.Collapsed;
            CardTitle.Text = "Bem Vindo!";
            CardSubtitle.Text = "Faça Login para Continuar";
            LabelUsuario.Text = "Usuário";
            BtnEntrar.Content = "Entrar";
            TxtRodapeInfo.Text = "Usuário: visualizar estoque e registrar movimentações.";
            TxtErro.Visibility = Visibility.Collapsed;
        }

        private void TabAdmin_Checked(object sender, RoutedEventArgs e)
        {
            isAdmin = true;

            if (AdminWarning == null)
                return;

            AdminWarning.Visibility = Visibility.Visible;
            CardTitle.Text = "Acesso Administrativo";
            CardSubtitle.Text = "Entre com sua Conta de Administrador";
            LabelUsuario.Text = "Administrador";
            BtnEntrar.Content = "Entrar como Administrador";
            TxtRodapeInfo.Text = "Admin: Gerencia todo o sistema.";
            TxtErro.Visibility = Visibility.Collapsed;

        }
        private async void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            var usuario = TxtUsuario.Text.Trim();
            var senha = TxtSenha.Password;

            if(string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha))
            {
                MostrarErro("Preencha usuário e senha.");
                return;
            }
            BtnEntrar.IsEnabled = false;
            BtnEntrar.Content = "Entrando...";
            TxtErro.Visibility = Visibility.Collapsed;

            try
            {
                var resultado = await _authApiService.LoginAsync(usuario, senha);

                if (resultado == null)
                {
                    MostrarErro("Usuário ou senha Inválidos");
                    return;
                }

                if (isAdmin && resultado.Role != "Admin")
                {
                    MostrarErro("Este usuário não possui permissão de administrador.");
                    return;
                }

                SessaoUsuario.Token = resultado.Token;
                SessaoUsuario.Usuario = resultado.Usuario;
                SessaoUsuario.Role = resultado.Role;
                SessaoUsuario.Expiracao = resultado.Expiracao;

                var main = new MainWindow();
                main.Show();

                Close();
            }
            catch(Exception ex) 
            {
                MostrarErro($"Erro ao conectar com a API: {ex.Message}");
            }

            finally
            {
                BtnEntrar.IsEnabled = true;
                BtnEntrar.Content = isAdmin ? "Entrar como administrador" : "Entrar";
            }
        }

        private void MostrarErro(string message)
        {
            TxtErro.Text = message;
            TxtErro.Visibility = Visibility.Visible;
        }

       
    }
}
