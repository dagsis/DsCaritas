using Azure;
using Caritas.Insfrastructure.Models;
using DsCommon.IUnitOfWorkPatern;
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
        private readonly IUnitOfWork _unitWork;


        public ServiceManagement(IHttpClientFactory clientFactory, IWebHostEnvironment hostEnvironment, IUnitOfWork unitWork)
        {
            _httpClientFactory = clientFactory;
            _hostEnvironment = hostEnvironment;
            _unitWork = unitWork;
        }
        public async Task<string> PostMail(string envia,int cliente ,string nombre, string asunto, string sHtml)
        {
            string body = sHtml;

            EmailViewModel viewModel = new EmailViewModel();

            var PathToFileDorso = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                    + "templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                    + Path.DirectorySeparatorChar.ToString() + "Dorso del Aviso.pdf";

            var PathToFilePdf = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                  + "templates" + Path.DirectorySeparatorChar.ToString() + "PdfAvisos"
                  + Path.DirectorySeparatorChar.ToString() + cliente + ".pdf";
           
            using (var client = new HttpClient())
            {


                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    //Add other fields
                    multipartFormContent.Add(new StringContent("Panteon Nuestra Señora de la  Merced "), name: "DisplayName");
                    multipartFormContent.Add(new StringContent(envia), name: "Envia");
                  //  multipartFormContent.Add(new StringContent("no-responder@dagsis.com.ar"), name: "Usuario");
                  //  multipartFormContent.Add(new StringContent("Perg0022."), name: "Password");
                    multipartFormContent.Add(new StringContent("sistemaspanteon@panteonlamerced.org.ar"), name: "Usuario");
                    multipartFormContent.Add(new StringContent("ClavePanteon23"), name: "Password");
                    multipartFormContent.Add(new StringContent(asunto), name: "Asunto");
                    multipartFormContent.Add(new StringContent(body), name: "HtmlMessage");

                    // Add the file
                    var fileStreamContent = new StreamContent(File.OpenRead(PathToFileDorso));
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    multipartFormContent.Add(fileStreamContent, name: "uploadfile", fileName: "Dorso del Aviso.pdf");

                    if (File.Exists(PathToFilePdf))
                    {
                        var fileStreamContent1 = new StreamContent(File.OpenRead(PathToFilePdf));
                        fileStreamContent1.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                        multipartFormContent.Add(fileStreamContent1, name: "uploadfile", fileName: cliente + ".pdf");
                    }

                    var resul = new Resultado();

                    try
                    {
                        var response = await client.PostAsync("http://dagsist.net.ar/v1/emailSender/emailSenderAplica", multipartFormContent);

                        response.EnsureSuccessStatusCode();

                        resul.Cliente = cliente;
                        resul.Email = envia;
                        resul.Result = await response.Content.ReadAsStringAsync() == "" ? "Ok" : await response.Content.ReadAsStringAsync();
                        
                       await  _unitWork.Repository<Resultado>().AddAsync(resul);

                        return await response.Content.ReadAsStringAsync();

                    }
                    catch (Exception ex )
                    {

                        resul.Cliente = cliente;
                        resul.Email = envia;
                        resul.Result = ex.Message;

                        await _unitWork.Repository<Resultado>().AddAsync(resul);
                      
                    }
                    //Send it
                 //   var response = await client.PostAsync("http://localhost:5172/v1/emailSender/emailSenderAplica", multipartFormContent);

                }

                return "Ok";
            }


        }
    }
}
