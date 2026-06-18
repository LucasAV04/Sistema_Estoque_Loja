using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EstoqueLoja.WPF.Views
{
   
    public class MovimentacaoViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int ProdutoId { get; set; }
        public string RefProduto { get; set; } = string.Empty;
        public string NomeProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public string Origem { get; set; } = string.Empty;
        public string? Observacao { get; set; }

        public string QuantidadeFormatada =>
            Tipo == "ENTRADA" ? $"+{Quantidade}" : $"-{Quantidade}";

        public Brush QuantidadeCor => Tipo switch
        {
            "ENTRADA" => new SolidColorBrush(Color.FromRgb(5, 150, 105)),   // verde
            "SAIDA" => new SolidColorBrush(Color.FromRgb(220, 38, 38)),   // vermelho
            _ => new SolidColorBrush(Color.FromRgb(29, 78, 216))    // azul (venda)
        };

        public Brush TipoBadgeBg => Tipo switch
        {
            "ENTRADA" => new SolidColorBrush(Color.FromRgb(240, 253, 244)),
            "SAIDA" => new SolidColorBrush(Color.FromRgb(254, 242, 242)),
            _ => new SolidColorBrush(Color.FromRgb(239, 246, 255))
        };

        public Brush TipoBadgeFg => Tipo switch
        {
            "ENTRADA" => new SolidColorBrush(Color.FromRgb(5, 150, 105)),
            "SAIDA" => new SolidColorBrush(Color.FromRgb(220, 38, 38)),
            _ => new SolidColorBrush(Color.FromRgb(29, 78, 216))
        };
    }

    public partial class HistoricoPage : Page
    {
        private readonly HistoricoApiService _api = new();
        private readonly EstoqueApiService _estoqueApi = new();

        private List<MovimentacaoViewModel> _todos = new();
        private List<EstoqueDetalhadoResponseDto> _estoqueCache = new();

        public HistoricoPage()
        {
            InitializeComponent();
            Loaded += async (_, _) => await Carregar();
        }

        private async Task Carregar()
        {
            try
            {
                var movs = await _api.ListarTodosAsync();
                _estoqueCache = await _estoqueApi.ListarDetalhadoAsync();

                _todos = movs.Select(m =>
                {
                    var estoque = _estoqueCache.FirstOrDefault(e => e.ProdutoId == m.ProdutoId);
                    return new MovimentacaoViewModel
                    {
                        Id = m.Id,
                        CreatedAt = m.CreatedAt,
                        Usuario = m.Usuario,
                        Tipo = m.Tipo,
                        ProdutoId = m.ProdutoId,
                        RefProduto = estoque?.Ref ?? $"ID:{m.ProdutoId}",
                        NomeProduto = estoque?.Nome ?? $"Produto {m.ProdutoId}",
                        Quantidade = m.Quantidade,
                        Origem = m.Origem,
                        Observacao = m.Observacao
                    };
                }).ToList();

                AplicarFiltros(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar histórico:\n{ex.Message}");
            }
        }

        private void AplicarFiltros(object? sender, EventArgs? e)
        {
            if (_todos == null || GridHistorico == null) return;

            var resultado = _todos.AsEnumerable();

            // Filtro tipo
            if (RbEntrada?.IsChecked == true) resultado = resultado.Where(m => m.Tipo == "ENTRADA");
            else if (RbVenda?.IsChecked == true) resultado = resultado.Where(m => m.Tipo != "ENTRADA" && m.Tipo != "SAIDA");
            else if (RbSaida?.IsChecked == true) resultado = resultado.Where(m => m.Tipo == "SAIDA");

            // Filtro período
            var agora = DateTime.Now;
            if (RbHoje?.IsChecked == true)
                resultado = resultado.Where(m => m.CreatedAt.Date == agora.Date);
            else if (Rb7dias?.IsChecked == true)
                resultado = resultado.Where(m => m.CreatedAt >= agora.AddDays(-7));
            else if (Rb30dias?.IsChecked == true)
                resultado = resultado.Where(m => m.CreatedAt >= agora.AddDays(-30));

            // Filtro busca
            var termo = TxtBusca?.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(termo))
                resultado = resultado.Where(m =>
                    m.NomeProduto.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                    m.RefProduto.Contains(termo, StringComparison.OrdinalIgnoreCase));

            var lista = resultado.ToList();

            GridHistorico.ItemsSource = lista;

            // Resumo
            var entradas = lista.Where(m => m.Tipo == "ENTRADA").ToList();
            var vendas = lista.Where(m => m.Tipo != "ENTRADA" && m.Tipo != "SAIDA").ToList();
            var saidas = lista.Where(m => m.Tipo == "SAIDA").ToList();

            TxtResEntradas.Text = entradas.Count.ToString();
            TxtResEntradasQtd.Text = $"{entradas.Sum(m => m.Quantidade)} unidades";
            TxtResVendas.Text = vendas.Count.ToString();
            TxtResVendasQtd.Text = $"{vendas.Sum(m => m.Quantidade)} unidades";
            TxtResSaidas.Text = saidas.Count.ToString();
            TxtResSaidasQtd.Text = $"{saidas.Sum(m => m.Quantidade)} unidades";
            TxtResTotal.Text = lista.Count.ToString();
        }

        private void GridHistorico_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GridHistorico.SelectedItem is not MovimentacaoViewModel vm) return;

            var tipoLabel = vm.Tipo switch
            {
                "ENTRADA" => "Entrada",
                "SAIDA" => "Saída",
                _ => "Venda"
            };

            var msg =
                $"Data:      {vm.CreatedAt:dd/MM/yyyy HH:mm}\n" +
                $"Usuário:   {vm.Usuario}\n" +
                $"Tipo:      {tipoLabel}\n" +
                $"Ref:       {vm.RefProduto}\n" +
                $"Produto:   {vm.NomeProduto}\n" +
                $"Quantidade:{vm.Quantidade}\n" +
                $"Motivo:    {vm.Origem}" +
                (string.IsNullOrWhiteSpace(vm.Observacao)
                    ? "" : $"\nObservação: {vm.Observacao}");

            MessageBox.Show(msg, "Detalhes da Movimentação",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}