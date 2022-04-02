using cnab_entities.dto;
using cnab_entities.models;
using cnab_services.arquivo;
using cnab_services.database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace cnab_api.Controllers
{
    public class OperacoesController : BaseController
    {
        private readonly IArquivoService<CNAB> _arquivoService;
        private readonly IArquivoDbService _arquivoDbService;
        private readonly ITipoTransacaoDbService _tipoTransacaoDbService;

        public OperacoesController(ILogger<OperacoesController> log, IArquivoDbService arquivoDbService, ITipoTransacaoDbService tipoTransacaoDbService, IArquivoService<CNAB> arquivoService) : base(log)
        {
            _arquivoDbService = arquivoDbService;
            _arquivoService = arquivoService;
            _tipoTransacaoDbService = tipoTransacaoDbService;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload-arquivo-cnab")]
        [SwaggerOperation(Summary = "Upload arquivo CNAB.", Description = "Método responsável por fazer o upload e processamento do arquivo cnab e a gravação na base de dados.")]
        [SwaggerResponse(201, "O processamento do arquivo e a gravação na base de dados foram realizados com sucesso.")]
        [SwaggerResponse(400, "Ocorreu um erro durante alguma das etapas do processamento.")]
        [SwaggerResponse(401, "Operação não autorizada pois necessita do token de autenticação.")]
        public async Task<IActionResult> UploadArquivoCnab(List<IFormFile> fileList)
        {
            try
            {
                if (fileList == null)
                    return BadRequest("A lista de arquivos está nula!");

                if (fileList.Count == 0)
                    return BadRequest("A lista de arquivos está vazia!");

                ArquivoUploadResult<CNAB> resultadoUpload = await _arquivoService.Upload(fileList);
                CompleteArquivoUpload<CNAB>? cnab = resultadoUpload.CompleteArquivosUpload.FirstOrDefault();

                if (cnab != null && cnab.Valido)
                {
                    await _arquivoDbService.Adicionar(cnab);
                    return Created(String.Empty, resultadoUpload);
                }
                else
                    return BadRequest(resultadoUpload);
            }
            catch (Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }

        [HttpGet]
        [Route("obter-todas-lojas")]
        [SwaggerOperation(Summary = "Obter uma lista com o nome de todas as lojas.", Description = "Método responsável por buscar na base de dados o nome de todas as lojas cadastradas.")]
        [SwaggerResponse(200, "As lojas foram encontradas.")]
        [SwaggerResponse(204, "Não foi possível encontrar nenhuma loja cadastrada.")]
        [SwaggerResponse(400, "Ocorreu um erro durante a busca do nome das lojas.")]
        [SwaggerResponse(401, "Operação não autorizada pois necessita do token de autenticação.")]
        public async Task<IActionResult> ObterTodasLojas()
        {
            try
            {
                IList<string> result = await _arquivoDbService.ObterTodasLojas();

                if (result != null && result.Count > 0)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }

        [HttpGet]
        [Route("obter-dados-loja")]
        [SwaggerOperation(Summary = "Obter as operações realizadas por uma loja específica.", Description = "Método responsável por buscar na base de dados as operações da loja passada como parâmetro.")]
        [SwaggerResponse(200, "As operações da loja informada foram encontradas.")]
        [SwaggerResponse(204, "Não foi possível encontrar nenhuma operação para a loja informada.")]
        [SwaggerResponse(400, "Ocorreu um erro durante a busca das operações referente a loja.")]
        [SwaggerResponse(401, "Operação não autorizada pois necessita do token de autenticação.")]
        public async Task<IActionResult> ObterDadosLoja([BindRequired] string nomeLoja)
        {
            try
            {
                OperacaoLojaResult result = await _arquivoDbService.ObterDadosLoja(nomeLoja);

                if (result != null && result.Operacoes != null && result.Operacoes.Count > 0)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }

        [HttpGet]
        [Route("obter-todos-registros-cnab")]
        [SwaggerOperation(Summary = "Obter todos os registros CNAB inseridos na base de dados via upload de arquivo.", Description = "Método responsável por buscar na base de dados uma lista completa com todos os registros inseridos via UPLOAD.")]
        [SwaggerResponse(200, "Os registros foram encontrados.")]
        [SwaggerResponse(204, "Não foi possível encontrar nenhum registro inserido na base de dados.")]
        [SwaggerResponse(400, "Ocorreu um erro durante a busca dos registros.")]
        [SwaggerResponse(401, "Operação não autorizada pois necessita do token de autenticação.")]
        public async Task<IActionResult> ObterTodosRegistros()
        {
            try
            {
                IList<CNABDatabase> result = await _arquivoDbService.ObterTodosRegistros();

                if (result != null && result.Count > 0)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }

        [HttpGet]
        [Route("obter-tipos-transacao")]
        [SwaggerOperation(Summary = "Obter todos os tipos de transações de um arquivo CNAB.", Description = "Método responsável por buscar na base de dados uma lista completa com todos os tipos de transações possíveis de um arquivo CNAB.")]
        [SwaggerResponse(200, "Os tipos de transação foram encontrados.")]
        [SwaggerResponse(204, "Não foi possível encontrar nenhum registro inserido na base de dados.")]
        [SwaggerResponse(400, "Ocorreu um erro durante a busca dos tipos de transações.")]
        [SwaggerResponse(401, "Operação não autorizada pois necessita do token de autenticação.")]
        public async Task<IActionResult> ObterTiposTransacao()
        {
            try
            {
                IList<TipoTransacao> result = await _tipoTransacaoDbService.ObterTodosRegistros();

                if (result != null && result.Count > 0)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return LogErrorMessage(GetControllerName(), GetActionName(), ex.Message);
            }
        }
    }
}
