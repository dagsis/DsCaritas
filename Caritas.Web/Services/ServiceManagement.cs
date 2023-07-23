using DsCommon.ModelsView;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO;
using System.IO.Pipes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;

namespace Caritas.Web.Services
{
    public class ServiceManagement : IServiceManagement
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _hostEnvironment;


        public ServiceManagement(IHttpClientFactory clientFactory, IWebHostEnvironment hostEnvironment)
        {
            _httpClientFactory = clientFactory;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<string> PostMail(string envia, string nombre, string asunto, string sHtml)
        {
            string body = sHtml;

            EmailViewModel viewModel = new EmailViewModel();

            var PathToFileDorso = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                    + "templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                    + Path.DirectorySeparatorChar.ToString() + "Dorso del Aviso.pdf";

            //viewModel.Usuario = "no-responder@dagsis.com.ar"; //"sistemaspanteon@caritasbsas.org.ar";
            //viewModel.Password = "Perg0022."; //"$ClavePanteon23";

            ////    viewModel.Usuario = "sistemaspanteon@caritasbsas.org.ar";
            ////    viewModel.Password = "$ClavePanteon23";
            //viewModel.Envia = envia;
            //viewModel.Asunto = asunto;
            //viewModel.HtmlMessage = body;
            //viewModel.DisplayName = nombre;

            //HttpResponseMessage? apiResponse = null;

            //HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
            //HttpRequestMessage message = new();

            //message.Headers.Add("Accept", "application/json");
            //message.RequestUri = new Uri("http://localhost:5172/v1/emailSender/emailSenderAplica");

            ////      message.RequestUri = new Uri("http://dagsist.net.ar/v1/emailSender/emailSenderGestion");

            //message.Content = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");

            //message.Method = HttpMethod.Post;
            //apiResponse = await client.SendAsync(message);

            //var apiContent = await apiResponse.Content.ReadAsStringAsync();

            //return "Ok";

            using (var client = new HttpClient())
            {
                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    //Add other fields
                    //     multipartFormContent.Add(new StringContent(null), name: "Servidor");
                    multipartFormContent.Add(new StringContent(nombre), name: "DisplayName");
                    multipartFormContent.Add(new StringContent(envia), name: "Envia");
                    multipartFormContent.Add(new StringContent("no-responder@dagsis.com.ar"), name: "Usuario");
                    multipartFormContent.Add(new StringContent("Perg0022."), name: "Password");
                    multipartFormContent.Add(new StringContent(asunto), name: "Asunto");
                    multipartFormContent.Add(new StringContent(body), name: "HtmlMessage");

                   // Add the file
                    var fileStreamContent = new StreamContent(File.OpenRead(PathToFileDorso));
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    multipartFormContent.Add(fileStreamContent, name: "uploadfile", fileName: "Dorso del Aviso.pdf");

                    //Send it
                    var response = await client.PostAsync("http://localhost:5172/v1/emailSender/emailSenderAplica", multipartFormContent);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }


        }
    }
}
