using AutoMapper;
using Caritas.Insfrastructure.Model;
using Caritas.Web.DTOs;
using Caritas.Web.Extensions;
using DsCommon.Enums;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.ModelsTable;
using DsCommon.ModelsView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Caritas.Web.Controllers
{
    [Authorize(Roles = "Administrador,Usuario,Empleado")]
    public class ClientesController : Controller
    {
        private readonly IUnitOfWork _unitWork;
        IMapper _mapper;

        public ClientesController(IUnitOfWork unitWork, IMapper mapper)
        {
            _unitWork = unitWork;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var model = new TableUIExt()
            {
                Metodo="POST",               
                CreateUrl = "Clientes",
                IsServerSide = "true",
                IsOnModalCreate = false,
                WidthAcciones = "15%",
                ViewOrder = new OrderUI[] {
                    new OrderUI() { Order = 1 },
                },
                Dom = "Bfrtip",
                ApiUrl = "/Clientes/GetAll",
                Fields = new FieldUI[] {
                    new FieldUI() { Label = "Código", Data = "id", ColumWidth = "5%" },
                    new FieldUI() { Label = "Apellido y Nombre", Data = "fullName", ColumWidth = "15%" },
                    new FieldUI() { Label = "Domicilio", Data = "domicilio", ColumWidth = "15%" },
                    new FieldUI() { Label = "Localidad", Data = "localidad", ColumWidth = "10%" },
                    new FieldUI() { Label = "Documento", Data = "documento", ColumWidth = "10%" },
                    new FieldUI() { Label = "Teléfono", Data = "telefono", ColumWidth = "12%" },
                    new FieldUI() { Label = "P.Elect.", Data = "dest", ColumWidth = "5%" },
                    new FieldUI() { Label = "Estado", Data = "status",ColumWidth = "5%"},
                },
                Methods = new MethodsUI[] {
                    new MethodsUI() {Icono="fa fa-eye",titulo="Detalles",Url="/Clientes/Detail",Clase="btn btn-info btn-sm" },
                    new MethodsUI() {Icono="fa fa-pencil",titulo="Editar",Url="/Clientes/Edit",Clase="btn btn-primary btn-sm",IsOnModal=false },
                    new MethodsUI() {Icono="fa fa-trash",titulo="Borrar",Url="/Clientes/Delete",Clase="btn btn-danger btn-sm" },
                }
            };

            ViewBag.titulo = "Listado de Clientes";
            ViewData["Title"] = "Clientes";
            ViewData["TableTitle"] = "Listado de Clientes";

            return View("~/Views/Shared/_TableViewExt.cshtml", model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Usuario,Empleado")]
        public async Task<IActionResult> Detail(int id)
        {

            ViewBag.Title = "Detalle de Cliente";

            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            var model = await _unitWork.Repository<Cliente>().GetByIdAsync(id); ;

            return PartialView(model);
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

            var queryRegistro = await _unitWork.Repository<Cliente>().GetAsync(null,null,"",true); 

            // Total de registros antes de filtrar.
            int TotalRegistros = queryRegistro.Count();

            var queryRegis = queryRegistro
                  .Where(e => string.Concat(e.Id, e.FullName.ToLower(), e.Localidad.ToLower(), e.Domicilio.ToLower()).Contains(ValorBuscado));

            // Total de registros ya filtrados.
            int TotalRegistrosFiltrados = queryRegis.Count();

            switch (sortColumn)
            {
                case "codigo":
                    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) queryRegis = queryRegis.OrderBy(c => c.Id);
                    break;
                case "fullName":
                    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) queryRegis = queryRegis.OrderBy(c => c.FullName);
                    break;
                default:
                    break;
            }


            var allObj = queryRegis.Skip(OmitirRegistros).Take(CantidadRegistros);

            List<ClienteDto> clientes = _mapper.Map<List<ClienteDto>>(allObj);

            foreach (var item in clientes)
            {
                if ((int)item.Estado == 1)
                {
                    item.Status = "<span class='badge rounded-pill bg-success'>Activo</span>";
                }
                else
                {
                    item.Status = "<span class='badge rounded-pill bg-danger'>Inactivo</span>";
                }
            }

            return Json(new
            {
                draw = NroPeticion,
                recordsTotal = TotalRegistros,
                recordsFiltered = TotalRegistrosFiltrados,
                data = clientes
            });

        }
       
        public ActionResult Create()
        {
            ViewBag.Title = "Nuevo Cliente";

            var model = new ClienteDto();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ClienteDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Estado = (EstadoEnum)Enum.Parse(typeof(EstadoEnum), model.Status);

                    var cliente = _mapper.Map<Cliente>(model);

                    await _unitWork.Repository<Cliente>().AddAsync(cliente);

                    TempData["SuccessMessage"] = "Registro Agregado Correctamente";
                    return RedirectToAction("Index", "Clientes");
                }
                catch (Exception e)
                {
                    TempData["SuccessMessage"] = e.Message;
                    return RedirectToAction("Index", "Clientes");
                }
            }
            else
            {
                ViewBag.Title = "Nueva Aplicación";

                return PartialView(model);
            }
        }


        [HttpGet]
        [Breadcrumb("Clientes")]
        [Breadcrumb("Editar Cliente")]
        public async Task<IActionResult> Edit(int id)
        {

            ViewBag.Title = "Editar Cliente";

            var allObj = await _unitWork.Repository<Cliente>().GetByIdAsync(id);

            ClienteDto model = _mapper.Map<ClienteDto>(allObj);

            model.Status = Convert.ToString((int)allObj.Estado);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClienteDto model)
        {

            if (ModelState.IsValid)
            {
                model.Estado = (EstadoEnum)Enum.Parse(typeof(EstadoEnum), model.Status);

                var cliente =_mapper.Map<Cliente>(model);

                await _unitWork.Repository<Cliente>().UpdateAsync(cliente);

                TempData["SuccessMessage"] = "Registro Actualizado Correctamente";
                return RedirectToAction("Index", "Clientes");
            }
            else
            {               
                ViewBag.Title = "Editar Cliente";
                return View(model);
            }

        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            ViewBag.Title = "Eliminar Cliente";

            var allObj = await _unitWork.Repository<Cliente>().GetByIdAsync(id);

            return PartialView(allObj);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmedAsync(Cliente model)
        {
            var objFromDb = await _unitWork.Repository<Cliente>().GetByIdAsync(model.Id);
            if (objFromDb == null)
            {
                return Json(new { success = false, msg = "Registro no encontrado" });
            }


            if (objFromDb == null)
            {
                return Json(new { success = true, msg = "Registro no encontrado" });
            }

            await _unitWork.Repository<Cliente>().DeleteAsync(objFromDb);

            return Json(new { success = true, msg = "Registro Eliminado con Exito" });

        }


    }
}
