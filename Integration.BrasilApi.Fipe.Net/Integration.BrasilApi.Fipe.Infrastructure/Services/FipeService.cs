using Flurl.Http;
using Integration.BrasilApi.Fipe.Domain.Interfaces;
using Integration.BrasilApi.Fipe.Domain.Models;
using System.Reflection.Metadata;

namespace Integration.BrasilApi.Fipe.Infrastructure.Services;

public class FipeService : IFipeService
{
    public async Task<VeiculoFipeResponse> ObterPrecoAsync(string codigoFipe)
    {
        var url = $"https://brasilapi.com.br/api/fipe/preco/v1/{codigoFipe}";
        var result = await url.GetJsonAsync<List<VeiculoFipeResponse>>();

        var veiculo = result.FirstOrDefault();
        if (veiculo != null)
        {
            GerarRelatorioPdf(veiculo);
        }

        return veiculo;
    }

    private void GerarRelatorioPdf(VeiculoFipeResponse dados)
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"Fipe_{dados.CodigoFipe}.pdf");

        Document.Create(container => {
            container.Page(page => {
                page.Margin(1, Unit.Centimetre);
                page.Header().Row(row => {
                    row.RelativeItem().Text("RELATÓRIO DE AVALIAÇÃO AUTOMOTIVA").FontSize(20).SemiBold().FontColor("#2d3436");
                    row.RelativeItem().AlignRight().Text("AUTO PREMIUM LTDA").FontSize(10).Italic();
                });

                page.Content().PaddingVertical(1, Unit.Centimetre).Column(col => {
                    col.Item().Text($"Modelo: {dados.Modelo}").FontSize(14);
                    col.Item().Text($"Marca: {dados.Marca}").FontSize(12);
                    col.Item().PaddingTop(5).LineHorizontal(1);

                    col.Item().PaddingTop(10).Table(table => {
                        table.ColumnsDefinition(columns => {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                        table.Cell().Text("Ano Modelo:"); table.Cell().Text(dados.AnoModelo.ToString());
                        table.Cell().Text("Combustível:"); table.Cell().Text(dados.Combustivel);
                        table.Cell().Text("Mês de Referência:"); table.Cell().Text(dados.MesReferencia);
                        table.Cell().Background("#f1f2f6").Text("VALOR DE MERCADO:").SemiBold();
                        table.Cell().Background("#f1f2f6").Text(dados.Valor).SemiBold();
                    });
                });

                page.Footer().AlignCenter().Text(x => {
                    x.Span("Documento gerado em: ");
                    x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                });
            });
        }).GeneratePdf(path);
    }
}
