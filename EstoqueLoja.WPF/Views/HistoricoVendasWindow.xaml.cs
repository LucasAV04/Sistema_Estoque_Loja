using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Helpers;
using EstoqueLoja.WPF.Services;
using System.Windows;
using System.Windows.Controls;
namespace EstoqueLoja.WPF.Views
{
    public partial class HistoricoVendasWindow : Window
    {
        private readonly VendaApiService _vendaApi = new();
        private List<VendaHistoricoViewModel> _historico = new();

        public HistoricoVendasWindow()
        {
            InitializeComponent();

           
            if (SessaoUsuario.IsAdmin)
                ColExcluir.Visibility = Visibility.Visible;

            Loaded += async (_, _) => await CarregarHistorico();
        }

        private async Task CarregarHistorico()
        {
            try
            {
                TxtSubtitulo.Text = "Carregando...";

                var vendas = await _vendaApi.ListarAsync();

                _historico = vendas.Select(v => new VendaHistoricoViewModel
                {
                    Id = v.Id,
                    Data = v.Data,
                    ValorTotal = v.ValorTotal,
                    TotalItens = v.Itens.Sum(i => i.Quantidade),
                    ResumoItens = string.Join(", ", v.Itens.Select(i => i.NomeProduto)),
                    Itens = v.Itens
                }).ToList();

                AplicarFiltro();
            }
            catch (Exception ex)
            {
                TxtSubtitulo.Text = "Erro ao carregar.";
                MessageBox.Show($"Erro ao carregar histórico:\n{ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AplicarFiltro()
        {
            var termo = TxtBusca?.Text?.Trim() ?? string.Empty;

            var resultado = _historico.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(termo))
            {
                resultado = resultado.Where(v =>
                    v.Id.ToString().Contains(termo) ||
                    v.ResumoItens.Contains(termo, StringComparison.OrdinalIgnoreCase));
            }

            var lista = resultado.ToList();

            GridHistorico.ItemsSource = lista;

            TxtSubtitulo.Text = $"{lista.Count} venda(s) encontrada(s)";

            TxtRodape.Text = $"Exibindo {lista.Count} de {_historico.Count} venda(s)";

            TxtTotalGeral.Text = lista.Any()
                ? $"Total filtrado: {lista.Sum(v => v.ValorTotal):C2}"
                : string.Empty;
        }

        private void TxtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private async void BtnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            await CarregarHistorico();
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnGerarRecibo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is VendaHistoricoViewModel vm)
            {
                var venda = new VendaResponseDto
                {
                    Id = vm.Id,
                    Data = vm.Data,
                    ValorTotal = vm.ValorTotal,
                    Itens = vm.Itens
                };
                var janela = new ReciboClienteWindow(venda) { Owner = this };
                janela.ShowDialog();
            }
        }

        private async void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not VendaHistoricoViewModel vm)
                return;

            var confirmar = MessageBox.Show(
                $"Tem certeza que deseja excluir a Venda Nº {vm.Id} " +
                $"no valor de {vm.ValorTotal:C2}?\n\n" +
                "Esta ação não poderá ser desfeita.",
                "Confirmar exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmar != MessageBoxResult.Yes) return;

            try
            {
                var sucesso = await _vendaApi.DeletarAsync(vm.Id);

                if (sucesso)
                {
                    MessageBox.Show("Venda excluída com sucesso.",
                        "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    await CarregarHistorico();
                }
                else
                {
                    MessageBox.Show("Erro ao excluir a venda.",
                        "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir:\n{ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class VendaHistoricoViewModel
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }
        public int TotalItens { get; set; }
        public string ResumoItens { get; set; } = string.Empty;
        public List<VendaItemResponseDto> Itens { get; set; } = new();
    }
}
