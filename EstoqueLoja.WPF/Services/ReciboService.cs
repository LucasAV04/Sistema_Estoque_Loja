using EstoqueLoja.WPF.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace EstoqueLoja.WPF.Services
{
    public static class ReciboService
    {
        // Azul Oceano da identidade
        private static readonly string AzulOceano = "#1A5276";
        private static readonly string AzulClaro = "#EBF5FB";
        private static readonly string CinzaBorda = "#BDC3C7";
        private static readonly string CinzaTexto = "#555555";
        private static readonly string PretoTexto = "#1C1C1C";

        public static byte[] Gerar(VendaResponseDto venda, DadosClienteDto cliente)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var subtotal = venda.Itens.Sum(i => i.ValorTotal);
            var totalFinal = subtotal - cliente.Desconto;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginHorizontal(36);
                    page.MarginVertical(30);
                    page.DefaultTextStyle(x =>
                        x.FontSize(10)
                         .FontFamily("Arial")
                         .FontColor(PretoTexto));

                    page.Content().Column(col =>
                    {
                        // ═══════════════════════════════════════════
                        // CABEÇALHO
                        // ═══════════════════════════════════════════
                        col.Item().Border(1.5f).BorderColor(AzulOceano)
                           .Column(header =>
                           {
                               // Faixa azul com nome da loja
                               header.Item()
                                     .Background(AzulOceano)
                                     .PaddingVertical(14)
                                     .PaddingHorizontal(20)
                                     .Row(row =>
                                     {
                                         row.RelativeItem().Column(c =>
                                         {
                                             c.Item().Text("LOJA DIAMANTE")
                                              .FontSize(28)
                                              .Bold()
                                              .FontColor(Colors.White);
                                             c.Item().Text("PROPOSTA DE COMPRA")
                                              .FontSize(11)
                                              .FontColor("#AED6F1");
                                         });

                                         row.ConstantItem(160).AlignRight().AlignMiddle().Column(c =>
                                         {
                                             c.Item().AlignRight().Text($"Nº {venda.Id:D6}")
                                              .FontSize(18)
                                              .Bold()
                                              .FontColor(Colors.White);
                                             c.Item().AlignRight()
                                              .Text(venda.Data.ToString("dd/MM/yyyy  HH:mm"))
                                              .FontSize(10)
                                              .FontColor("#AED6F1");
                                         });
                                     });

                               // Endereço da loja
                               header.Item()
                                     .Background(AzulClaro)
                                     .PaddingVertical(5)
                                     .PaddingHorizontal(20)
                                     .AlignCenter()
                                     .Text("Rua Laranjeiras, 189 – Centro  |  Tel.: (79) 3214-6229  |  Aracaju – SE")
                                     .FontSize(9)
                                     .FontColor(AzulOceano);
                           });

                        col.Item().Height(10);

                        // ═══════════════════════════════════════════
                        // DADOS DO CLIENTE
                        // ═══════════════════════════════════════════
                        col.Item().Border(0.75f).BorderColor(CinzaBorda).Column(secao =>
                        {
                            // Título da seção
                            secao.Item()
                                 .Background(AzulOceano)
                                 .Padding(6)
                                 .PaddingHorizontal(10)
                                 .Text("DADOS DO CLIENTE")
                                 .FontSize(9)
                                 .Bold()
                                 .FontColor(Colors.White);

                            secao.Item().Padding(10).Column(campos =>
                            {
                                // Linha 1: Nome + CPF/CNPJ
                                campos.Item().Row(row =>
                                {
                                    row.RelativeItem(3).Column(c =>
                                    {
                                        c.Item().Text("Nome:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text(cliente.Nome).FontSize(10).Bold();
                                    });
                                    row.ConstantItem(16);
                                    row.RelativeItem().Column(c =>
                                    {
                                        c.Item().Text("CPF / CNPJ:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text(cliente.CpfCnpj).FontSize(10);
                                    });
                                    row.ConstantItem(16);
                                    row.RelativeItem().Column(c =>
                                    {
                                        c.Item().Text("Telefone:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text(cliente.Telefone).FontSize(10);
                                    });
                                });

                                campos.Item().Height(8);

                                // Linha 2: Rua + Número + Apto/Complemento
                                campos.Item().Row(row =>
                                {
                                    row.RelativeItem(3).Column(c =>
                                    {
                                        c.Item().Text("Rua:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text(cliente.Rua).FontSize(10);
                                    });
                                    row.ConstantItem(12);
                                    row.RelativeItem().Column(c =>
                                    {
                                        c.Item().Text("Número:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text(cliente.Numero).FontSize(10);
                                    });
                                });

                                campos.Item().Height(8);

                                // Linha 3: Bairro + Cidade + Estado + CEP
                                campos.Item().Row(row =>
                                {
                                    row.RelativeItem(2).Column(c =>
                                    {
                                        c.Item().Text("Bairro:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text(cliente.Bairro).FontSize(10);
                                    });
                                    row.ConstantItem(12);
                                    row.RelativeItem(2).Column(c =>
                                    {
                                        c.Item().Text("Cidade:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text(cliente.Cidade).FontSize(10);
                                    });
                                    row.ConstantItem(12);
                                    row.RelativeItem().Column(c =>
                                    {
                                        c.Item().Text("Estado:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text(cliente.Estado).FontSize(10);
                                    });
                                    row.ConstantItem(12);
                                    row.RelativeItem().Column(c =>
                                    {
                                        c.Item().Text("CEP:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text(cliente.Cep).FontSize(10);
                                    });
                                });

                                campos.Item().Height(8);

                                // Ponto de Referência
                                campos.Item().Column(c =>
                                {
                                    c.Item().Text("Ponto de Referência:").FontSize(8).FontColor(CinzaTexto);
                                    c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                        .PaddingBottom(3)
                                        .Text(cliente.PontoReferencia).FontSize(10);
                                });

                                campos.Item().Height(8);

                                // Vendedor
                                campos.Item().Row(row =>
                                {
                                    row.RelativeItem().Column(c =>
                                    {
                                        c.Item().Text("Vendedor:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text(cliente.Vendedor).FontSize(10);
                                    });
                                    row.ConstantItem(12);
                                    row.RelativeItem(2).Column(c =>
                                    {
                                        c.Item().Text("Loja / Venda:").FontSize(8).FontColor(CinzaTexto);
                                        c.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                            .PaddingBottom(3)
                                            .Text("Loja Diamante").FontSize(10);
                                    });
                                });
                            });
                        });

                        col.Item().Height(10);

                        // ═══════════════════════════════════════════
                        // TABELA DE MERCADORIAS
                        // ═══════════════════════════════════════════
                        col.Item().Border(0.75f).BorderColor(CinzaBorda).Column(tabela =>
                        {
                            tabela.Item()
                                  .Background(AzulOceano)
                                  .Padding(6)
                                  .PaddingHorizontal(10)
                                  .Text("MERCADORIAS")
                                  .FontSize(9)
                                  .Bold()
                                  .FontColor(Colors.White);

                            // Cabeçalho da tabela
                            tabela.Item()
                                  .Background(AzulClaro)
                                  .BorderBottom(0.75f).BorderColor(CinzaBorda)
                                  .Padding(6)
                                  .Row(row =>
                                  {
                                      row.RelativeItem(5).Text("MERCADORIA")
                                          .FontSize(9).Bold().FontColor(AzulOceano);
                                      row.RelativeItem().AlignCenter().Text("QTD.")
                                          .FontSize(9).Bold().FontColor(AzulOceano);
                                      row.RelativeItem(2).AlignRight().Text("P. UNITÁRIO")
                                          .FontSize(9).Bold().FontColor(AzulOceano);
                                      row.RelativeItem(2).AlignRight().Text("TOTAL")
                                          .FontSize(9).Bold().FontColor(AzulOceano);
                                  });

                            // Linhas dos itens
                            var pares = venda.Itens
                                .Select((item, index) => (item, index))
                                .ToList();

                            foreach (var (item, index) in pares)
                            {
                                var bg = index % 2 == 0 ? "#FFFFFF" : "#F8F9FA";

                                tabela.Item()
                                      .Background(bg)
                                      .BorderBottom(0.3f).BorderColor("#E0E0E0")
                                      .Padding(6)
                                      .Row(row =>
                                      {
                                          row.RelativeItem(5).Column(c =>
                                          {
                                              c.Item().Text(item.NomeProduto)
                                                  .FontSize(10);
                                              c.Item().Text($"REF: {item.RefProduto}")
                                                  .FontSize(8).FontColor(CinzaTexto);
                                          });
                                          row.RelativeItem().AlignCenter()
                                              .AlignMiddle()
                                              .Text(item.Quantidade.ToString())
                                              .FontSize(10);
                                          row.RelativeItem(2).AlignRight()
                                              .AlignMiddle()
                                              .Text(item.ValorUnitario.ToString("C2"))
                                              .FontSize(10);
                                          row.RelativeItem(2).AlignRight()
                                              .AlignMiddle()
                                              .Text(item.ValorTotal.ToString("C2"))
                                              .FontSize(10).Bold();
                                      });
                            }

                            // Linhas em branco para completar (mínimo 5 linhas como no físico)
                            var linhasFaltando = Math.Max(0, 5 - venda.Itens.Count);
                            for (int i = 0; i < linhasFaltando; i++)
                            {
                                tabela.Item()
                                      .BorderBottom(0.3f).BorderColor("#E0E0E0")
                                      .Height(22);
                            }
                        });

                        col.Item().Height(8);

                        // ═══════════════════════════════════════════
                        // TOTAIS + PAGAMENTO
                        // ═══════════════════════════════════════════
                        col.Item().Row(row =>
                        {
                            // FORMA DE PAGAMENTO (esquerda)
                            row.RelativeItem().Border(0.75f).BorderColor(CinzaBorda)
                               .Column(pag =>
                               {
                                   pag.Item()
                                      .Background(AzulOceano)
                                      .Padding(6).PaddingHorizontal(10)
                                      .Text("FORMA DE PAGAMENTO")
                                      .FontSize(9).Bold().FontColor(Colors.White);

                                   pag.Item().Padding(10).Column(c =>
                                   {
                                       var formas = new[] { "À Vista", "Cartão", "Boleto", "Pix" };
                                       foreach (var forma in formas)
                                       {
                                           var marcado = cliente.FormaPagamento
                                               .Equals(forma, StringComparison.OrdinalIgnoreCase);

                                           c.Item().Row(r =>
                                           {
                                               r.ConstantItem(16).Border(0.75f)
                                                .BorderColor(CinzaBorda)
                                                .Background(marcado ? AzulOceano : Colors.White)
                                                .Height(14).Width(14)
                                                .AlignCenter().AlignMiddle()
                                                .Text(marcado ? "✓" : " ")
                                                .FontSize(9).Bold()
                                                .FontColor(Colors.White);
                                               r.ConstantItem(6);
                                               r.AutoItem()
                                                .AlignMiddle()
                                                .Text(forma)
                                                .FontSize(10);
                                                
                                           });
                                           c.Item().Height(6);
                                       }

                                       c.Item().Height(6);
                                       c.Item().Column(e =>
                                       {
                                           e.Item().Text("Entrada R$:").FontSize(8).FontColor(CinzaTexto);
                                           e.Item().BorderBottom(0.5f).BorderColor(CinzaBorda)
                                               .PaddingBottom(3)
                                               .Text(cliente.Entrada > 0
                                                   ? cliente.Entrada.ToString("C2")
                                                   : "")
                                               .FontSize(10).Bold();
                                       });
                                   });
                               });

                            row.ConstantItem(10);

                            // RESUMO FINANCEIRO (direita)
                            row.RelativeItem().Border(0.75f).BorderColor(CinzaBorda)
                               .Column(fin =>
                               {
                                   fin.Item()
                                      .Background(AzulOceano)
                                      .Padding(6).PaddingHorizontal(10)
                                      .Text("RESUMO FINANCEIRO")
                                      .FontSize(9).Bold().FontColor(Colors.White);

                                   fin.Item().Padding(10).Column(c =>
                                   {
                                       // Subtotal
                                       c.Item().Row(r =>
                                       {
                                           r.RelativeItem().Text("Subtotal:").FontSize(10);
                                           r.AutoItem().Text(subtotal.ToString("C2")).FontSize(10);
                                       });
                                       c.Item().Height(6);

                                       // Desconto
                                       c.Item().Row(r =>
                                       {
                                           r.RelativeItem().Text("Desconto:").FontSize(10)
                                               .FontColor(cliente.Desconto > 0 ? "#C0392B" : CinzaTexto);
                                           r.AutoItem().Text(cliente.Desconto > 0
                                               ? $"- {cliente.Desconto:C2}"
                                               : "—")
                                               .FontSize(10)
                                               .FontColor(cliente.Desconto > 0 ? "#C0392B" : CinzaTexto);
                                       });
                                       c.Item().Height(6);

                                       // Entrada
                                       if (cliente.Entrada > 0)
                                       {
                                           c.Item().Row(r =>
                                           {
                                               r.RelativeItem().Text("Entrada:").FontSize(10);
                                               r.AutoItem().Text(cliente.Entrada.ToString("C2"))
                                                   .FontSize(10);
                                           });
                                           c.Item().Height(6);
                                       }

                                       c.Item().LineHorizontal(0.75f).LineColor(CinzaBorda);
                                       c.Item().Height(6);

                                       // TOTAL FINAL
                                       c.Item().Background(AzulOceano)
                                           .Padding(8)
                                           .Row(r =>
                                           {
                                               r.RelativeItem().Text("TOTAL R$")
                                                .FontSize(11).Bold().FontColor(Colors.White);
                                               r.AutoItem().Text(totalFinal.ToString("C2"))
                                                .FontSize(13).Bold().FontColor(Colors.White);
                                           });

                                       if (cliente.Entrada > 0)
                                       {
                                           c.Item().Height(6);
                                           c.Item().Row(r =>
                                           {
                                               r.RelativeItem().Text("Saldo restante:")
                                                   .FontSize(10).FontColor(CinzaTexto);
                                               r.AutoItem()
                                                   .Text((totalFinal - cliente.Entrada).ToString("C2"))
                                                   .FontSize(10).Bold();
                                           });
                                       }
                                   });
                               });
                        });

                        col.Item().Height(10);

                        // ═══════════════════════════════════════════
                        // OBSERVAÇÕES
                        // ═══════════════════════════════════════════
                        if (!string.IsNullOrWhiteSpace(cliente.Observacoes))
                        {
                            col.Item().Border(0.75f).BorderColor(CinzaBorda).Column(obs =>
                            {
                                obs.Item()
                                   .Background(AzulOceano)
                                   .Padding(6).PaddingHorizontal(10)
                                   .Text("OBSERVAÇÕES")
                                   .FontSize(9).Bold().FontColor(Colors.White);

                                obs.Item().Padding(10)
                                    .Text(cliente.Observacoes)
                                    .FontSize(10);
                            });

                            col.Item().Height(10);
                        }

                        // ═══════════════════════════════════════════
                        // RODAPÉ
                        // ═══════════════════════════════════════════
                        col.Item().Background(AzulOceano)
                           .Padding(8)
                           .AlignCenter()
                           .Column(f =>
                           {
                               f.Item().AlignCenter()
                                   .Text("Rua Laranjeiras, 189 – Centro  |  Tel.: (79) 3214-6229")
                                   .FontSize(9).FontColor(Colors.White);
                               f.Item().AlignCenter()
                                   .Text("E-mail: jaugustovasconcelos@uol.com.br  |  Aracaju – SE")
                                   .FontSize(9).FontColor("#AED6F1");
                           });
                    });
                });
            }).GeneratePdf();
        }

        public static void SalvarComDialogo(VendaResponseDto venda, DadosClienteDto? cliente = null)
        {
            cliente ??= new DadosClienteDto();

            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"Recibo_Venda_{venda.Id:D6}",
                DefaultExt = ".pdf",
                Filter = "PDF|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                var bytes = Gerar(venda, cliente);
                File.WriteAllBytes(dialog.FileName, bytes);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = dialog.FileName,
                    UseShellExecute = true
                });
            }
        }
    }
}