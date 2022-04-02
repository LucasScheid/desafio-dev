using cnab_entities.dto;
using cnab_entities.models;
using cnab_helpers.http;
using cnap_app.Models;
using Microsoft.AspNetCore.Mvc;

namespace cnap_app.Controllers
{
    public class ConsultaController : Controller
    {
        private readonly ILogger<ConsultaController> _logger;
        private readonly IHttpUtil _httpUtil;

        public ConsultaController(ILogger<ConsultaController> logger, IHttpUtil httpUtil)
        {
            _logger = logger;
            _httpUtil = httpUtil;
        }

        public IActionResult LojasIndex()
        {
            return View();
        }

        public IActionResult GeralIndex()
        {
            return View();
        }

        public IActionResult TiposTransacaoIndex()
        {
            return View();
        }

        public IActionResult StatusDatabaseIndex()
        {
            return View();
        }
    }
}
