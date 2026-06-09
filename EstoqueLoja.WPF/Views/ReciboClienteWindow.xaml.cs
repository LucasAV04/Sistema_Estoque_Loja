using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Services;
using System.Globalization;
using System.IO;
using System.Windows;

namespace EstoqueLoja.WPF.Views
{
    public partial class ReciboClienteWindow : Window
    {
        private readonly VendaResponseDto _venda;

        public ReciboClienteWindow(VendaResponseDto venda)
        {
            InitializeComponent();
            _venda = venda;
        }

        private DadosClienteDto ColetarDados()
        {
            decimal.TryParse(
                TxtDesconto.Text.Replace(",", "."),
                NumberStyles.Any, CultureInfo.InvariantCulture,
                out var desconto);

            decimal.TryParse(
                TxtEntrada.Text.Replace(",", "."),
                NumberStyles.Any, CultureInfo.InvariantCulture,
                out var entrada);

            return new DadosClienteDto
            {
                Nome = TxtNome.Text.Trim(),
                CpfCnpj = TxtCpf.Text.Trim(),
                Telefone = TxtTelefone.Text.Trim(),
                Vendedor = TxtVendedor.Text.Trim(),
                Rua = TxtRua.Text.Trim(),
                Numero = TxtNumero.Text.Trim(),
                Bairro = TxtBairro.Text.Trim(),
                Cidade = TxtCidade.Text.Trim(),
                Estado = TxtEstado.Text.Trim(),
                Cep = TxtCep.Text.Trim(),
                PontoReferencia = TxtReferencia.Text.Trim(),
                FormaPagamento = (CmbPagamento.SelectedItem as System.Windows.Controls.ComboBoxItem)
                                       ?.Content?.ToString() ?? "À Vista",
                Desconto = desconto,
                Entrada = entrada,
                Observacoes = TxtObservacoes.Text.Trim()
            };
        }

        private void BtnSalvarPdf_Click(object sender, RoutedEventArgs e)
        {
            var cliente = ColetarDados();
            ReciboService.SalvarComDialogo(_venda, cliente);
        }

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            var cliente = ColetarDados();
            var bytes = ReciboService.Gerar(_venda, cliente);

            // Salva em temp e abre com o leitor padrão (que permite imprimir)
            var temp = Path.Combine(Path.GetTempPath(),
                $"Recibo_Venda_{_venda.Id:D6}.pdf");

            File.WriteAllBytes(temp, bytes);

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = temp,
                UseShellExecute = true
            });
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) => Close();
    }
}