using EstoqueLoja.WPF.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace EstoqueLoja.WPF.Services
{
    public static class NotaFiscalPdfService
    {
        private const string Azul = "#1A5276";
        private const string AzulCl = "#EBF5FB";
        private const string Cinza = "#BDC3C7";
        private const string CinzaTx = "#555555";

        public static byte[] Gerar(
            NotaFiscalResponseDto nf,
            VendaResponseDto venda)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var subtotal = venda.Itens.Sum(i => i.ValorTotal);
            var totalFinal = subtotal - nf.Desconto;

            return Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.MarginHorizontal(28);
                    page.MarginVertical(24);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    page.Content().Column(col =>
                    {
                        // ── TOPO: aviso de recebimento ──────────────────
                        col.Item().Border(0.75f).BorderColor(Cinza).Padding(6).Row(r =>
                        {
                            r.RelativeItem(3).Text(
                                $"RECEBEMOS DE {nf.NomeCliente.ToUpper()} OS PRODUTOS / SERVIÇOS " +
                                $"CONSTANTES DA NOTA FISCAL INDICADO AO LADO\n" +
                                $"EMISSÃO: {nf.DataEmissao:dd/MM/yyyy}  –  " +
                                $"DEST./REM.: {nf.NomeCliente}  –  " +
                                $"VALOR TOTAL: R$ {totalFinal:N2}")
                                .FontSize(8).FontColor(CinzaTx);
                            r.ConstantItem(10);
                            r.ConstantItem(120).Column(c =>
                            {
                                c.Item().Border(0.75f).BorderColor(Cinza)
                                    .Background(AzulCl).Padding(6).Column(x =>
                                    {
                                        x.Item().Text("NF-e").FontSize(11).Bold().FontColor(Azul);
                                        x.Item().Text($"Nº {nf.Numero}").FontSize(10).Bold();
                                        x.Item().Text($"SÉRIE {nf.Serie}").FontSize(9);
                                    });
                            });
                        });

                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Border(0.5f).BorderColor(Cinza)
                                .Padding(4).Text("DATA DE RECEBIMENTO").FontSize(7).FontColor(CinzaTx);
                            r.RelativeItem(3).Border(0.5f).BorderColor(Cinza)
                                .Padding(4).Text("IDENTIFICAÇÃO E ASSINATURA DO RECEBEDOR").FontSize(7).FontColor(CinzaTx);
                        });

                        col.Item().Height(6);

                        // ── CABEÇALHO PRINCIPAL ─────────────────────────
                        col.Item().Border(1f).BorderColor(Cinza).Row(r =>
                        {
                            // Emitente
                            r.RelativeItem(2).BorderRight(0.75f).BorderColor(Cinza)
                             .Padding(10).Column(c =>
                             {
                                 c.Item().Text("IDENTIFICAÇÃO DO EMITENTE").FontSize(7).FontColor(CinzaTx);
                                 c.Item().Text("LOJA DIAMANTE")
                                     .FontSize(16).Bold().FontColor(Azul);
                                 c.Item().Height(4);
                                 c.Item().Text("Rua Laranjeiras, 189 – CENTRO – CEP: 49010-000")
                                     .FontSize(8);
                                 c.Item().Text("Aracaju – SE").FontSize(8);
                                 c.Item().Text("TEL: 3214-6229").FontSize(8);
                                 c.Item().Height(4);
                                 c.Item().Text($"CNPJ: 27.082.513-4").FontSize(8).Bold();
                                 c.Item().Text("Insc. Estadual: 27.082.513-4").FontSize(8);
                             });

                            // Centro: DANFE
                            r.RelativeItem(2).BorderRight(0.75f).BorderColor(Cinza)
                             .Padding(8).Column(c =>
                             {
                                 c.Item().AlignCenter().Text("DANFE").FontSize(14).Bold().FontColor(Azul);
                                 c.Item().AlignCenter().Text("DOCUMENTO AUXILIAR DA")
                                     .FontSize(8).FontColor(CinzaTx);
                                 c.Item().AlignCenter().Text("NOTA FISCAL ELETRÔNICA")
                                     .FontSize(8).FontColor(CinzaTx);
                                 c.Item().Height(6);
                                 c.Item().Row(row =>
                                 {
                                     row.RelativeItem().Border(0.75f).BorderColor(Cinza)
                                         .Padding(4).Column(x =>
                                         {
                                             x.Item().AlignCenter().Text("0 – ENTRADA").FontSize(8);
                                             x.Item().AlignCenter().Text("1 – SAÍDA").FontSize(8).Bold();
                                         });
                                     row.ConstantItem(6);
                                     row.ConstantItem(34).Border(0.75f).BorderColor(Azul)
                                         .Background(AzulCl)
                                         .AlignCenter().AlignMiddle()
                                         .Text("1").FontSize(20).Bold().FontColor(Azul);
                                 });
                                 c.Item().Height(6);
                                 c.Item().AlignCenter()
                                     .Text($"Nº {nf.Numero}  fl. 1 / 1")
                                     .FontSize(9).Bold();
                                 c.Item().AlignCenter()
                                     .Text($"SÉRIE {nf.Serie}")
                                     .FontSize(9).Bold();
                             });

                            // Direita: datas
                            r.RelativeItem().Padding(8).Column(c =>
                            {
                                c.Item().Text("DATA DE EMISSÃO").FontSize(7).FontColor(CinzaTx);
                                c.Item().Border(0.5f).BorderColor(Cinza).Padding(4)
                                    .Text(nf.DataEmissao.ToString("dd/MM/yyyy")).FontSize(10).Bold();
                                c.Item().Height(6);
                                c.Item().Text("HORA DE EMISSÃO").FontSize(7).FontColor(CinzaTx);
                                c.Item().Border(0.5f).BorderColor(Cinza).Padding(4)
                                    .Text(nf.DataEmissao.ToString("HH:mm:ss")).FontSize(10).Bold();
                                c.Item().Height(6);
                                c.Item().Text("DATA SAÍDA/ENTRADA").FontSize(7).FontColor(CinzaTx);
                                c.Item().Border(0.5f).BorderColor(Cinza).Padding(4)
                                    .Text(nf.DataEmissao.ToString("dd/MM/yyyy")).FontSize(9);
                            });
                        });

                        // Natureza da Operação
                        col.Item().Border(0.75f).BorderColor(Cinza).Row(r =>
                        {
                            r.RelativeItem(2).BorderRight(0.5f).BorderColor(Cinza).Padding(5).Column(c =>
                            {
                                c.Item().Text("NATUREZA DA OPERAÇÃO").FontSize(7).FontColor(CinzaTx);
                                c.Item().Text(nf.NaturezaOperacao).FontSize(9).Bold();
                            });
                            r.RelativeItem().Padding(5).Column(c =>
                            {
                                c.Item().Text("INSCRIÇÃO ESTADUAL").FontSize(7).FontColor(CinzaTx);
                                c.Item().Text("27.082.513-4").FontSize(9);
                            });
                        });

                        col.Item().Height(4);

                        // ── DESTINATÁRIO ────────────────────────────────
                        col.Item().Background(Azul).Padding(5).PaddingHorizontal(8)
                            .Text("DESTINATÁRIO / REMETENTE")
                            .FontSize(8).Bold().FontColor(Colors.White);

                        col.Item().Border(0.75f).BorderColor(Cinza).Padding(8).Column(dest =>
                        {
                            // Linha 1
                            dest.Item().Row(r =>
                            {
                                r.RelativeItem(3).Column(c =>
                                {
                                    c.Item().Text("NOME / RAZÃO SOCIAL").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text(nf.NomeCliente.ToUpper()).FontSize(10).Bold();
                                });
                                r.ConstantItem(10);
                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("CNPJ / CPF").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text(nf.CpfCnpjCliente).FontSize(10);
                                });
                                r.ConstantItem(10);
                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("DATA DA EMISSÃO").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text(nf.DataEmissao.ToString("dd/MM/yyyy")).FontSize(10);
                                });
                            });
                            dest.Item().Height(6);
                            // Linha 2: Endereço
                            dest.Item().Row(r =>
                            {
                                r.RelativeItem(3).Column(c =>
                                {
                                    c.Item().Text("ENDEREÇO").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text(nf.EnderecoCliente).FontSize(10);
                                });
                                r.ConstantItem(10);
                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("BAIRRO / DISTRITO").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text(nf.BairroCliente).FontSize(10);
                                });
                                r.ConstantItem(10);
                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("CEP").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text(nf.CepCliente).FontSize(9);
                                });
                            });
                            dest.Item().Height(6);
                            // Linha 3
                            dest.Item().Row(r =>
                            {
                                r.RelativeItem(2).Column(c =>
                                {
                                    c.Item().Text("MUNICÍPIO").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text(nf.MunicipioCliente).FontSize(10);
                                });
                                r.ConstantItem(10);
                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("FONE / FAX").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text(nf.TelefoneCliente).FontSize(9);
                                });
                                r.ConstantItem(10);
                                r.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("UF").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text(nf.UfCliente).FontSize(10);
                                });
                                r.ConstantItem(10);
                                r.RelativeItem(2).Column(c =>
                                {
                                    c.Item().Text("INSCRIÇÃO ESTADUAL").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text("—").FontSize(9);
                                });
                            });
                        });

                        col.Item().Height(4);

                        // ── CÁLCULO DO IMPOSTO ──────────────────────────
                        col.Item().Background(Azul).Padding(5).PaddingHorizontal(8)
                            .Text("CÁLCULO DO IMPOSTO")
                            .FontSize(8).Bold().FontColor(Colors.White);

                        col.Item().Border(0.75f).BorderColor(Cinza).Row(r =>
                        {
                            void CelulaImposto(ColumnDescriptor c, string label, string valor)
                            {
                                c.Item().BorderRight(0.5f).BorderColor(Cinza).Padding(5).Column(x =>
                                {
                                    x.Item().Text(label).FontSize(7).FontColor(CinzaTx);
                                    x.Item().Text(valor).FontSize(9).Bold();
                                });
                            }
                            r.RelativeItem().Column(c => CelulaImposto(c, "BASE CALC. DO ICMS", "0,00"));
                            r.RelativeItem().Column(c => CelulaImposto(c, "VALOR DO ICMS", "0,00"));
                            r.RelativeItem().Column(c => CelulaImposto(c, "BASE CALC. ICMS SUBST.", "0,00"));
                            r.RelativeItem().Column(c => CelulaImposto(c, "VALOR ICMS SUBST.", "0,00"));
                            r.RelativeItem().Column(c => CelulaImposto(c,
                                "VALOR TOTAL DOS PRODUTOS", $"{subtotal:N2}"));
                        });

                        col.Item().Border(0.75f).BorderColor(Cinza).Row(r =>
                        {
                            void CelulaImposto2(ColumnDescriptor c, string label, string valor)
                            {
                                c.Item().BorderRight(0.5f).BorderColor(Cinza).Padding(5).Column(x =>
                                {
                                    x.Item().Text(label).FontSize(7).FontColor(CinzaTx);
                                    x.Item().Text(valor).FontSize(9).Bold();
                                });
                            }
                            r.RelativeItem().Column(c => CelulaImposto2(c, "VALOR DO FRETE", "0,00"));
                            r.RelativeItem().Column(c => CelulaImposto2(c, "VALOR DO SEGURO", "0,00"));
                            r.RelativeItem().Column(c => CelulaImposto2(c, "DESCONTO",
                                nf.Desconto > 0 ? nf.Desconto.ToString("N2") : "0,00"));
                            r.RelativeItem().Column(c => CelulaImposto2(c, "OUTRAS DESP. ACESS.", "0,00"));
                            r.RelativeItem().Column(c => CelulaImposto2(c, "VALOR DO IPI", "0,00"));
                            r.RelativeItem().Column(c =>
                            {
                                c.Item().Padding(5).Column(x =>
                                {
                                    x.Item().Text("VALOR TOTAL DA NOTA").FontSize(7).FontColor(CinzaTx);
                                    x.Item().Text($"R$ {totalFinal:N2}").FontSize(11)
                                        .Bold().FontColor(Azul);
                                });
                            });
                        });

                        col.Item().Height(4);

                        // ── TRANSPORTADOR ───────────────────────────────
                        col.Item().Background(Azul).Padding(5).PaddingHorizontal(8)
                            .Text("TRANSPORTADOR / VOLUMES TRANSPORTADOS")
                            .FontSize(8).Bold().FontColor(Colors.White);

                        col.Item().Border(0.75f).BorderColor(Cinza).Row(r =>
                        {
                            r.RelativeItem(2).BorderRight(0.5f).BorderColor(Cinza).Padding(5).Column(c =>
                            {
                                c.Item().Text("RAZÃO SOCIAL").FontSize(7).FontColor(CinzaTx);
                                c.Item().Text("—").FontSize(9);
                            });
                            r.RelativeItem().BorderRight(0.5f).BorderColor(Cinza).Padding(5).Column(c =>
                            {
                                c.Item().Text("FRETE POR CONTA").FontSize(7).FontColor(CinzaTx);
                                c.Item().Text("9 – SEM FRETE").FontSize(9).Bold();
                            });
                            r.RelativeItem().Padding(5).Column(c =>
                            {
                                c.Item().Text("QUANTIDADE").FontSize(7).FontColor(CinzaTx);
                                c.Item().Text($"{venda.Itens.Sum(i => i.Quantidade)}").FontSize(9);
                            });
                        });

                        col.Item().Height(4);

                        // ── DADOS DO PRODUTO / SERVIÇOS ─────────────────
                        col.Item().Background(Azul).Padding(5).PaddingHorizontal(8)
                            .Text("DADOS DO PRODUTO / SERVIÇOS")
                            .FontSize(8).Bold().FontColor(Colors.White);

                        // Cabeçalho da tabela
                        col.Item().Background(AzulCl).Border(0.75f).BorderColor(Cinza)
                           .Padding(4).Row(r =>
                           {
                               void Th(RowDescriptor row, string t, uint w = 1)
                               {
                                   row.RelativeItem(w).Text(t)
                                       .FontSize(7).Bold().FontColor(Azul);
                               }
                               Th(r, "CÓD.", 1);
                               Th(r, "DESCRIÇÃO DO PRODUTO / SERVIÇO", 5);
                               Th(r, "UN.", 1);
                               Th(r, "QUANT.", 1);
                               Th(r, "VALOR UNIT.", 2);
                               Th(r, "DESCONTO", 2);
                               Th(r, "VALOR TOTAL", 2);
                           });

                        foreach (var (item, idx) in venda.Itens.Select((x, i) => (x, i)))
                        {
                            var bg = idx % 2 == 0 ? "#FFFFFF" : "#F8F9FA"; 
                            col.Item().Background(bg).Border(0.5f).BorderColor("#E0E0E0")
                               .Padding(4).Row(r =>
                               {
                                   r.RelativeItem(1).Text(item.ProdutoId.ToString()).FontSize(8);
                                   r.RelativeItem(5).Column(c =>
                                   {
                                       c.Item().Text(item.NomeProduto).FontSize(8);
                                       c.Item().Text($"REF: {item.RefProduto}")
                                           .FontSize(7).FontColor(CinzaTx);
                                   });
                                   r.RelativeItem(1).Text("UN").FontSize(8);
                                   r.RelativeItem(1).AlignRight()
                                       .Text(item.Quantidade.ToString()).FontSize(8);
                                   r.RelativeItem(2).AlignRight()
                                       .Text(item.ValorUnitario.ToString("N2")).FontSize(8);
                                   r.RelativeItem(2).AlignRight()
                                       .Text("0,00").FontSize(8).FontColor(CinzaTx);
                                   r.RelativeItem(2).AlignRight()
                                       .Text(item.ValorTotal.ToString("N2")).FontSize(8).Bold();
                               });
                        }

                        // Linhas em branco
                        for (int i = 0; i < Math.Max(0, 4 - venda.Itens.Count); i++)
                            col.Item().Border(0.5f).BorderColor("#E0E0E0").Height(18);

                        col.Item().Height(4);

                        // ── FORMA DE PAGAMENTO + TOTAIS ─────────────────
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Border(0.75f).BorderColor(Cinza).Column(pag =>
                            {
                                pag.Item().Background(Azul).Padding(5).PaddingHorizontal(8)
                                    .Text("FORMA DE PAGAMENTO")
                                    .FontSize(8).Bold().FontColor(Colors.White);

                                var formas = new[] { "À Vista", "Cartão", "Boleto", "Pix" };
                                foreach (var f in formas)
                                {
                                    var sel = nf.FormaPagamento.Equals(f,
                                        StringComparison.OrdinalIgnoreCase);
                                    pag.Item().Padding(5).Row(pr =>
                                    {
                                        pr.ConstantItem(14).Border(0.75f).BorderColor(Cinza)
                                          .Background(sel ? Azul : Colors.White)
                                          .Height(12).AlignCenter().AlignMiddle()
                                          .Text(sel ? "✓" : " ").FontSize(8)
                                          .FontColor(sel ? Colors.White : Colors.White);
                                        pr.ConstantItem(6);
                                        pr.AutoItem().AlignMiddle()
                                          .Text(f).FontSize(9);
                                    });
                                }

                                pag.Item().Padding(5).Column(c =>
                                {
                                    c.Item().Text("VENDEDOR").FontSize(7).FontColor(CinzaTx);
                                    c.Item().BorderBottom(0.5f).BorderColor(Cinza).PaddingBottom(3)
                                        .Text(nf.Vendedor).FontSize(9);
                                });
                            });

                            r.ConstantItem(8);

                            r.RelativeItem().Border(0.75f).BorderColor(Cinza).Column(tot =>
                            {
                                tot.Item().Background(Azul).Padding(5).PaddingHorizontal(8)
                                    .Text("RESUMO DOS VALORES")
                                    .FontSize(8).Bold().FontColor(Colors.White);

                                void Linha(ColumnDescriptor c, string label, string val,
                                    bool bold = false, bool destaque = false)
                                {
                                    c.Item().Padding(4).Row(r2 =>
                                    {
                                        r2.RelativeItem().Text(label).FontSize(9)
                                            .FontColor(destaque ? Azul : Colors.Black);
                                        r2.AutoItem().Text(val).FontSize(9)
                                            .FontColor(destaque ? Azul : Colors.Black);
                                    });
                                    c.Item().LineHorizontal(0.3f).LineColor("#E0E0E0");
                                }

                                tot.Item().Padding(4).Column(c =>
                                {
                                    Linha(c, "Subtotal:", $"R$ {subtotal:N2}");
                                    Linha(c, "Desconto:", nf.Desconto > 0
                                        ? $"- R$ {nf.Desconto:N2}" : "—");
                                    Linha(c, "Frete:", "—");
                                    Linha(c, "TOTAL DA NOTA:", $"R$ {totalFinal:N2}",
                                        bold: true, destaque: true);
                                });
                            });
                        });

                        col.Item().Height(6);

                        // ── DADOS ADICIONAIS ────────────────────────────
                        col.Item().Border(0.75f).BorderColor(Cinza).Column(add =>
                        {
                            add.Item().Background(Azul).Padding(5).PaddingHorizontal(8)
                                .Text("DADOS ADICIONAIS").FontSize(8).Bold().FontColor(Colors.White);

                            add.Item().Padding(8).Column(c =>
                            {
                                c.Item().Text("INFORMAÇÕES COMPLEMENTARES")
                                    .FontSize(7).FontColor(CinzaTx);
                                c.Item().Height(3);

                                var obs = string.IsNullOrWhiteSpace(nf.Observacoes)
                                    ? "EMPRESA OPTANTE PELO SIMPLES NACIONAL."
                                    : nf.Observacoes;
                                c.Item().Text(obs).FontSize(9);
                            });
                        });

                        col.Item().Height(10);

                        // ── ASSINATURAS ─────────────────────────────────
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Column(c =>
                            {
                                c.Item().BorderBottom(0.75f).BorderColor("#333").Height(28);
                                c.Item().AlignCenter().PaddingTop(4)
                                    .Text("Assinatura do Destinatário").FontSize(8).FontColor(CinzaTx);
                            });
                            r.ConstantItem(30);
                            r.RelativeItem().Column(c =>
                            {
                                c.Item().BorderBottom(0.75f).BorderColor("#333").Height(28);
                                c.Item().AlignCenter().PaddingTop(4)
                                    .Text("Assinatura do Emitente / Vendedor").FontSize(8).FontColor(CinzaTx);
                            });
                        });

                        col.Item().Height(10);

                        // ── RODAPÉ ──────────────────────────────────────
                        col.Item().Background(Azul).Padding(6).AlignCenter().Column(f =>
                        {
                            f.Item().AlignCenter()
                                .Text("LOJA DIAMANTE  |  Rua Laranjeiras, 189 – Centro  |  Tel.: (79) 3214-6229")
                                .FontSize(8).FontColor(Colors.White);
                            f.Item().AlignCenter()
                                .Text("E-mail: jaugustovasconcelos@uol.com.br  |  Aracaju – SE")
                                .FontSize(8).FontColor("#AED6F1");
                        });
                    });
                });
            }).GeneratePdf();
        }

        public static void SalvarComDialogo(
            NotaFiscalResponseDto nf,
            VendaResponseDto venda)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"NF_{nf.Numero}_Venda_{nf.VendaId}",
                DefaultExt = ".pdf",
                Filter = "PDF|*.pdf"
            };

            if (dlg.ShowDialog() == true)
            {
                var bytes = Gerar(nf, venda);
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
