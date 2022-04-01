using cnab_entities.enums;
using cnab_entities.map;
using cnab_entities.models;
using cnab_services.database;
using Microsoft.AspNetCore.Http;

namespace cnab_services.arquivo
{
    internal static class ArquivoUploadFactory
    {
        public static IArquivoUpload<CNAB> Create(ArquivoUploadType arquivoUploadType, IFormFile file, int fileMaxSize, IArquivoMapPosicao arquivoMapPosicao, ITipoTransacaoDbService tipoTransacaoDbService)
        {
            return arquivoUploadType switch
            {
                ArquivoUploadType.TXT => new ArquivoUploadTXT(file, fileMaxSize, arquivoMapPosicao, tipoTransacaoDbService),
                _ => throw new Exception("Tipo não esperado."),
            };
        }
    }
}
