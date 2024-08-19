using Microsoft.AspNetCore.Mvc;

namespace FlightsApi.Controllers;

public class ErrorsController : Controller
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public IActionResult HandleError() =>
        Problem();
}
