using Caritas.Web.DTOs;
using Caritas.Web.Extensions;
using DsCommon;
using DsCommon.Enums;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.ModelsApi;
using DsCommon.ModelsTable;
using DsCommon.ModelsView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Caritas.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IUnitOfWork _unitWork;

        public RolesController(IUnitOfWork unitWork)
        {
            _unitWork = unitWork;
        }


        [Breadcrumb("Roles")]
        [Breadcrumb("Lista de Roles")]
        [Authorize(Roles = "Usuario")]
        public IActionResult Index()
        {
            var model = new TableUIExt()
            {
                Metodo = "GET",
                IsServerSide = "false",
                CreateUrl = "Roles",                                
                IsOnModalCreate = true,
                IsCreateAllow = false,
                WidthAcciones = "20%",
                ViewOrder = new OrderUI[] {
                    new OrderUI() { Order = 1 },
                },                
                Dom = "Bfrtip",
                ApiUrl = "/Roles/GetAll",
                Fields = new FieldUI[] {
                new FieldUI() { Label = "Id", Data = "id", ColumWidth = "30%" },
                    new FieldUI() { Label = "Nombre", Data = "name", ColumWidth = "50%" },
                    new FieldUI() { Label = "Estado", Data = "status", ColumWidth = "10%" }
                },
                Methods = new MethodsUI[] {
                    new MethodsUI() {Icono="fa fa-eye",titulo="Permisos",Url="/Roles/Permisos",Clase="btn btn-info btn-sm" },
                }
            };
            ViewData["Title"] = "Roles";
            ViewData["TableTitle"] = "Listado de Roles";

            return View("~/Views/Shared/_TableViewExt.cshtml", model);
        }

            [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            var companiaId = SDRutas.CompaniaId;

            var allObj = await _unitWork.Roles.GetAllRolAsync<List<RoleViewModel>>(accessToken,Convert.ToInt32(companiaId));

            foreach (var item in allObj)
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

            return Json(new { data = allObj });

        }

        public ActionResult Create()
        {
            ViewBag.Title = "Nuevo Rol";

            RoleDto rol = new RoleDto();
            return PartialView(rol);
        }


        [HttpPost]
        public async Task<ActionResult> Create(RoleDto model)
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            var companiaId = SDRutas.CompaniaId;

            if (ModelState.IsValid)
            {
                if (await _unitWork.Roles.CheckRoleAsync(model.Name,Convert.ToInt32(companiaId),accessToken))
                  {
                      return Ok(new { status = false, msg = "El Rol Existe" });
                  }
                  else
                 {
                    var rol = new RoleViewModel()
                    {
                       Name = model.Name,
                       CompaniaId = Convert.ToInt32(companiaId),
                    };

                    model.Estado = (EstadoEnum)Enum.Parse(typeof(EstadoEnum), model.Status);
                    var result = await _unitWork.Roles.CreateRol<ResponseDto>(rol,accessToken);
                    if (!result.success)
                    {                       
                        return Ok(new { success = false, msg = result.msg });
                    }

                   return Ok(new { success = true, msg = "Rol Creado con Exito" });
                }

            }
            else
            {
                ViewBag.Title = "Nuevo Rol";
                return PartialView(model);
            }
        }
    }
}
