using EstoqueLoja.WPF.DTOs;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.IO;


namespace EstoqueLoja.WPF.Services
{
    public class ReciboService
    {
        public static byte[] Gerar(VendaResponseDto venda)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Content().Column(col =>
                    {
                        // Cabeçalho
                        col.Item().AlignCenter().Text("LOJAS DIAMANTE")
                            .FontSize(18).Bold();

                        col.Item().AlignCenter().Text("Sistema de Controle de Estoque")
                            .FontSize(10).FontColor(Colors.Grey.Medium);

                        col.Item().PaddingVertical(6).LineHorizontal(0.5f)
                            .LineColor(Colors.Grey.Medium);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Venda Nº {venda.Id}").Bold();
                            row.RelativeItem().AlignRight()
                                .Text(venda.Data.ToString("dd/MM/yyyy HH:mm"));
                        });

                        col.Item().PaddingVertical(6).LineHorizontal(0.5f)
                            .LineColor(Colors.Grey.Medium);

                        // Cabeçalho da tabela
                        col.Item().Row(row =>
                        {
                            row.RelativeItem(4).Text("Produto").Bold();
                            row.RelativeItem(1).AlignCenter().Text("Qtd").Bold();
                            row.RelativeItem(2).AlignRight().Text("Unit.").Bold();
                            row.RelativeItem(2).AlignRight().Text("Total").Bold();
                        });

                        col.Item().LineHorizontal(0.3f).LineColor(Colors.Grey.Lighten2);

                        // Itens
                        foreach (var item in venda.Itens)
                        {
                            col.Item().PaddingVertical(3).Row(row =>
                            {
                                row.RelativeItem(4).Text(item.NomeProduto);
                                row.RelativeItem(1).AlignCenter()
                                    .Text(item.Quantidade.ToString());
                                row.RelativeItem(2).AlignRight()
                                    .Text(item.ValorUnitario.ToString("C2"));
                                row.RelativeItem(2).AlignRight()
                                    .Text(item.ValorTotal.ToString("C2"));
                            });

                            col.Item().LineHorizontal(0.3f)
                                .LineColor(Colors.Grey.Lighten3);
                        }

                        col.Item().PaddingVertical(4).LineHorizontal(0.5f)
                            .LineColor(Colors.Grey.Medium);

                        // Total
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("TOTAL GERAL").Bold().FontSize(13);
                            row.RelativeItem().AlignRight()
                                .Text(venda.ValorTotal.ToString("C2")).Bold().FontSize(13);
                        });

                        col.Item().PaddingTop(20).AlignCenter()
                            .Text("Obrigado pela preferência!")
                            .FontColor(Colors.Grey.Medium).Italic();
                    });
                });
            }).GeneratePdf();
        }

        public static void SalvarComDialogo(VendaResponseDto venda)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"Recibo_Venda_{venda.Id}",
                DefaultExt = ".pdf",
                Filter = "PDF|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                var bytes = Gerar(venda);
                File.WriteAllBytes(dialog.FileName, bytes);

                // Abre o PDF automaticamente após salvar
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = dialog.FileName,
                    UseShellExecute = true
                });
            }
        }
    }
}
