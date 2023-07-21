using AutoMapper;
using Caritas.Insfrastructure.Models;
using Caritas.Web.DTOs;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.ModelsTable;
using DsCommon.ModelsView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;

namespace Caritas.Web.Controllers
{
    [Authorize(Roles = "Administrador,Usuario,Empleado")]
    public class AvisoController : Controller
    {
        private readonly IUnitOfWork _unitWork;
        private readonly IAuthorizationService _authorizationService;
        private readonly IWebHostEnvironment _hostEnvironment;

        IMapper _mapper;
        private int contador = 0;

        public AvisoController(IUnitOfWork unitWork, IAuthorizationService authorizationService, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            _unitWork = unitWork;
            _authorizationService = authorizationService;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            bool bCreate = true;
            bool bEdit = true;
            bool bDelete = true;

            var model = new TableUIExt()
            {
                Metodo = "POST",
                CreateUrl = "Aviso",
                IsOnPermisoCreate = bCreate,
                IsServerSide = "true",
                IsOnModalCreate = false,
                WidthAcciones = "15%",
                ViewOrder = new OrderUI[] {
                    new OrderUI() { Order = 1 },
                },
                Dom = "Bfrtip",
                ApiUrl = "/Aviso/GetAll",
                Fields = new FieldUI[] {
                    new FieldUI() { Label = "Id", Data = "id", ColumWidth = "5%" },
                    new FieldUI() { Label = "Descripción", Data = "descripcion", ColumWidth = "35%" },
                    new FieldUI() { Label = "Plantilla", Data = "plantilla", ColumWidth = "35%" },
                },
                Methods = new MethodsUI[] {
                    new MethodsUI() {Icono="fa fa-eye",titulo="Enviar Emails",Url="/Aviso/Detail",Clase="btn btn-info btn-sm",IsOnModal=false },
                    new MethodsUI() {Icono="fa fa-pencil",titulo="Editar",Url="/Aviso/Edit",Clase="btn btn-primary btn-sm",IsOnModal=false,Permiso = bEdit },
                    new MethodsUI() {Icono="fa fa-trash",titulo="Borrar",Url="/Aviso/Delete",Clase="btn btn-danger btn-sm",Permiso = bDelete },
                }
            };

            ViewBag.titulo = "Listado de Plantillas";
            ViewData["Title"] = "Avisos";
            ViewData["TableTitle"] = "Listado de Plantillas para Emails";

            return View("~/Views/Shared/_TableViewExt.cshtml", model);
        }

        public async Task<IActionResult> GetAll()
        {

            //Representa el número de veces que se ha realizado una petición
            int NroPeticion = Convert.ToInt32(Request.Form["draw"].FirstOrDefault() ?? "0");

            //cuantos registros va a devolver
            int CantidadRegistros = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            //cuantos registros va a omitir
            int OmitirRegistros = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            //el texto de busqueda
            string ValorBuscado = Request.Form["search[value]"].FirstOrDefault() ?? "";

            var queryRegistro = await _unitWork.Repository<PlantillaEmail>().GetAsync(null, null, "", true);

            //  Total de registros antes de filtrar.
            int TotalRegistros = queryRegistro.Count();

            var queryRegis = queryRegistro
                  .Where(e => string.Concat(e.Id, e.Descripcion).Contains(ValorBuscado));

            // Total de registros ya filtrados.
            int TotalRegistrosFiltrados = queryRegis.Count();

            switch (sortColumn)
            {
                case "descripcion":
                    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) queryRegis = queryRegis.OrderBy(c => c.Descripcion);
                    break;
                default:
                    break;
            }


            var allObj = queryRegis.Skip(OmitirRegistros).Take(CantidadRegistros);

            List<PlantillaEmailDto> avisos = _mapper.Map<List<PlantillaEmailDto>>(allObj);

            //foreach (var item in clientes)
            //{
            //    if ((int)item.Estado == 1)
            //    {
            //        item.Status = "<span class='badge rounded-pill bg-success'>Activo</span>";
            //    }
            //    else
            //    {
            //        item.Status = "<span class='badge rounded-pill bg-danger'>Inactivo</span>";
            //    }
            //}

            return Json(new
            {
                draw = NroPeticion,
                recordsTotal = TotalRegistros,
                recordsFiltered = TotalRegistrosFiltrados,
                data = avisos
            });

        }


        [HttpGet]
        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<IActionResult> Detail(int id)
        {

            var allObj = await _unitWork.Repository<PlantillaEmail>().GetByIdAsync(id);

            PlantillaEmailDto avisos = _mapper.Map<PlantillaEmailDto>(allObj);


            ViewBag.Title = "Plantilla Email";


            return View(avisos);
        }

       
        [HttpPost]
        public async Task<JsonResult> GetTextAsync()
        {

            var aviso = await _unitWork.Repository<Aviso>().GetAsync(null, x => x.OrderBy(y => y.Cliente), "", true);
          
            return Json(aviso);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<JsonResult> Enviar(int cliente)
        {
            var avisos = await  _unitWork.Repository<Aviso>().GetAsync(x=>x.Cliente == cliente,null,"",true);

            var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                      + "templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                      + Path.DirectorySeparatorChar.ToString() + "Aviso.html";

            var subject = $"Aviso de Periodo Vencido - Panteon Ntra Sra de la Merced - {avisos[0].Cliente}";

            string HtmlBody = "";
            using (StreamReader streamReader = System.IO.File.OpenText(PathToFile))
            {
                HtmlBody = streamReader.ReadToEnd();
            }

            ////{0} : Subject  
            ////{1} : DateTime  
            ////{2} : Name  
            ////{3} : Email  
            ////{4} : Message  
            ////{5} : callbackURL  

            string nombre = avisos[0].Nombre + ' ' + avisos[0].Apellido;
            string tipo = "";
            string codigo = avisos[0].CPagoElectronico;
            decimal importe = 0;

            foreach (var item in avisos)
            {
                tipo = tipo + (item.Tipo == "N" ? "Nicho " : "Urna ") + item.Nicho + " / " + item.Piso + "° desde " + item.FecDesde.ToString("dd/MM/yyyy") + " hasta " + item.FecHasta.ToString("dd/MM/yyyy") + "<br>";
                importe = importe + item.Importe;
            }

            string messageBody = string.Format(HtmlBody,
                subject,
                nombre,
                tipo,
                importe.ToString("N2"),
                codigo
                );
            EmailViewModel emailViewModel = new EmailViewModel()
            {
                Asunto = subject,
                DisplayName = "Carlos D Agostino",
                Envia = "carlos@dagsistemas.com.ar",
                HtmlMessage = messageBody,
                Usuario = "carlos@dagsistemas.com.ar",
                Password = "Q722rtg3",
                Token = ""
            };

            await _unitWork.Usuarios.EnviarEmail(emailViewModel);

            return Json(new { cantidad = avisos.Count - 1,resultado = "Ok" } );
        }
    }
}
