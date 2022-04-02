using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace cnab_helpers.http
{
    public class HttpUtil : IHttpUtil
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpUtil(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private static ByteArrayContent ConvertObjectToByteArrayContent<T>(T valor)
        {
            ByteArrayContent byteContent = new ByteArrayContent((Encoding.UTF8.GetBytes((JsonConvert.SerializeObject(valor)))));
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }

        private static string GetHttErrorpMessage(HttpResponseMessage httpResponse, string operation, string url)
        {
            return $"Erro Http [{operation}] : [{httpResponse.StatusCode} - {httpResponse.Content.ReadAsStringAsync().Result}] URL: [{ url }]";
        }

        private HttpClient GetHttpClient(string token = null)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();

            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return httpClient;
        }

        public async Task<R> GetAsync<R>(string url, string token = null)
        {
            using HttpClient httpClient = GetHttpClient(token);

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<R>(responseContent);
            }
            else
            {
                throw new Exception(GetHttErrorpMessage(response, "GetAsync<R>", url));
            }
        }

        public async Task PutAsync<B>(string url, B corpoRequisicao, string token = null)
        {
            using HttpClient httpClient = GetHttpClient(token);

            var content = ConvertObjectToByteArrayContent(corpoRequisicao);

            var response = await httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(GetHttErrorpMessage(response, "PutAsync<B>", url));
            }
        }

        public async Task<R> PostAsync<B, R>(string url, B corpoRequisicao, string token = null)
        {
            using HttpClient httpClient = GetHttpClient(token);

            var requestContent = ConvertObjectToByteArrayContent(corpoRequisicao);

            var response = await httpClient.PostAsync(url, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<R>(responseContent);
            }
            else
            {
                throw new Exception(GetHttErrorpMessage(response, "PostAsync<B,R>", url));
            }
        }

        public async Task PostAsync<B>(string url, B corpoRequisicao, string token = null)
        {
            using HttpClient httpClient = GetHttpClient(token);

            var requestContent = ConvertObjectToByteArrayContent(corpoRequisicao);

            var response = await httpClient.PostAsync(url, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(GetHttErrorpMessage(response, "PostAsync<B>", url));
            }
        }

        public async Task<R> PostAsync<R>(string url, string token = null)
        {
            using HttpClient httpClient = GetHttpClient(token);

            var response = await httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(GetHttErrorpMessage(response, "PostAsync", url));
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<R>(responseContent);
            }
        }

        public async Task PutAsync(string url, string token = null)
        {
            using HttpClient httpClient = GetHttpClient(token);

            var response = await httpClient.PutAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(GetHttErrorpMessage(response, "PutAsync", url));
            }
        }

        public async Task GetAsync(string url, string token = null)
        {
            using HttpClient httpClient = GetHttpClient(token);

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(GetHttErrorpMessage(response, "GetAsync", url));
            }
        }

        public async Task DeleteAsync(string url, string token = null)
        {
            using HttpClient httpClient = GetHttpClient(token);

            var response = await httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(GetHttErrorpMessage(response, "DeleteAsync", url));
            }
        }

        public async Task<R> PostAsyncMultipart<R>(string url, IFormFile arquivo, string token = null)
        {
            using HttpClient httpClient = GetHttpClient(token);

            using StreamContent streamContent = new(arquivo.OpenReadStream());
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            using var content = new MultipartFormDataContent { { streamContent, "fileList", arquivo.FileName } };

            request.Content = content;

            var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<R>(responseContent);
        }
    }
}
