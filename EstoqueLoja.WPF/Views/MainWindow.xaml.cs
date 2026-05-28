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

            TxtUsuarioLogado.Text = $"{SessaoUsuario.Usuario} ({SessaoUsuario.Role})";
        }

        private void TrocarConteudo(string titulo, string mensagem)
        {
            TxtTituloPagina.Text = titulo;

            ConteudoPrincipal.Children.Clear();

            ConteudoPrincipal.Children.Add(new TextBlock
            {
                Text = mensagem,
                FontSize = 24,
                Foreground = System.Windows.Media.Brushes.DimGray,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            });
        }

        private void BtnProdutos_Click(object sender, RoutedEventArgs e)
        {
            TrocarConteudo("Produtos", "Tela de Produtos será carregada aqui.");
        }

        private void BtnEstoque_Click(object sender, RoutedEventArgs e)
        {
            TrocarConteudo("Estoque", "Tela de Estoque será carregada aqui.");
        }

        private void BtnEntrada_Click(object sender, RoutedEventArgs e)
        {
            TrocarConteudo("Entrada de Estoque", "Tela de Entrada será Carregada aqui");
        }
        private void BtnSaida_Click(object sender, RoutedEventArgs e)
        {
            TrocarConteudo("Saída / Venda", "Tela de saída será carregada aqui.");
        }
        private void BtnHistorico_Click(object sender, RoutedEventArgs e)
        {
            TrocarConteudo("Histórico", "Tela de histórico será carregada aqui.");
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
    