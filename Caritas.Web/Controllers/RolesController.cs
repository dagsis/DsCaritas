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
            //var model = new TableUIExt()
            //{
            //    Metodo = "GET",
            //    IsServerSide = "false",
            //    CreateUrl = "Roles",                                
            //    IsOnModalCreate = true,
            //    IsCreateAllow = false,
            //    WidthAcciones = "20%",
            //    ViewOrder = new OrderUI[] {
            //        new OrderUI() { Order = 1 },
            //    },                
            //    Dom = "Bfrtip",
            //    ApiUrl = "/Roles/GetAll",
            //    Fields = new FieldUI[] {
            //    new FieldUI() { Label = "Id", Data = "id", ColumWidth = "30%" },
            //        new FieldUI() { Label = "Nombre", Data = "name", ColumWidth = "50%" },
            //        new FieldUI() { Label = "Estado", Data = "status", ColumWidth = "10%" }
            //    },
            //    Methods = new MethodsUI[] {
            //        new MethodsUI() {Icono="fa fa-eye",titulo="Permisos",Url="/Roles/Permisos",Clase="btn btn-info btn-sm" },
            //    }
            //};
            //ViewData["Title"] = "Roles";
            //ViewData["TableTitle"] = "Listado de Roles";

            //return View("~/Views/Shared/_TableViewExt.cshtml", model);

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

            var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            var modulos = await _unitWork.Roles.GetModuleRoleAsync<List<ModulosRoleViewModel>>(accessToken, Convert.ToInt32(SDRutas.AplicacionId), userId);

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
                    sTabla += "<td>" + item.Codigo + "<input type ='hidden' name = 'modulos[" + item.Id + "][idmodulo]' value = '" + item.Id + "' required /> </td>";
                    sTabla += "<td>" + item.Titulo + "<input type ='hidden' name = 'user[" + item.Id + "][iduser]' value = '" + item.Permisos.ApplicationModuloUserId + "' required /> </td>";
                    sTabla += "<td><div class='toggle-flip'><label><input type='checkbox' name='modulos[" + item.Id + "][r]'" + rCheck + "/><span class='flip-indecator' data-toggle-on='ON' data-toggle-off='OFF'></span></label></div></td>";
                    sTabla += "<td><div class='toggle-flip'><label><input type='checkbox' name='modulos[" + item.Id + "][w]'" + wCheck + "/><span class='flip-indecator' data-toggle-on='ON' data-toggle-off='OFF'></span></label></div></td>";
                    sTabla += "<td><div class='toggle-flip'><label><input type='checkbox' name='modulos[" + item.Id + "][u]'" + uCheck + "/><span class='flip-indecator' data-toggle-on='ON' data-toggle-off='OFF'></span></label></div></td>";
                    sTabla += "<td><div class='toggle-flip'><label><input type='checkbox' name='modulos[" + item.Id + "][d]'" + dCheck + "/><span class='flip-indecator' data-toggle-on='ON' data-toggle-off='OFF'></span></label></div></td>";
                    sTabla += "</tr>";



                    no += 1;
                }
            }



            return Ok(sTabla);
        }

        [HttpPost]
        public async Task<IActionResult> SetPermisos(IFormCollection formdata)
        {
            string usuario = formdata["user[1][iduser]"];

            // var role = _db.ApplicationRols.FirstOrDefault(r => r.Id == usuario);

            var mod = formdata.Where(e => e.Key.Contains("modulos")).ToList();

            //var user = await userManager.FindByNameAsync(User.Identity.Name);
            //var claims = await userManager.GetClaimsAsync(user);
            //var result = await userManager.RemoveClaimsAsync(user, claims);

            //claims = await _userHelper.GetRoleClaimsAsync(role);

            //foreach (var item in claims)
            //{
            //    await roleManager.RemoveClaimAsync(role, item);
            //}


            for (int i = 1; i <= mod.Count(); i++)
            {
                var a = mod.Where(e => e.Key.Contains("modulos[" + i + "]")).ToList();
                if (a.Count() > 0)
                {
                    var idModulo = a[0].Value;

                    var r = a.Where(e => e.Key.Contains("[r]"));
                    string rRead = r.Count() > 0 ? "1" : "0";

                    var w = a.Where(e => e.Key.Contains("[w]"));
                    string wWrite = w.Count() > 0 ? "1" : "0";

                    var u = a.Where(e => e.Key.Contains("[u]"));
                    string uUpdate = u.Count() > 0 ? "1" : "0";

                    var d = a.Where(e => e.Key.Contains("[d]"));
                    string dDelete = d.Count() > 0 ? "1" : "0";

                    //try
                    //{
                    //    var permisoDel = _db.ApplicationPermisos.Where(r => r.ApplicationModuloId == i && r.ApplicationRolId == rol).ToList();

                    //    _db.ApplicationPermisos.RemoveRange(permisoDel);

                    //    var permisoAdd = new ApplicationPermisos()
                    //    {
                    //        ApplicationRolId = rol,
                    //        ApplicationModuloId = Convert.ToInt32(idModulo),
                    //        R = rRead,
                    //        W = wWrite,
                    //        U = uUpdate,
                    //        D = dDelete
                    //    };


                    //    switch (permisoAdd.ApplicationModuloId)
                    //    {
                    //        case 1:
                    //            await this.roleManager.AddClaimAsync(role, new Claim("ViewDashBoard", permisoAdd.R == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("CreateDashBoard", permisoAdd.W == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("UpdateDashBoard", permisoAdd.U == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("DeleteDashBoard", permisoAdd.D == "1" ? "True" : "False"));
                    //            break;
                    //        case 2:
                    //            await this.roleManager.AddClaimAsync(role, new Claim("ViewUsuarios", permisoAdd.R == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("CreateUsuario", permisoAdd.W == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("UpdateUsuario", permisoAdd.U == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("DeleteUsuario", permisoAdd.D == "1" ? "True" : "False"));
                    //            break;
                    //        case 3:
                    //            await this.roleManager.AddClaimAsync(role, new Claim("ViewClientes", permisoAdd.R == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("CreateCliente", permisoAdd.W == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("UpdateCliente", permisoAdd.U == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("DeleteCliente", permisoAdd.D == "1" ? "True" : "False"));
                    //            break;
                    //        case 4:
                    //            await this.roleManager.AddClaimAsync(role, new Claim("ViewProductos", permisoAdd.R == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("CreateProducto", permisoAdd.W == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("UpdateProducto", permisoAdd.U == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("DeleteProducto", permisoAdd.D == "1" ? "True" : "False"));
                    //            break;
                    //        case 5:
                    //            await this.roleManager.AddClaimAsync(role, new Claim("ViewCategorias", permisoAdd.R == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("CreateCategoria", permisoAdd.W == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("UpdateCategoria", permisoAdd.U == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("DeleteCategoria", permisoAdd.D == "1" ? "True" : "False"));
                    //            break;
                    //        case 6:
                    //            await this.roleManager.AddClaimAsync(role, new Claim("ViewPedidos", permisoAdd.R == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("CreatePedido", permisoAdd.W == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("UpdatePedido", permisoAdd.U == "1" ? "True" : "False"));
                    //            await this.roleManager.AddClaimAsync(role, new Claim("DeletePedido", permisoAdd.D == "1" ? "True" : "False"));
                    //            break;
                    //        default:
                    //            break;
                    //    }

                    //    _db.ApplicationPermisos.Add(permisoAdd);

                    //}
                    //catch (Exception ex)
                    //{

                    //    return Json(new { status = false, msg = ex.Message });
                    //}

                }

            }

            try
            {
                // _db.SaveChanges();

            }
            catch (Exception ex)
            {

                return Json(new { status = false, msg = ex.Message });
            }


            return Json(new { status = true, msg = "Permisos Asignados Correctamente" });
        }
    }
}
