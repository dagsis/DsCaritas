using AutoMapper;
using Caritas.Insfrastructure.Models;
using Caritas.Web.DTOs;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.ModelsTable;
using DsCommon.ModelsView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Caritas.Web.Controllers
{
    [Authorize(Roles = "Administrador,Usuario,Empleado")]
    public class AvisoController : Controller
    {
        private readonly IUnitOfWork _unitWork;
        private readonly IAuthorizationService _authorizationService;
        IMapper _mapper;
        private int contador = 0;

        public AvisoController(IUnitOfWork unitWork, IAuthorizationService authorizationService, IMapper mapper)
        {
            _unitWork = unitWork;
            _authorizationService = authorizationService;
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

            var queryRegistro = await _unitWork.Repository<Aviso>().GetAsync(null, null, "", true);

            var queryRegis = queryRegistro.Where(e => string.Concat(e.Id).Contains(""));

            queryRegis = queryRegis.OrderBy(c => c.Cliente);
            return Json(queryRegis);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<JsonResult> Enviar(int cliente)
        {

           
            return Json(cliente);
        }
    }
}
