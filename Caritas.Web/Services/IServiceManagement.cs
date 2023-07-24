namespace Caritas.Web.Services
{
    public interface IServiceManagement
    {
        Task<string> PostMail(string envia,int cliente ,string nombre,string asunto,string sHtml);
    }
}
