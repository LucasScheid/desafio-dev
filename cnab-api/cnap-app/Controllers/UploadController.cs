using cnab_entities.dto;
using cnab_entities.models;
using cnab_helpers.http;
using cnap_app.Models;
using Microsoft.AspNetCore.Mvc;

namespace cnap_app.Controllers
{
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IHttpUtil _httpUtil;
        private readonly string _urlApi;
        private readonly string _userApi;
        private readonly string _passwordApi;
        private readonly int _fileMaxSize;
        private readonly IList<string> _erros = new List<string>();

        public UploadController(ILogger<UploadController> logger, IHttpUtil httpUtil)
        {
            _logger = logger;
            _httpUtil = httpUtil;
            _urlApi = Environment.GetEnvironmentVariable("URL_API");
            _userApi = Environment.GetEnvironmentVariable("API_INTERNAL_USER");
            _passwordApi = Environment.GetEnvironmentVariable("API_INTERNAL_PASSWORD");
            _fileMaxSize = Convert.ToInt32(Environment.GetEnvironmentVariable("FILE_UPLOAD_MAX_SIZE"));
            _erros.Clear();
        }

        public IActionResult Index()
        {
            return View();
        }

        [DisableRequestSizeLimit]
        public async Task<IActionResult> Enviar(List<IFormFile> arquivos)
        {
            try
            {
                if (arquivos.Count == 0)
                {
                    _erros.Add("Nenhum arquivo selecionado, favor verificar.");
                    ViewData["Erro"] = _erros;
                    return View(ViewData);
                }

                foreach (var arquivo in arquivos)
                {
                    if (arquivo == null || arquivo.Length == 0)
                    {
                        _erros.Add("Arquivo vazio!");
                        ViewData["Erro"] = _erros;
                        return View(ViewData);
                    }

                    if (arquivo.Length > _fileMaxSize)
                    {
                        _erros.Add("Arquivo supera 2 mega!");
                        ViewData["Erro"] = _erros;
                        return View(ViewData);
                    }

                    ArquivoUploadResult<CNAB> resultUpload = await EnviarArquivoUpload(arquivo, (await ObterTokenAutenticacao()).Token);

                    if (resultUpload != null)
                    {
                        CompleteArquivoUpload<CNAB> arquivoUpload = resultUpload.CompleteArquivosUpload.FirstOrDefault();

                        if (arquivoUpload.Valido)
                        {
                            UploadResultado uploadResultado = new($"Arquivo {arquivo.FileName} carregado.", arquivoUpload.Resultado);
                            ViewData["Resultado"] = uploadResultado;
                        }
                        else
                        {
                            foreach  (string erro in arquivoUpload.Erros)
                                _erros.Add(erro);

                            ViewData["Erro"] = _erros;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _erros.Add(ex.Message);
                ViewData["Erro"] = _erros;
            }

            return View(ViewData);
        }

        private async Task<LoginResposta> ObterTokenAutenticacao()
        {
            return await _httpUtil.PostAsync<LoginRequisicao, LoginResposta>(string.Concat(_urlApi, "Login"), new LoginRequisicao(_userApi, _passwordApi));
        }

        private async Task<ArquivoUploadResult<CNAB>> EnviarArquivoUpload(IFormFile arquivo, string token)
        {
            return await _httpUtil.PostAsyncMultipart<ArquivoUploadResult<CNAB>>(string.Concat(_urlApi, "Operacoes/upload-arquivo-cnab"), arquivo, token); 
        }

    }
}
