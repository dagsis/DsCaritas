using DsCommon.ModelsView;
using Newtonsoft.Json;
using System.Text;

namespace Caritas.Web.Services
{
    public class ServiceManagement : IServiceManagement
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceManagement(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<string> PostMail(string envia, string nombre,string asunto,string sHtml)
        {
            HttpResponseMessage? apiResponse = null;
            string body = sHtml;

            HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
            HttpRequestMessage message = new();

            EmailViewModel viewModel = new EmailViewModel();

            viewModel.Usuario = "no-responder@dagsis.com.ar"; //"sistemaspanteon@caritasbsas.org.ar";
            viewModel.Password = "Perg0022."; //"$ClavePanteon23";

        //    viewModel.Usuario = "sistemaspanteon@caritasbsas.org.ar";
        //    viewModel.Password = "$ClavePanteon23";
            viewModel.Envia = envia;
            viewModel.Asunto = asunto;
            viewModel.HtmlMessage = body;
            viewModel.DisplayName = nombre;

            message.Headers.Add("Accept", "application/json");

            message.RequestUri = new Uri("http://localhost:5172/v1/emailSender/emailSenderGestion");

       //     message.RequestUri = new Uri("http://dagsist.net.ar/v1/emailSender/emailSenderGestion");

            message.Content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Post;

            apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            //  var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            
            return "Ok";
        }
    }
}
