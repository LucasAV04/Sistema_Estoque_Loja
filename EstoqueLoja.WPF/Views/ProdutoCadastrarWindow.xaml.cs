using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Services;
using System.Globalization;
using System.Windows;

namespace EstoqueLoja.WPF.Views
{
    public partial class ProdutoCadastrarWindow: Window
    {
        private readonly ProdutoApiService _api = new();

        public ProdutoCadastrarWindow()
        {
            InitializeComponent();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtRef.Text) ||
                string.IsNullOrWhiteSpace(TxtNome.Text) ||
                string.IsNullOrWhiteSpace(TxtTipo.Text))
            {
                MessageBox.Show("Referência, Nome e Tipo são obrigatórios.",
                    "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!decimal.TryParse(TxtValorVenda.Text.Replace(",", "."),
                    NumberStyles.Any, CultureInfo.InvariantCulture, out var valorVenda)
                || valorVenda <= 0)
            {
                MessageBox.Show("Informe um Valor de Venda válido.",
                    "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            decimal? valorCompra = null;
            if (!string.IsNullOrWhiteSpace(TxtValorCompra.Text) &&
                TxtValorCompra.Text != "0,00")
            {
                if (decimal.TryParse(TxtValorCompra.Text.Replace(",", "."),
                        NumberStyles.Any, CultureInfo.InvariantCulture, out var vc))
                    valorCompra = vc;
            }

            BtnSalvar.IsEnabled = false;
            BtnSalvar.Content = "Salvando...";

            try
            {
                var dto = new ProdutoCreateDto
                {
                    Ref = TxtRef.Text.Trim(),
                    Nome = TxtNome.Text.Trim(),
                    Tipo = TxtTipo.Text.Trim(),
                    Descricao = TxtDescricao.Text.Trim(),
                    Valor_Venda = valorVenda,
                    Valor_Compra = valorCompra
                };

                var sucesso = await _api.CriarAsync(dto);

                if (sucesso)
                {
                    MessageBox.Show(
                        $"Produto '{dto.Nome}' criado com sucesso!\n\nEstoque inicial: 0\nUse a tela Entrada para adicionar quantidade.",
                        "Produto criado", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Erro ao criar produto. Verifique se a referência já existe.",
                        "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro:\n{ex.Message}", "Erro",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                BtnSalvar.IsEnabled = true;
                BtnSalvar.Content = "Salvar Produto";
            }
        }
    }
}
