using DsCommon.Models;
using DsCommon.ModelsApi;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DsCommon.Services
{
    public class BaseService : IBaseService
    {
        // public ResponseDto responseModel { get; set; }
        public IHttpClientFactory httClient { get; set; }

        public BaseService(IHttpClientFactory httClient)
        {
            //   this.responseModel = new ResponseDto();
            this.httClient = httClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httClient.CreateClient("MangoAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data)
                          , Encoding.UTF8, "application/json");
                }

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
                }

                HttpResponseMessage apiResponse = null;
                switch (apiRequest.ApiType)
                {
                    case SDRutas.ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case SDRutas.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SDRutas.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SDRutas.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);

                return apiResponseDto;

            }
            catch (Exception ex)
            {

                var dto = new ResponseDto
                {
                    msg = ex.Message,
                    success = false
                };

                var res = JsonConvert.SerializeObject(dto);
                return JsonConvert.DeserializeObject<T>(res);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
