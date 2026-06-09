using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EstoqueLoja.WPF.Views
{
    public partial class VendasPage : Page
    {
        private readonly ProdutoApiService _produtoApi = new();
        private readonly VendaApiService _vendaApi = new();

        private List<ProdutoResponseDto> _resultadosBusca = new();
        private List<VendaItemDto> _carrinho = new();

        public VendasPage()
        {
            InitializeComponent();
            AtualizarCarrinho();
        }

        

        private void BtnHistorico_Click(object sender, RoutedEventArgs e)
        {
            var janela = new HistoricoVendasWindow
            {
                Owner = Window.GetWindow(this)
            };
            janela.ShowDialog();
        }

      

        private async void TxtBuscaProduto_TextChanged(object sender, TextChangedEventArgs e)
        {
            var termo = TxtBuscaProduto.Text.Trim();

            if (string.IsNullOrWhiteSpace(termo))
            {
                BorderResultados.Visibility = Visibility.Collapsed;
                _resultadosBusca.Clear();
                return;
            }

            try
            {
                var porNome = await _produtoApi.BuscarAsync(termo, null);
                var porRef = await _produtoApi.BuscarAsync(null, termo);

                _resultadosBusca = porNome
                    .Concat(porRef)
                    .GroupBy(p => p.Id)
                    .Select(g => g.First())
                    .OrderBy(p => p.Nome)
                    .ToList();

                ListResultados.ItemsSource = _resultadosBusca;

                BorderResultados.Visibility = _resultadosBusca.Any()
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            catch { }
        }

        private void TxtBuscaProduto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AdicionarAoCarrinho();
            else if (e.Key == Key.Down && ListResultados.Items.Count > 0)
                ListResultados.Focus();
        }

        private void ListResultados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListResultados.SelectedItem is ProdutoResponseDto produto)
            {
                TxtBuscaProduto.Text = produto.Nome;
                BorderResultados.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            AdicionarAoCarrinho();
        }

        private void AdicionarAoCarrinho()
        {
            ProdutoResponseDto? produto = null;

            if (ListResultados.SelectedItem is ProdutoResponseDto selecionado)
                produto = selecionado;
            else if (_resultadosBusca.Count == 1)
                produto = _resultadosBusca[0];

            if (produto == null)
            {
                MessageBox.Show(
                    "Selecione um produto da lista de resultados.",
                    "Produto não selecionado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var existente = _carrinho.FirstOrDefault(i => i.ProdutoId == produto.Id);
            if (existente != null)
                existente.Quantidade++;
            else
                _carrinho.Add(new VendaItemDto
                {
                    ProdutoId = produto.Id,
                    NomeProduto = produto.Nome,
                    RefProduto = produto.Ref,
                    Quantidade = 1,
                    ValorUnitario = produto.Valor_Venda
                });

            AtualizarCarrinho();
            TxtBuscaProduto.Clear();
            BorderResultados.Visibility = Visibility.Collapsed;
            ListResultados.SelectedItem = null;
            _resultadosBusca.Clear();
        }

        private void BtnRemoverItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is VendaItemDto item)
            {
                _carrinho.Remove(item);
                AtualizarCarrinho();
            }
        }

        private void AtualizarCarrinho()
        {
            var temItens = _carrinho.Any();

            BorderCarrinhoVazio.Visibility = temItens
                ? Visibility.Collapsed
                : Visibility.Visible;

            GridCarrinho.Visibility = temItens
                ? Visibility.Visible
                : Visibility.Collapsed;

            GridCarrinho.ItemsSource = null;
            GridCarrinho.ItemsSource = _carrinho;

            var total = _carrinho.Sum(i => i.ValorTotal);
            TxtTotalCarrinho.Text = total.ToString("C2");
            TxtQtdItensCarrinho.Text = $"{_carrinho.Sum(i => i.Quantidade)} item(ns)";

            BtnFinalizar.IsEnabled = temItens;
        }

       

        private async void BtnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            if (!_carrinho.Any())
                return;

            var total = _carrinho.Sum(i => i.ValorTotal);
            var totalItens = _carrinho.Sum(i => i.Quantidade);

            var confirmar = MessageBox.Show(
                $"Confirmar venda de {totalItens} item(ns) totalizando {total:C2}?",
                "Confirmar venda",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmar != MessageBoxResult.Yes) return;

            BtnFinalizar.IsEnabled = false;
            BtnFinalizar.Content = "Processando...";

            try
            {
                var venda = await _vendaApi.FinalizarAsync(_carrinho);

                if (venda == null)
                {
                    MessageBox.Show("Erro ao finalizar venda.",
                        "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _carrinho.Clear();
                AtualizarCarrinho();

                var gerarRecibo = MessageBox.Show(
                    $"Venda Nº {venda.Id} finalizada com sucesso!\n\nDeseja gerar o recibo em PDF?",
                    "Venda realizada",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                if (gerarRecibo == MessageBoxResult.Yes)
                {
                    var janela = new ReciboClienteWindow(venda)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    janela.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao finalizar venda:\n{ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                BtnFinalizar.IsEnabled = _carrinho.Any();
                BtnFinalizar.Content = "✔  Finalizar Venda";
            }
        }
    }
}