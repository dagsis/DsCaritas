using AutoMapper;
using Caritas.Insfrastructure.Model;
using Caritas.Insfrastructure.Models;
using Caritas.Web.DTOs;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.ModelsTable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;

namespace Caritas.Web.Controllers
{
    [Authorize(Roles = "Administrador,Usuario,Empleado")]
    public class CalendarioController : Controller
    {
        private readonly IUnitOfWork _unitWork;
        private readonly IAuthorizationService _authorizationService;
        IMapper _mapper;

        public CalendarioController(IUnitOfWork unitWork, IAuthorizationService authorizationService, IMapper mapper)
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
                CreateUrl = "Calendario",
                IsOnPermisoCreate = bCreate,
                IsServerSide = "true",
                IsOnModalCreate = false,
                WidthAcciones = "15%",
                ViewOrder = new OrderUI[] {
                    new OrderUI() { Order = 1 },
                },
                Dom = "Bfrtip",
                ApiUrl = "/Calendario/GetAll",
                Fields = new FieldUI[] {
                    new FieldUI() { Label = "Id", Data = "id", ColumWidth = "5%" },
                    new FieldUI() { Label = "Período", Data = "periodo", ColumWidth = "35%" },
                    new FieldUI() { Label = "Cuota", Data = "cuota", ColumWidth = "10%" },
                    new FieldUI() { Label = "Año", Data = "anio", ColumWidth = "10%" },
                    new FieldUI() { Label = "Vencimiento", Data = "vencimiento", ColumWidth = "10%",Tipo="D",ColumnaNumber =4  },

                },
                Methods = new MethodsUI[] {
                    //new MethodsUI() {Icono="fa fa-eye",titulo="Detalles",Url="/Calendario/Detail",Clase="btn btn-info btn-sm" },
                    new MethodsUI() {Icono="fa fa-pencil",titulo="Editar",Url="/Calendario/Edit",Clase="btn btn-primary btn-sm",IsOnModal=false,Permiso = bEdit },
                    new MethodsUI() {Icono="fa fa-trash",titulo="Borrar",Url="/Calendario/Delete",Clase="btn btn-danger btn-sm",Permiso = bDelete },
                }
            };

            ViewBag.titulo = "Listado de Cuotas";
            ViewData["Title"] = "Calendario";
            ViewData["TableTitle"] = "Listado de Cuotas";

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

            var queryRegistro = await _unitWork.Repository<Calendario>().GetAsync(null, null, "", true);

          //  Total de registros antes de filtrar.
            int TotalRegistros = queryRegistro.Count();

            var queryRegis = queryRegistro
                  .Where(e => string.Concat(e.Id, e.Periodo, e.Anio, e.Cuota).Contains(ValorBuscado));

            // Total de registros ya filtrados.
            int TotalRegistrosFiltrados = queryRegis.Count();

            switch (sortColumn)
            {
                case "año":
                    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) queryRegis = queryRegis.OrderBy(c => c.Anio);
                    break;
                case "cuota":
                    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) queryRegis = queryRegis.OrderBy(c => c.Cuota);
                    break;
                default:
                    break;
            }


            var allObj = queryRegis.Skip(OmitirRegistros).Take(CantidadRegistros);

            List<CalendarioDto> calendarios = _mapper.Map<List<CalendarioDto>>(allObj);

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
                data = calendarios
            });

        }
    }
}
