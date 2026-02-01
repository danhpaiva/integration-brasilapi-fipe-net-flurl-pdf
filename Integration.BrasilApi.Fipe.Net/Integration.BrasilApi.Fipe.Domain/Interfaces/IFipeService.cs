using Integration.BrasilApi.Fipe.Domain.Models;

namespace Integration.BrasilApi.Fipe.Domain.Interfaces;

public interface IFipeService
{
    Task<VeiculoFipeResponse> ObterPrecoAsync(string codigoFipe);
}
