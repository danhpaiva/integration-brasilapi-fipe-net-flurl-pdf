using Integration.BrasilApi.Fipe.Infrastructure.Services;

namespace Integration.BrasilApi.Fipe.Test;

public class FipeIntegrationTests
{
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
        var esperadoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"Fipe_{codigo}.pdf");

        // Act
        await service.ObterPrecoAsync(codigo);

        // Assert
        Assert.True(File.Exists(esperadoPath));
    }
}
