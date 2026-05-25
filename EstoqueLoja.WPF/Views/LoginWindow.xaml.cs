
using System.Windows;

namespace EstoqueLoja.WPF.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Botão Funcionando");
        }
    }
}
