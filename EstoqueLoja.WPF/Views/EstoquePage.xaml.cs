using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace EstoqueLoja.WPF.Views
{
    public partial class EstoquePage : Page
    {
        private readonly EstoqueApiService _estoqueApiService = new();

        private List<EstoqueDetalhadoResponseDto> _estoque = new();

        public EstoquePage()
        {
            InitializeComponent();

            CarregarEstoque();
        }

        private async void CarregarEstoque()
        {
            try
            {
                _estoque = await _estoqueApiService.ListarDetalhadoAsync();

                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao carregar estoque:\n\n{ex.Message}");
            }
        }

        private void TxtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltros();
        }

        private void CmbTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (GridEstoque == null || CmbTipo == null)
                return;

            var resultado = _estoque.AsEnumerable();

            var busca = TxtBusca.Text?.Trim();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                resultado = resultado.Where(x =>
                    x.Nome.Contains(busca, StringComparison.OrdinalIgnoreCase)
                    || x.Ref.Contains(busca, StringComparison.OrdinalIgnoreCase));
            }

            var tipoSelecionado =
                (CmbTipo.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (!string.IsNullOrWhiteSpace(tipoSelecionado)
                && tipoSelecionado != "Todos")
            {
                resultado = resultado.Where(x =>
                    x.Tipo.Equals(tipoSelecionado,
                    StringComparison.OrdinalIgnoreCase));
            }

            var lista = resultado.ToList();

            GridEstoque.ItemsSource = lista;

            TxtTotalItens.Text =
                $"Total de itens: {lista.Sum(x => x.Quantidade)}";

            TxtValorTotalEstoque.Text =
                $"Valor total em estoque: {lista.Sum(x => x.ValorTotal):C2}";
        }

        private void BtnGerarRelatorio_Click(object sender, RoutedEventArgs e)
        {
            if (_estoque == null || !_estoque.Any())
            {
                MessageBox.Show("Nenhum dado de estoque para gerar o relatório.",
                    "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var listaAtual = (GridEstoque.ItemsSource as IEnumerable<EstoqueDetalhadoResponseDto>)
                             ?.ToList() ?? _estoque;

            RelatorioEstoqueService.SalvarComDialogo(listaAtual);
        }
    }
}