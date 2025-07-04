using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class FormsModel : PageModel
    {
        private readonly ILogger<FormsModel> _logger;

        public FormsModel(ILogger<FormsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // L�gica para cargar la p�gina de formularios
            _logger.LogInformation("P�gina de Formularios cargada");
        }
    }
}