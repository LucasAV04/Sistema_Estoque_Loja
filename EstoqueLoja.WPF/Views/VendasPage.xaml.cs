using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Services;
using EstoqueLoja.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EstoqueLoja.WPF.Views
{
    public partial class VendasPage:Page
    {
        private readonly ProdutoApiService _produtoApi = new();
        private readonly VendaApiService _vendaApi = new();

        private List<ProdutoResponseDto> _resultadosBusca = new();
        private List<VendaItemDto> _carrinho = new();
        private List<VendaHistoricoViewModel> _historico = new();

        public VendasPage()
        {
            InitializeComponent();
            CarregarHistorico();
        }

        // ── PESQUISA DE PRODUTO ──────────────────────────────────────

        private async void TxtBuscaProduto_TextChanged(object sender, TextChangedEventArgs e)
        {
            var termo = TxtBuscaProduto.Text.Trim();

            if (string.IsNullOrWhiteSpace(termo))
            {
                ListResultados.Visibility = Visibility.Collapsed;
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
                    .ToList();

                ListResultados.ItemsSource = _resultadosBusca;
                ListResultados.Visibility = _resultadosBusca.Any()
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            catch { }
        }

        private void TxtBuscaProduto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AdicionarAoCarrinho();
        }

        private void ListResultados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListResultados.SelectedItem is ProdutoResponseDto produto)
            {
                TxtBuscaProduto.Text = produto.Nome;
                ListResultados.Visibility = Visibility.Collapsed;
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
                MessageBox.Show("Selecione um produto da lista de resultados.");
                return;
            }

            // Se já existe no carrinho, incrementa
            var existente = _carrinho.FirstOrDefault(i => i.ProdutoId == produto.Id);
            if (existente != null)
            {
                existente.Quantidade++;
            }
            else
            {
                _carrinho.Add(new VendaItemDto
                {
                    ProdutoId = produto.Id,
                    NomeProduto = produto.Nome,
                    RefProduto = produto.Ref,
                    Quantidade = 1,
                    ValorUnitario = produto.Valor_Venda
                });
            }

            AtualizarCarrinho();
            TxtBuscaProduto.Clear();
            ListResultados.Visibility = Visibility.Collapsed;
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
            GridCarrinho.ItemsSource = null;
            GridCarrinho.ItemsSource = _carrinho;

            var total = _carrinho.Sum(i => i.ValorTotal);
            TxtTotalCarrinho.Text = total.ToString("C2");
        }

        // ── FINALIZAR VENDA ──────────────────────────────────────────

        private async void BtnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            if (!_carrinho.Any())
            {
                MessageBox.Show("Adicione ao menos um produto ao carrinho.");
                return;
            }

            var confirmar = MessageBox.Show(
                $"Confirmar venda de {_carrinho.Count} item(ns) totalizando {_carrinho.Sum(i => i.ValorTotal):C2}?",
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
                    MessageBox.Show("Erro ao finalizar venda.");
                    return;
                }

                _carrinho.Clear();
                AtualizarCarrinho();
                await CarregarHistorico();

                var gerarRecibo = MessageBox.Show(
                    $"Venda Nº {venda.Id} finalizada com sucesso!\n\nDeseja gerar o recibo em PDF?",
                    "Venda realizada",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                if (gerarRecibo == MessageBoxResult.Yes)
                    ReciboService.SalvarComDialogo(venda);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao finalizar venda:\n{ex.Message}");
            }
            finally
            {
                BtnFinalizar.IsEnabled = true;
                BtnFinalizar.Content = "Finalizar Venda";
            }
        }

        // ── HISTÓRICO ────────────────────────────────────────────────

        private async Task CarregarHistorico()
        {
            try
            {
                var vendas = await _vendaApi.ListarAsync();

                _historico = vendas.Select(v => new VendaHistoricoViewModel
                {
                    Id = v.Id,
                    Data = v.Data,
                    ValorTotal = v.ValorTotal,
                    TotalItens = v.Itens.Sum(i => i.Quantidade),
                    Itens = v.Itens
                }).ToList();

                AplicarFiltroHistorico();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar histórico:\n{ex.Message}");
            }
        }

        private async void BtnAtualizarHistorico_Click(object sender, RoutedEventArgs e)
        {
            await CarregarHistorico();
        }

        private void TxtBuscaHistorico_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltroHistorico();
        }

        private void AplicarFiltroHistorico()
        {
            if (GridHistorico == null) return;

            var termo = TxtBuscaHistorico?.Text?.Trim() ?? string.Empty;

            var resultado = _historico.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(termo))
            {
                resultado = resultado.Where(v =>
                    v.Id.ToString().Contains(termo) ||
                    v.Itens.Any(i => i.NomeProduto
                        .Contains(termo, StringComparison.OrdinalIgnoreCase)));
            }

            GridHistorico.ItemsSource = resultado.ToList();
        }

        private void GridHistorico_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GridHistorico.SelectedItem is not VendaHistoricoViewModel vm) return;

            var venda = new VendaResponseDto
            {
                Id = vm.Id,
                Data = vm.Data,
                ValorTotal = vm.ValorTotal,
                Itens = vm.Itens
            };

            ReciboService.SalvarComDialogo(venda);
        }
    }

}
