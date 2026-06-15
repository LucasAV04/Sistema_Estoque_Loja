using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace EstoqueLoja.WPF.Views
{
    public partial class EntradaPage:Page
    {
        private readonly ProdutoApiService _produtoApi = new();
        private readonly EstoqueApiService _estoqueApi = new();
        private readonly MovimentacaoApiService _movApi = new();

        private List<ProdutoResponseDto> _resultados = new();
        private ProdutoResponseDto? _produtoSelecionado;
        private List<EstoqueDetalhadoResponseDto> _estoqueCache = new();

        public EntradaPage()
        {
            InitializeComponent();
        }

        private async void TxtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            var termo = TxtBusca.Text.Trim();

            if (string.IsNullOrWhiteSpace(termo))
            {
                BorderResultados.Visibility = Visibility.Collapsed;
                return;
            }

            try
            {
                var porNome = await _produtoApi.BuscarAsync(termo, null);
                var porRef = await _produtoApi.BuscarAsync(null, termo);

                _resultados = porNome.Concat(porRef)
                    .GroupBy(p => p.Id)
                    .Select(g => g.First())
                    .OrderBy(p => p.Nome)
                    .ToList();

                ListResultados.ItemsSource = _resultados;
                BorderResultados.Visibility = _resultados.Any()
                    ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { }
        }

        private async void ListResultados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListResultados.SelectedItem is not ProdutoResponseDto produto) return;

            _produtoSelecionado = produto;
            TxtBusca.Text = produto.Nome;
            BorderResultados.Visibility = Visibility.Collapsed;
            ListResultados.SelectedItem = null;

            TxtRef.Text = produto.Ref;
            TxtNome.Text = produto.Nome;
            TxtTipo.Text = produto.Tipo;

            try
            {
                _estoqueCache = await _estoqueApi.ListarDetalhadoAsync();
                var item = _estoqueCache.FirstOrDefault(x => x.ProdutoId == produto.Id);
                TxtEstoqueAtual.Text = (item?.Quantidade ?? 0).ToString();
            }
            catch
            {
                TxtEstoqueAtual.Text = "0";
            }

            CardProduto.Visibility = Visibility.Visible;
            CardEntrada.Visibility = Visibility.Visible;
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (_produtoSelecionado == null)
            {
                MessageBox.Show("Selecione um produto.", "Atenção",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!int.TryParse(TxtQuantidade.Text, out var qtd) || qtd <= 0)
            {
                MessageBox.Show("Informe uma quantidade válida (maior que zero).",
                    "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var motivo = (CmbMotivo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Compra";

            BtnRegistrar.IsEnabled = false;
            BtnRegistrar.Content = "Registrando...";

            try
            {
                await _movApi.RegistrarAsync(new MovimentacaoEstoqueDto
                {
                    ProdutoId = _produtoSelecionado.Id,
                    Tipo = "ENTRADA",
                    Quantidade = qtd,
                    Origem = motivo,
                    Observacao = string.IsNullOrWhiteSpace(TxtObservacao.Text)
                                    ? null : TxtObservacao.Text.Trim()
                });

                var atual = int.Parse(TxtEstoqueAtual.Text);
                TxtEstoqueAtual.Text = (atual + qtd).ToString();

                MessageBox.Show(
                    $"Entrada registrada com sucesso!\n\n" +
                    $"Produto: {_produtoSelecionado.Nome}\n" +
                    $"Entrada: +{qtd}\n" +
                    $"Novo estoque: {atual + qtd}",
                    "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                TxtQuantidade.Text = "1";
                TxtObservacao.Text = string.Empty;
                CmbMotivo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar entrada:\n{ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                BtnRegistrar.IsEnabled = true;
                BtnRegistrar.Content = "✔  Registrar Entrada";
            }
        }
    }
}
