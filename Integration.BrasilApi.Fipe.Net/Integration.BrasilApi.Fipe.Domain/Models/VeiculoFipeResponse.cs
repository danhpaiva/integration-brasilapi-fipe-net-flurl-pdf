namespace Integration.BrasilApi.Fipe.Domain.Models;

public record VeiculoFipeResponse(
    string Valor,
    string Marca,
    string Modelo,
    int AnoModelo,
    string Combustivel,
    string CodigoFipe,
    string MesReferencia,
    int TipoVeiculo,
    string SiglaCombustivel,
    string DataConsulta
);
