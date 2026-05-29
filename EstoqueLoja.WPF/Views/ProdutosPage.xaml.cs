using System.Windows;
using System.Windows.Controls;

namespace EstoqueLoja.WPF.Views
{
    public partial class ProdutosPage: Page
    {
        public ProdutosPage()
        {
            InitializeComponent();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Buscar produtos funcionando!");
        }
    }
}
