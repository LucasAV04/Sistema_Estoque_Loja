using EstoqueLoja.WPF.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace EstoqueLoja.WPF.Services
{
    public static class RelatorioEstoqueService
    {
        public static byte[] Gerar(List<EstoqueDetalhadoResponseDto> itens)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var totalItens = itens.Sum(i => i.Quantidade);
            var totalValor = itens.Sum(i => i.ValorTotal);
            var geradoEm = DateTime.Now;

            return Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginHorizontal(32);
                    page.MarginVertical(28);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    
                    page.Header().Column(col =>
                    {
                        col.Item().Background("#1B2A4A").Padding(14).Row(r =>
                        {
                            r.RelativeItem().Column(c =>
                            {
                                c.Item().Text("LOJA DIAMANTE")
                                    .FontSize(20).Bold().FontColor(Colors.White);
                                c.Item().Text("Relatório de Estoque")
                                    .FontSize(11).FontColor("#AED6F1");
                            });
                            r.ConstantItem(200).AlignRight().AlignMiddle().Column(c =>
                            {
                                c.Item().AlignRight()
                                    .Text($"Gerado em: {geradoEm:dd/MM/yyyy HH:mm}")
                                    .FontSize(9).FontColor("#AED6F1");
                                c.Item().AlignRight()
                                    .Text($"Total de itens no relatório: {itens.Count}")
                                    .FontSize(9).FontColor("#AED6F1");
                            });
                        });

                     
                        col.Item().Background("#EBF5FB").Padding(8).PaddingHorizontal(14).Row(r =>
                        {
                            r.RelativeItem().Text($"Qtd. total em estoque: {totalItens}")
                                .FontSize(10).Bold().FontColor("#1B2A4A");
                            r.RelativeItem().AlignCenter()
                                .Text($"Produtos listados: {itens.Count}")
                                .FontSize(10).Bold().FontColor("#1B2A4A");
                            r.RelativeItem().AlignRight()
                                .Text($"Valor total: {totalValor:C2}")
                                .FontSize(10).Bold().FontColor("#065F46");
                        });

                        col.Item().Height(10);

                        
                        col.Item().Background("#F3F4F6")
                           .Border(0.5f).BorderColor("#D1D5DB")
                           .Padding(6).Row(r =>
                           {
                               r.ConstantItem(40).Text("ID")
                                   .FontSize(9).Bold().FontColor("#374151");
                               r.ConstantItem(80).Text("Ref")
                                   .FontSize(9).Bold().FontColor("#374151");
                               r.RelativeItem().Text("Nome")
                                   .FontSize(9).Bold().FontColor("#374151");
                               r.ConstantItem(80).AlignCenter().Text("Tipo")
                                   .FontSize(9).Bold().FontColor("#374151");
                               r.ConstantItem(60).AlignCenter().Text("Qtd.")
                                   .FontSize(9).Bold().FontColor("#374151");
                               r.ConstantItem(80).AlignRight().Text("Vl. Unit.")
                                   .FontSize(9).Bold().FontColor("#374151");
                               r.ConstantItem(90).AlignRight().Text("Total")
                                   .FontSize(9).Bold().FontColor("#374151");
                           });
                    });

                    // ── LINHAS ─────────────────────────────────────
                    page.Content().Column(col =>
                    {
                        foreach (var (item, idx) in itens.Select((x, i) => (x, i)))
                        {
                            var bg = idx % 2 == 0 ? "#FFFFFF" : "#F8F9FA";
                            var qtdCor = item.Quantidade == 0 ? "#DC2626"
                                       : item.Quantidade <= 3 ? "#D97706"
                                       : "#111827";

                            col.Item().Background(bg)
                               .BorderBottom(0.3f).BorderColor("#E5E7EB")
                               .Padding(6).Row(r =>
                               {
                                   r.ConstantItem(40).Text(item.ProdutoId.ToString())
                                       .FontSize(9).FontColor("#6B7280");
                                   r.ConstantItem(80).Text(item.Ref)
                                       .FontSize(9).FontColor("#6B7280");
                                   r.RelativeItem().Text(item.Nome)
                                       .FontSize(9).FontColor("#111827");
                                   r.ConstantItem(80).AlignCenter().Text(item.Tipo)
                                       .FontSize(9).FontColor("#6B7280");
                                   r.ConstantItem(60).AlignCenter()
                                       .Text(item.Quantidade.ToString())
                                       .FontSize(9).Bold().FontColor(qtdCor);
                                   r.ConstantItem(80).AlignRight()
                                       .Text(item.ValorVenda.ToString("C2"))
                                       .FontSize(9).FontColor("#374151");
                                   r.ConstantItem(90).AlignRight()
                                       .Text(item.ValorTotal.ToString("C2"))
                                       .FontSize(9).Bold().FontColor("#065F46");
                               });
                        }

                        
                        col.Item().Height(6);
                        col.Item().Background("#1B2A4A").Padding(8).Row(r =>
                        {
                            r.RelativeItem().Text("TOTAL GERAL")
                                .FontSize(10).Bold().FontColor(Colors.White);
                            r.ConstantItem(60).AlignCenter()
                                .Text(totalItens.ToString())
                                .FontSize(10).Bold().FontColor(Colors.White);
                            r.ConstantItem(80).AlignRight()
                                .Text("").FontSize(10);
                            r.ConstantItem(90).AlignRight()
                                .Text(totalValor.ToString("C2"))
                                .FontSize(11).Bold().FontColor("#86EFAC");
                        });
                    });

                
                    page.Footer().Background("#F3F4F6").Padding(8).Row(r =>
                    {
                        r.RelativeItem().Text("Loja Diamante — Rua Laranjeiras, 189 – Centro – Aracaju/SE")
                            .FontSize(8).FontColor("#6B7280");
                        r.AutoItem().AlignRight()
                            .Text(x =>
                            {
                                x.Span("Página ").FontSize(8).FontColor("#6B7280");
                                x.CurrentPageNumber().FontSize(8).FontColor("#6B7280");
                                x.Span(" de ").FontSize(8).FontColor("#6B7280");
                                x.TotalPages().FontSize(8).FontColor("#6B7280");
                            });
                    });
                });
            }).GeneratePdf();
        }

        public static void SalvarComDialogo(List<EstoqueDetalhadoResponseDto> itens)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"Relatorio_Estoque_{DateTime.Now:yyyyMMdd_HHmm}",
                DefaultExt = ".pdf",
                Filter = "PDF|*.pdf"
            };

            if (dlg.ShowDialog() == true)
            {
                var bytes = Gerar(itens);
                File.WriteAllBytes(dlg.FileName, bytes);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = dlg.FileName,
                    UseShellExecute = true
                });
            }
        }
    }
}