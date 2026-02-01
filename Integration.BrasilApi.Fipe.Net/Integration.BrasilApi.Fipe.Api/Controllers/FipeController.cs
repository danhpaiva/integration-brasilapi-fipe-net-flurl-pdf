using Integration.BrasilApi.Fipe.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Integration.BrasilApi.Fipe.Api.Controllers;

public class FipeController : Controller
{
    private readonly IFipeService _fipeService;
    private readonly ILogger<FipeController> _logger;

    public FipeController(IFipeService fipeService, ILogger<FipeController> logger)
    {
        _fipeService = fipeService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Detalhes(string codigoFipe)
    {
        if (string.IsNullOrWhiteSpace(codigoFipe))
        {
            ModelState.AddModelError("", "O código FIPE é obrigatório.");
            return View("Index");
        }

        try
        {
            var veiculo = await _fipeService.ObterPrecoAsync(codigoFipe);

            if (veiculo == null)
            {
                ViewBag.ErrorMessage = "Veículo não encontrado para o código informado.";
                return View("Index");
            }

            // O PDF já é gerado dentro do serviço conforme a regra de negócio
            return View(veiculo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar código FIPE {Codigo}", codigoFipe);
            ViewBag.ErrorMessage = "Erro de integração com a BrasilAPI. Tente novamente mais tarde.";
            return View("Index");
        }
    }
}
