using cnab_entities.dto;
using Microsoft.AspNetCore.Http;

namespace cnab_services.arquivo
{
    public interface IArquivoService<T>
    {
        Task<ArquivoUploadResult<T>> Upload(List<IFormFile> fileList);
    }
}
