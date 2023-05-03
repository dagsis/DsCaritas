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
using Newtonsoft.Json;
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
            ViewBag.titulo = "Listado de Roles";
            ViewData["Title"] = "Roles";
            ViewData["TableTitle"] = "Listado de Roles";

            TempData["page_function_js"] = "/js/functions/function_role.js";
            return View();
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

        [HttpGet]
        public async Task<IActionResult> Permisos(string id)
        {

            var roleId = id;
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            var modulos = await _unitWork.Roles.GetModuleRoleAsync<List<ModulosRoleViewModel>>(accessToken, Convert.ToInt32(SDRutas.AplicacionId), roleId);

            string sTabla = "";

            if (modulos != null)
            {
                int no = 0;
                foreach (var item in modulos)
                {
                    string rCheck = item.Permisos.R == "1" ? " checked" : "";
                    string wCheck = item.Permisos.W == "1" ? " checked" : "";
                    string uCheck = item.Permisos.U == "1" ? " checked" : "";
                    string dCheck = item.Permisos.D == "1" ? " checked" : "";

                    sTabla += "<tr>";
                    sTabla += "<td>" + item.Codigo + "<input type ='hidden' name = 'modulos[" + item.Codigo + "][idmodulo]' value = '" + item.Codigo + "' required /> </td>";
                    sTabla += "<td>" + item.Titulo + "<input type ='hidden' name = 'role[" + item.Codigo + "][idrole]' value = '" + item.Permisos.ApplicationRoleId + "' required /> </td>";
                    sTabla += "<td><div class='toggle-flip'><label><input type='checkbox' name='modulos[" + item.Codigo + "][r]'" + rCheck + "/><span class='flip-indecator' data-toggle-on='ON' data-toggle-off='OFF'></span></label></div></td>";
                    sTabla += "<td><div class='toggle-flip'><label><input type='checkbox' name='modulos[" + item.Codigo + "][w]'" + wCheck + "/><span class='flip-indecator' data-toggle-on='ON' data-toggle-off='OFF'></span></label></div></td>";
                    sTabla += "<td><div class='toggle-flip'><label><input type='checkbox' name='modulos[" + item.Codigo + "][u]'" + uCheck + "/><span class='flip-indecator' data-toggle-on='ON' data-toggle-off='OFF'></span></label></div></td>";
                    sTabla += "<td><div class='toggle-flip'><label><input type='checkbox' name='modulos[" + item.Codigo + "][d]'" + dCheck + "/><span class='flip-indecator' data-toggle-on='ON' data-toggle-off='OFF'></span></label></div></td>";
                    sTabla += "</tr>";



                    no += 1;
                }
            }



            return Ok(sTabla);
        }

        [HttpPost]
        public async Task<IActionResult> SetPermisos(IFormCollection formdata)
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            string role = formdata["role[1][idrole]"];

            var mod = formdata.Where(e => e.Key.Contains("modulos")).ToList();

            var valor = JsonConvert.SerializeObject(mod);

            var valorPermiso = new ValorRoleviewModel()
            {
                RoleId = role,
                Values = valor
            };

            var result = await _unitWork.Roles.SetModuleRoleAsync<ResponseDto>(valorPermiso, accessToken);

            if (result.success)
            {
                return Json(new { status = true, msg = "Permisos Asignados Correctamente" });
            }

            return Json(new { status = true, msg = "Permisos Asignados Correctamente" });
                   
        }
    }
}
