namespace Caritas.Web.Services
{
    public interface IServiceManagement
    {
        Task<string> PostMail(string envia, string nombre,string asunto,string sHtml);
    }
}
