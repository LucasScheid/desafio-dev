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
        private readonly string _urlApi;
        private readonly string _userApi;
        private readonly string _passwordApi;

        public ConsultaController(ILogger<ConsultaController> logger, IHttpUtil httpUtil)
        {
            _logger = logger;
            _httpUtil = httpUtil;
            _urlApi = Environment.GetEnvironmentVariable("URL_API");
            _userApi = Environment.GetEnvironmentVariable("API_INTERNAL_USER");
            _passwordApi = Environment.GetEnvironmentVariable("API_INTERNAL_PASSWORD");
        }

        public async Task<IActionResult> LojasResultado(string lojaFiltro)
        {
            try
            {
                OperacaoLojaResult lojaResult = await BuscarDadosLoja((await ObterTokenAutenticacao()).Token, lojaFiltro);

                if (lojaResult != null && lojaResult.Operacoes.Count > 0)
                {
                    ViewData["Resultado"] = lojaResult;
                }
                else
                {
                    ViewData["Mensagem"] = "Nenhum registro encontrado.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewData["Mensagem"] = ex.Message;
            }

            return View();
        }

        public async Task<IActionResult> LojasIndex()
        {
            try
            {
                IList<string> lojasFiltro = await BuscarLojasFiltro((await ObterTokenAutenticacao()).Token);

                if (lojasFiltro != null && lojasFiltro.Count > 0)
                {
                    ViewData["Resultado"] = lojasFiltro;
                }
                else
                {
                    ViewData["Mensagem"] = "Nenhuma loja encontrada no momento. Favor realizar uma carga via upload e retornar nesta opção.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewData["Mensagem"] = ex.Message;
            }

            return View();
        }

        public IActionResult GeralIndex()
        {
            return View();
        }

        public async Task<IActionResult> TiposTransacaoIndex()
        {
            try
            {
                ICollection<TipoTransacao> resultTipoTransacao = await BuscarTiposTransacao((await ObterTokenAutenticacao()).Token);

                if (resultTipoTransacao != null && resultTipoTransacao.Count > 0)
                {
                    TipoTransacaoResult tipoTransacaoResult = new(resultTipoTransacao);
                    ViewData["Resultado"] = tipoTransacaoResult;
                }
                else
                {
                    ViewData["Mensagem"] = "Nenhum registro encontrado.";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewData["Mensagem"] = ex.Message;
            }

            return View();
        }

        public IActionResult StatusDatabaseIndex()
        {
            return View();
        }

        private async Task<LoginResposta> ObterTokenAutenticacao()
        {
            return await _httpUtil.PostAsync<LoginRequisicao, LoginResposta>(string.Concat(_urlApi, "Login"), new LoginRequisicao(_userApi, _passwordApi));
        }

        private async Task<ICollection<TipoTransacao>> BuscarTiposTransacao(string token)
        {
            return await _httpUtil.GetAsync<ICollection<TipoTransacao>>(string.Concat(_urlApi, "Operacoes/obter-tipos-transacao"), token);
        }

        private async Task<IList<string>> BuscarLojasFiltro(string token)
        {
            return await _httpUtil.GetAsync<IList<string>>(string.Concat(_urlApi, "Operacoes/obter-todas-lojas"), token);
        }

        private async Task<OperacaoLojaResult> BuscarDadosLoja(string token, string nomeLoja)
        {
            return await _httpUtil.GetAsync<OperacaoLojaResult>(string.Concat(_urlApi, "Operacoes/obter-dados-loja?nomeLoja=", nomeLoja), token);
        }
    }
}
