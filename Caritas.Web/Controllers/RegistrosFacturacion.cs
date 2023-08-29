using AutoMapper;
using Caritas.Insfrastructure.Model;
using Caritas.Insfrastructure.Models;
using Caritas.Web.DTOs;
using Caritas.Web.Extensions;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.ModelsTable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Caritas.Web.Controllers
{
    [Authorize(Roles = "Administrador,Usuario,Empleado")]
    public class RegistrosFacturacion : Controller
    {
        private readonly IUnitOfWork _unitWork;
        private readonly IAuthorizationService _authorizationService;
        IMapper _mapper;

        public RegistrosFacturacion(IUnitOfWork unitWork, IAuthorizationService authorizationService, IMapper mapper)
        {
            _unitWork = unitWork;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }


        [Breadcrumb("Resultados")]
        [Breadcrumb("Listado de Resultados de Envios")]
        public async Task<IActionResult> Index()
        {
            bool bCreate, bEdit, bDelete = false;

            var claimsCreateMaestros = await _authorizationService.AuthorizeAsync(User, "CreateAdministracion");
            bCreate = claimsCreateMaestros.Succeeded ? true : false;
            var claimsEditMaestros = await _authorizationService.AuthorizeAsync(User, "EditAdministracion");
            bEdit = claimsEditMaestros.Succeeded ? true : false;
            var claimsDeleteMaestros = await _authorizationService.AuthorizeAsync(User, "DeleteAdministracion");
            bDelete = claimsDeleteMaestros.Succeeded ? true : false;

            var model = new TableUIExt()
            {
                Metodo = "POST",
                CreateUrl = "RegistrosFacturacion",
                IsOnPermisoCreate = false,
                IsServerSide = "true",
                IsOnModalCreate = false,
                WidthAcciones = "15%",
                ViewOrder = new OrderUI[] {
                    new OrderUI() { Order = 1 },
                },
                Dom = "Bfrtip",
                ApiUrl = "/RegistrosFacturacion/GetAll",
                Fields = new FieldUI[] {
                    new FieldUI() { Label = "#", Data = "id", ColumWidth = "5%" },
                    new FieldUI() { Label = "Cliente", Data = "cliente", ColumWidth = "30%" },
                    new FieldUI() { Label = "Email", Data = "email", ColumWidth = "30%" },
                    new FieldUI() { Label = "Resultado", Data = "result", ColumWidth = "20%" },
                },
                Methods = new MethodsUI[] {
                //    new MethodsUI() {Icono="fa fa-eye",titulo="Detalles",Url="/Clientes/Detail",Clase="btn btn-info btn-sm" },
                //    new MethodsUI() {Icono="fa fa-pencil",titulo="Editar",Url="/Clientes/Edit",Clase="btn btn-primary btn-sm",IsOnModal=false,Permiso = bEdit },
                    new MethodsUI() {Icono="fa fa-trash",titulo="Borrar",Url="/RegistrosFacturacion/Delete",Clase="btn btn-danger btn-sm",Permiso = bDelete },
                }
            };

            ViewBag.titulo = "Listado de Resultados";
            ViewData["Title"] = "Email Enviados";
            ViewData["TableTitle"] = "Listado de Email Enviados";

            return View("~/Views/Shared/_TableViewExt.cshtml", model);
        }

        [HttpPost]
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

            var queryRegistro = await _unitWork.Repository<Resultado>().GetAsync(null, null, "", true);

            // Total de registros antes de filtrar.
            int TotalRegistros = queryRegistro.Count();

            var queryRegis = queryRegistro
                  .Where(e => string.Concat(e.Id, e.Cliente, e.Email).Contains(ValorBuscado));

            // Total de registros ya filtrados.
            int TotalRegistrosFiltrados = queryRegis.Count();

            switch (sortColumn)
            {
                case "id":
                    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) queryRegis = queryRegis.OrderBy(c => c.Id);
                    break;
                case "cliente":
                    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) queryRegis = queryRegis.OrderBy(c => c.Cliente);
                    break;
                case "email":
                    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) queryRegis = queryRegis.OrderBy(c => c.Email);
                    break;
                default:
                    break;
            }


            var allObj = queryRegis.Skip(OmitirRegistros).Take(CantidadRegistros);

            List<ResultadoDto> resultados = _mapper.Map<List<ResultadoDto>>(allObj);

            return Json(new
            {
                draw = NroPeticion,
                recordsTotal = TotalRegistros,
                recordsFiltered = TotalRegistrosFiltrados,
                data = resultados
            }); 

        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            ViewBag.Title = "Eliminar email enviado";

            var allObj = await _unitWork.Repository<Resultado>().GetByIdAsync(id);

            return PartialView(allObj);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmedAsync(Resultado model)
        {
            var objFromDb = await _unitWork.Repository<Resultado>().GetByIdAsync(model.Id);
            if (objFromDb == null)
            {
                return Json(new { success = false, msg = "Registro no encontrado" });
            }


            if (objFromDb == null)
            {
                return Json(new { success = true, msg = "Registro no encontrado" });
            }

            await _unitWork.Repository<Resultado>().DeleteAsync(objFromDb);

            return Json(new { success = true, msg = "Registro Eliminado con Exito" });

        }
    }
}
