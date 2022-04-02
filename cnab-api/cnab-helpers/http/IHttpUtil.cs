using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace cnab_helpers.http
{
    public interface IHttpUtil
    {
        Task<R> PostAsync<B, R>(string url, B corpoRequisicao, string token = null);

        Task<R> PostAsyncMultipart<R>(string url, IFormFile arquivo, string token = null);

        Task PostAsync<B>(string url, B corpoRequisicao, string token = null);
        Task<R> PostAsync<R>(string url, string token = null);

        Task<R> GetAsync<R>(string url, string token = null);
        Task GetAsync(string url, string token = null);
    }
}
