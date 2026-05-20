using System.Windows;
using System.Windows.Controls;

namespace EstoqueLoja.WPF
{
    public partial class LoginWindow : Window
    {
        private bool _isAdmin = false;

        public LoginWindow()
        {
            InitializeComponent();
        }

      
        private void TabUsuario_Checked(object sender, RoutedEventArgs e)
        {
            _isAdmin = false;

            if (CardTitle == null) return;

            CardTitle.Text = "Bem-vindo!";
            CardSubtitle.Text = "Faça login para continuar";
            LabelUsuario.Text = "Usuário / matrícula";
            BtnEntrar.Content = "Entrar";
            AdminWarning.Visibility = Visibility.Collapsed;
            TxtRodapeInfo.Text = "Usuário: visualizar estoque e registrar movimentações.";
            TxtErro.Visibility = Visibility.Collapsed;
        }

        private void TabAdmin_Checked(object sender, RoutedEventArgs e)
        {
            _isAdmin = true;

            if (CardTitle == null) return;

            CardTitle.Text = "Acesso Restrito";
            CardSubtitle.Text = "Somente administradores autorizados";
            LabelUsuario.Text = "E-mail de administrador";
            BtnEntrar.Content = "Entrar como administrador";
            AdminWarning.Visibility = Visibility.Visible;
            TxtRodapeInfo.Text = "Admin: gerenciar produtos, usuários e configurações.";
            TxtErro.Visibility = Visibility.Collapsed;
        }

  
        private async void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            var usuario = TxtUsuario.Text.Trim();
            var senha = TxtSenha.Password;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha))
            {
                MostrarErro("Preencha todos os campos.");
                return;
            }

            BtnEntrar.IsEnabled = false;
            BtnEntrar.Content = "Aguarde...";
            TxtErro.Visibility = Visibility.Collapsed;

            try
            {
                
                await Task.Delay(600); 

                bool sucesso = _isAdmin
                    ? usuario == "Lucas" && senha == "3214"
                    : usuario == "Usuario" && senha == "1234";

                if (sucesso)
                {
                    var main = new MainWindow();
                    main.Show();
                    Close();
                }
                else
                {
                    MostrarErro("Usuário ou senha inválidos.");
                }
            }
            catch (Exception ex)
            {
                MostrarErro($"Erro ao conectar: {ex.Message}");
            }
            finally
            {
                BtnEntrar.IsEnabled = true;
                BtnEntrar.Content = _isAdmin ? "Entrar como administrador" : "Entrar";
            }
        }

        private void MostrarErro(string mensagem)
        {
            TxtErro.Text = mensagem;
            TxtErro.Visibility = Visibility.Visible;
        }
    }
}