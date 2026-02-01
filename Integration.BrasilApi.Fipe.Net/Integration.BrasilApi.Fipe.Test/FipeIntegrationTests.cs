using Integration.BrasilApi.Fipe.Domain.Interfaces;
using Integration.BrasilApi.Fipe.Infrastructure.Services;
using Microsoft.Extensions.Configuration; // Importante aqui também
using QuestPDF.Infrastructure;

namespace Integration.BrasilApi.Fipe.Test;

public class FipeIntegrationTests
{
    private readonly IFipeService _service;

    public FipeIntegrationTests()
    {
        QuestPDF.Settings.License = LicenseType.Community;

        // Criando uma configuração em memória para o teste
        var myConfiguration = new Dictionary<string, string>
        {
            {"BrasilApiConfig:BaseUrl", "https://brasilapi.com.br/api/fipe/preco/v1/"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration!)
            .Build();

        _service = new FipeService(configuration);
    }

    [Fact]
    public async Task ObterPreco_DeveRetornarVeiculo_QuandoCodigoValido()
    {
        // Act
        var resultado = await _service.ObterPrecoAsync("001004-9");

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Fiat", resultado.Marca);
    }
}