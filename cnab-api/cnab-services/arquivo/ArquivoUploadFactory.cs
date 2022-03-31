using cnab_entities.enums;
using cnab_entities.map;
using cnab_entities.models;
using Microsoft.AspNetCore.Http;

namespace cnab_services.arquivo
{
    internal static class ArquivoUploadFactory
    {
        public static IArquivoUpload<CNAB> Create(ArquivoUploadType arquivoUploadType, IFormFile file, int fileMaxSize, IArquivoMapPosicao arquivoMapPosicao)
        {
            return arquivoUploadType switch
            {
                ArquivoUploadType.TXT => new ArquivoUploadTXT(file, fileMaxSize, arquivoMapPosicao),
                _ => throw new Exception("Tipo não esperado."),
            };
        }
    }
}
