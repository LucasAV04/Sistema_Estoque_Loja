using EstoqueLoja.WPF.DTOs;
using EstoqueLoja.WPF.Services;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace EstoqueLoja.WPF.Views
{
    public partial class NotaFiscalWindow : Window
    {
        private readonly VendaResponseDto _venda;
        private readonly NotaFiscalApiService _nfApi = new();

        public NotaFiscalWindow(VendaResponseDto venda)
        {
            InitializeComponent();
            _venda = venda;
        }

        private EmitirNotaFiscalDto ColetarDto()
        {
            decimal.TryParse(TxtDesc.Text.Replace(",", "."),
                NumberStyles.Any, CultureInfo.InvariantCulture, out var desc);

            return new EmitirNotaFiscalDto
            {
                VendaId = _venda.Id,
                NomeCliente = TxtNome.Text.Trim(),
                CpfCnpjCliente = TxtCpf.Text.Trim(),
                TelefoneCliente = TxtTel.Text.Trim(),
                Vendedor = TxtVendedor.Text.Trim(),
                EnderecoCliente = $"{TxtEnd.Text.Trim()}, {TxtNum.Text.Trim()}".TrimEnd(',', ' '),
                BairroCliente = TxtBairro.Text.Trim(),
                MunicipioCliente = TxtMun.Text.Trim(),
                UfCliente = TxtUf.Text.Trim(),
                CepCliente = TxtCep.Text.Trim(),
                NaturezaOperacao = TxtNatureza.Text.Trim(),
                FormaPagamento = (CmbPag.SelectedItem as ComboBoxItem)
                                       ?.Content?.ToString() ?? "À Vista",
                Desconto = desc,
                Observacoes = TxtObs.Text.Trim()
            };
        }

        private async Task<(NotaFiscalResponseDto nf, bool ok)> EmitirNf()
        {
            try
            {
                var dto = ColetarDto();
                var nf = await _nfApi.EmitirAsync(dto);
                return (nf!, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao emitir NF:\n{ex.Message}",
                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return (null!, false);
            }
        }

        private async void BtnSalvarPdf_Click(object sender, RoutedEventArgs e)
        {
            var (nf, ok) = await EmitirNf();
            if (!ok) return;
            NotaFiscalPdfService.SalvarComDialogo(nf, _venda);
        }

        private async void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            var (nf, ok) = await EmitirNf();
            if (!ok) return;

            var bytes = NotaFiscalPdfService.Gerar(nf, _venda);
            var temp = Path.Combine(Path.GetTempPath(), $"NF_{nf.Numero}.pdf");
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