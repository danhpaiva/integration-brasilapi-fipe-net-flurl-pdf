using Flurl.Http;
using Integration.BrasilApi.Fipe.Domain.Interfaces;
using Integration.BrasilApi.Fipe.Domain.Models;
using QuestPDF.Fluent; // Essencial para o .Create e .GeneratePdf
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Integration.BrasilApi.Fipe.Infrastructure.Services;

public class FipeService : IFipeService
{
    public async Task<VeiculoFipeResponse?> ObterPrecoAsync(string codigoFipe)
    {
        var url = $"https://brasilapi.com.br/api/fipe/preco/v1/{codigoFipe}";

        // A BrasilApi retorna 404 se não achar, o Flurl lança exceção por padrão.
        // Vamos tratar para retornar null se não encontrar.
        try
        {
            var result = await url.GetJsonAsync<List<VeiculoFipeResponse>>();
            var veiculo = result.FirstOrDefault();

            if (veiculo != null)
            {
                GerarRelatorioPdf(veiculo);
            }

            return veiculo;
        }
        catch (FlurlHttpException ex) when (ex.StatusCode == 404)
        {
            return null;
        }
    }

    private void GerarRelatorioPdf(VeiculoFipeResponse dados)
    {
        // Limpa o nome do arquivo para evitar caracteres inválidos
        var fileName = $"Fipe_{dados.CodigoFipe.Replace("-", "_")}.pdf";
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

        // Uso do Document do QuestPDF.Fluent
        QuestPDF.Fluent.Document.Create(container => {
            container.Page(page => {
                page.Margin(1, Unit.Centimetre);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily(Fonts.SegoeSD));

                page.Header().Row(row => {
                    row.RelativeItem().Column(col => {
                        col.Item().Text("RELATÓRIO DE AVALIAÇÃO").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                        col.Item().Text("Documento Oficial de Mercado").FontSize(10).Italic();
                    });

                    row.RelativeItem().AlignRight().Column(col => {
                        col.Item().Text("AUTO PREMIUM LTDA").FontSize(14).SemiBold();
                        col.Item().Text("CNPJ: 00.000.000/0001-00").FontSize(8);
                    });
                });

                page.Content().PaddingVertical(1, Unit.Centimetre).Column(col => {
                    col.Spacing(10);

                    col.Item().Background(Colors.Grey.Lighten3).Padding(5).Text("DADOS DO VEÍCULO").SemiBold();

                    col.Item().Table(table => {
                        table.ColumnsDefinition(columns => {
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(2);
                        });

                        table.Cell().Text("Marca:"); table.Cell().Text(dados.Marca);
                        table.Cell().Text("Modelo:"); table.Cell().Text(dados.Modelo);
                        table.Cell().Text("Ano:"); table.Cell().Text(dados.AnoModelo.ToString());
                        table.Cell().Text("Combustível:"); table.Cell().Text($"{dados.Combustivel} ({dados.SiglaCombustivel})");
                        table.Cell().Text("Código FIPE:"); table.Cell().Text(dados.CodigoFipe);
                    });

                    col.Item().PaddingTop(20).Element(container => {
                        container.Border(1).BorderColor(Colors.Grey.Medium).Background(Colors.Grey.Lighten4).Padding(10).Row(row => {
                            row.RelativeItem().Text("VALOR DE MERCADO (FIPE):").FontSize(14).SemiBold();
                            row.RelativeItem().AlignRight().Text(dados.Valor).FontSize(18).SemiBold().FontColor(Colors.Green.Medium);
                        });
                    });

                    col.Item().PaddingTop(50).Text("NOTAS:").Underline();
                    col.Item().Text("1. Este valor é uma referência baseada na tabela FIPE.");
                    col.Item().Text("2. A avaliação física do veículo pode alterar o valor final de revenda.");
                });

                page.Footer().AlignCenter().Column(col => {
                    col.Item().LineHorizontal(1);
                    col.Item().Text(x => {
                        x.Span("Gerado em: ").FontSize(9);
                        x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(9);
                    });
                });
            });
        }).GeneratePdf(path);
    }
}