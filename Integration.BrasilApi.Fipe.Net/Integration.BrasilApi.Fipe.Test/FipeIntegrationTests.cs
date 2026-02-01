using Integration.BrasilApi.Fipe.Infrastructure.Services;
using QuestPDF.Infrastructure;

namespace Integration.BrasilApi.Fipe.Test;

public class FipeIntegrationTests
{
    public FipeIntegrationTests()
    {
        // Essencial para o ambiente de testes não estourar Exception
        QuestPDF.Settings.License = LicenseType.Community;
    }
    [Fact]
    public async Task ObterPreco_DeveRetornarVeiculo_QuandoCodigoValido()
    {
        // Arrange
        var service = new FipeService(); // Em prod usaríamos Mock do Http
        var codigoValido = "001004-9";

        // Act
        var resultado = await service.ObterPrecoAsync(codigoValido);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Fiat", resultado.Marca);
        Assert.Contains("Palio", resultado.Modelo);
    }

    [Fact]
    public async Task ObterPreco_DeveGerarArquivoNoDesktop()
    {
        // Arrange
        var service = new FipeService();
        var codigo = "001004-9";
        // Ajuste aqui para bater com a lógica do Service (ex: usar underline se o service usar)
        var nomeArquivo = $"Fipe_{codigo.Replace("-", "_")}.pdf";
        var esperadoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), nomeArquivo);

        // Act
        await service.ObterPrecoAsync(codigo);

        // Assert
        var existe = File.Exists(esperadoPath);
        Assert.True(existe, $"O arquivo deveria ter sido criado em: {esperadoPath}");

        // Clean up (Opcional, mas recomendado para devs Sênior)
        if (existe) File.Delete(esperadoPath);
    }
}
