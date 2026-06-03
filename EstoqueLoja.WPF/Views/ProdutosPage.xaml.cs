using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Helpers;
using EstoqueLoja.WPF.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EstoqueLoja.WPF.Views
{
    public partial class ProdutosPage : Page
    {
        private readonly ProdutoApiService _produtoApiService = new();
        private List<ProdutoResponseDto> _produtos = new();

        public ProdutosPage()
        {
            InitializeComponent();
            CmbTipo.SelectedIndex = 0;
            CarregarProdutos();
        }

        private async void CarregarProdutos()
        {
            try
            {
                _produtos = await _produtoApiService.BuscarAsync(null, null);
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar produtos: {ex.Message}");
            }
        }

        private async void TxtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            await BuscarProdutos();
        }

        private void CmbTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltros();
        }

        private async Task BuscarProdutos()
        {
            try
            {
                var termo = TxtBusca.Text.Trim();

                if (string.IsNullOrWhiteSpace(termo))
                {
                    _produtos = await _produtoApiService.BuscarAsync(null, null);
                    AplicarFiltros();
                    return;
                }

                var produtosPorNome = await _produtoApiService.BuscarAsync(termo, null);
                var produtosPorRef = await _produtoApiService.BuscarAsync(null, termo);

                _produtos = produtosPorNome
                    .Concat(produtosPorRef)
                    .GroupBy(p => p.Id)
                    .Select(g => g.First())
                    .ToList();

                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar produtos: {ex.Message}");
            }
        }

        private void AplicarFiltros()
        {
            if (GridProdutos == null || CmbTipo == null || TxtTotalVenda == null)
                return;

            var resultado = _produtos.AsEnumerable();

            var tipoSelecionado = (CmbTipo.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (!string.IsNullOrWhiteSpace(tipoSelecionado) && tipoSelecionado != "Todos")
            {
                resultado = resultado.Where(p =>
                    p.Tipo.Equals(tipoSelecionado, StringComparison.OrdinalIgnoreCase));
            }

            var listaFinal = resultado.ToList();

            GridProdutos.ItemsSource = listaFinal;

            var totalVenda = listaFinal.Sum(p => p.Valor_Venda);

            TxtTotalVenda.Text = $"Valor total da venda unitária: {totalVenda:C2}";
        }

        private async void GridProdutos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GridProdutos.SelectedItem is not ProdutoResponseDto produtoSelecionado)
                return;

            if (!SessaoUsuario.IsAdmin)
            {
                MessageBox.Show(
                    "Somente administradores podem alterar informações dos produtos.",
                    "Acesso negado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            var janela = new ProdutoEditarWindow(produtoSelecionado)
            {
                Owner = Window.GetWindow(this)
            };

            var resultado = janela.ShowDialog();

            if (resultado == true)
            {
                var confirmar = MessageBox.Show(
                    "Deseja confirmar a alteração deste produto?",
                    "Confirmar alteração",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (confirmar != MessageBoxResult.Yes)
                    return;

                var dto = new ProdutoCreateDto
                {
                    Ref = janela.ProdutoEditado.Ref,
                    Nome = janela.ProdutoEditado.Nome,
                    Descricao = janela.ProdutoEditado.Descricao,
                    Tipo = janela.ProdutoEditado.Tipo,
                    Valor_Compra = janela.ProdutoEditado.Valor_Compra,
                    Valor_Venda = janela.ProdutoEditado.Valor_Venda
                };

                var sucesso = await _produtoApiService.AtualizarAsync(janela.ProdutoEditado.Id, dto);

                if (sucesso )
                {
                    MessageBox.Show("Produto atualizado com sucesso.");
                    await BuscarProdutos();
                }
                else
                {
                    MessageBox.Show("Erro ao atualizar produto.");
                }
            }
        }
    }
}