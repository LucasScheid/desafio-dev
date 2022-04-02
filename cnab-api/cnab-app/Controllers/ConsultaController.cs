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

        public async Task<IActionResult> GeralIndex()
        {
            try
            {
                IList<CNABDatabase> result = await BuscarTodosRegistros((await ObterTokenAutenticacao()).Token);

                if (result != null && result.Count > 0)
                    ViewData["Resultado"] = result;
                else
                    ViewData["Mensagem"] = "Nenhum registro encontrado.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ViewData["Mensagem"] = ex.Message;
            }

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

        public async Task<IActionResult> StatusDatabaseIndex()
        {
            string mensagem = "A infraestrutura de banco ainda não está 100% disponível, aguarde alguns segundos e tente novamente por favor.";

            try
            {
                ICollection<TipoTransacao> resultTipoTransacao = await BuscarTiposTransacao((await ObterTokenAutenticacao()).Token);

                if (resultTipoTransacao != null && resultTipoTransacao.Count > 0)
                    ViewData["Resultado"] = "Banco de Dados online e 100% disponível para utilização!";
                else
                    ViewData["Mensagem"] = mensagem;
            }
            catch (Exception)
            {
                ViewData["Mensagem"] = mensagem;
            }

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

        private async Task<IList<CNABDatabase>> BuscarTodosRegistros(string token)
        {
            return await _httpUtil.GetAsync<IList<CNABDatabase>>(string.Concat(_urlApi, "Operacoes/obter-todos-registros-cnab"), token);
        }

    }
}
