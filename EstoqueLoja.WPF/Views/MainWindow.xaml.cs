using EstoqueLoja.WPF.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace EstoqueLoja.WPF.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TxtUsuarioLogado.Text = SessaoUsuario.Usuario;
            TxtRoleLogada.Text = SessaoUsuario.Role;
        }

        private void TrocarConteudo(Page pagina)
        {
            FramePrincipal.Navigate(pagina);
        }

        private void BtnProdutos_Click(object sender, RoutedEventArgs e)
        {
            TrocarConteudo(new ProdutosPage());
        }

        private void BtnEstoque_Click(object sender, RoutedEventArgs e)
        {
            TrocarConteudo(new EstoquePage());
        }

        private void BtnVenda_Click(object sender, RoutedEventArgs e)
        {

            TrocarConteudo(new VendasPage());
        }
        private void BtnSaida_Click(object sender, RoutedEventArgs e)
        {

            FramePrincipal.Navigate(new ProdutosPage());
        }
        private void BtnHistorico_Click(object sender, RoutedEventArgs e)
        {

            FramePrincipal.Navigate(new ProdutosPage());
        }

        private void BtnSair_Click(object sender, RoutedEventArgs e)
        {
            SessaoUsuario.Token = string.Empty;
            SessaoUsuario.Usuario = string.Empty;
            SessaoUsuario.Role = string.Empty;

            var login = new LoginWindow();
            login.Show();

            Close();
        }
    }
}
    