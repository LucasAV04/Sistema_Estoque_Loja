using EstoqueLoja.WPF.DTOs;
using System.Globalization;
using System.Windows;

namespace EstoqueLoja.WPF.Views
{
    public partial class ProdutoEditarWindow:Window
    {
        public ProdutoResponseDto ProdutoEditado { get; private set; }

        public ProdutoEditarWindow(ProdutoResponseDto produto)
        {
            InitializeComponent();

            ProdutoEditado = produto;

            TxtRef.Text = produto.Ref;
            TxtNome.Text = produto.Nome;
            TxtDescricao.Text = produto.Descricao;
            TxtTipo.Text = produto.Tipo;
            TxtValorCompra.Text = produto.Valor_Compra?.ToString("F2");
            TxtValorVenda.Text = produto.Valor_Venda.ToString("F2");
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtRef.Text) ||
                string.IsNullOrWhiteSpace(TxtNome.Text) ||
                string.IsNullOrWhiteSpace(TxtTipo.Text))
            {
                MessageBox.Show("Ref, Nome e Tipo são obrigatórios.");
                return;
            }

            if (!decimal.TryParse(TxtValorVenda.Text.Replace(",", "."),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var valorVenda))
            {
                MessageBox.Show("Valor de venda inválido.");
                return;
            }

            decimal? valorCompra = null;

            if (!string.IsNullOrWhiteSpace(TxtValorCompra.Text))
            {
                if (!decimal.TryParse(TxtValorCompra.Text.Replace(",", "."),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out var compra))
                {
                    MessageBox.Show("Valor de compra inválido.");
                    return;
                }

                valorCompra = compra;
            }

            ProdutoEditado.Ref = TxtRef.Text.Trim();
            ProdutoEditado.Nome = TxtNome.Text.Trim();
            ProdutoEditado.Descricao = TxtDescricao.Text.Trim();
            ProdutoEditado.Tipo = TxtTipo.Text.Trim();
            ProdutoEditado.Valor_Compra = valorCompra;
            ProdutoEditado.Valor_Venda = valorVenda;

            DialogResult = true;
            Close();
        }
    }
}
