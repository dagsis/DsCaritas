
using Azure.Core;
using Caritas.Insfrastructure.Helpers;
using Caritas.Insfrastructure.Model;
using Caritas.Web.DTOs;
using Caritas.Web.Extensions;
using DsCommon;
using DsCommon.Enums;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.ModelsApi;
using DsCommon.ModelsTable;
using DsCommon.ModelsView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Data;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Caritas.Web.Controllers
{

    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitWork;

        public UsersController(IUnitOfWork unitWork)
        {
            _unitWork = unitWork;
        }

        [Breadcrumb("Usuarios")]
        [Breadcrumb("Usuarios")]
        [Authorize(Roles = "Administrador,Usuario")]
        public IActionResult Index()
        {          
            ViewBag.titulo = "Listado de Usuarios";
            ViewData["Title"] = "Usuarios";
            ViewData["TableTitle"] = "Listado de Usuarios";

            TempData["page_function_js"] = "/js/functions/function_user.js";

            return View();       
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            var compania = HttpContext.User.Claims.First(c => c.Type == "compania").Value;
            var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var allObj = await _unitWork.Usuarios.GetAllUserAsync<List<UserViewModel>>(accessToken, Convert.ToInt32(compania), userId);

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

            return Json(new { data = allObj.Where(r => r.Role != "Administrador") });

        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<IActionResult> Detail(string id)
        {

            ViewBag.Title = "Detalle de Usuario";

            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            var user = await _unitWork.Usuarios.GetUserByAsync<UserViewModel>(id, accessToken);

            return PartialView(user);
        }

        [Breadcrumb("Usuarios")]
        [Breadcrumb("Editar Perfil")]
        [Authorize(Roles = "Administrador,Usuario,Empleado")]
        public async Task<ActionResult> PerfilEdit()
        {
            var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            var user = await _unitWork.Usuarios.GetUserByAsync<UserViewModel>(userId, accessToken);

            ViewBag.Title = "Perfil Usuario";

            UserEditPerfilDto model = new UserEditPerfilDto()
            {
                Id = userId,
                Email = user.Email,
                Nombre = user.Nombre,
                PhoneNumber = user.PhoneNumber,
                Direccion = user.Direccion,
                CPostal = user.CPostal,
                Ciudad = user.Ciudad,
                Provincia = user.Provincia,
                //  Role = user.Role,
                Password = null,
                ConfirmPassword = null,
            };

            TempData["page_function_js"] = "/js/functions/function_form.js";

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> PerfilEdit(UserViewModel model)
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;

            var result = await _unitWork.Usuarios.UpdateUser<ResponseDto>(model, accessToken);

            if (result.success)
            {
                return Json(new { status = "success", msg = "Perfil actualizado con éxito" });
            }
            else
            {
                return Json(new { status = "error", msg = result.msg });
            }

		}

        [Breadcrumb("Usuarios")]
        [Breadcrumb("Crear Usuario")]
        [Authorize(Roles = "Administrador,Usuario")]
        public ActionResult Create()
        {
            ViewBag.Title = "Agregar Usuario";
            var roles = Roles.TraerRoles();

            var selecList = new List<SelectListItem>();
            roles.ForEach(i => selecList.Add(new SelectListItem(i.Name, i.Id.ToString())));

            UserCreateDto model = new UserCreateDto()
            {
                Role = "Usuario",
                Roles = selecList,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto model)
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            var compania = HttpContext.User.Claims.First(c => c.Type == "compania").Value;

            var roles = Roles.TraerRoles();

            var selecList = new List<SelectListItem>();
            roles.ForEach(i => selecList.Add(new SelectListItem(i.Name, i.Id.ToString())));

            string msg;

            if (ModelState.IsValid)
            {
                var user = new UserViewModel()
                {
                    Nombre = model.Nombre,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Role = model.Role,
                    Password = "123456",
                    CompaniaId = Convert.ToInt32(compania),
                };

                var result = await _unitWork.Usuarios.CreateUser<ResponseDto>(user, accessToken);
                if (!result.success)
                {
                    msg = "Problemas para crear el registro";
                    return Ok(new { success = false, msg = msg });
                }

                TempData["SuccessMessage"] = "Usuario Agregado Correctamente";

                return RedirectToAction("Index", "Users");
            }
            else
            {

                model = new UserCreateDto()
                {
                    Role = "Usuario",
                    Roles = selecList
                };

                ViewBag.Title = "Agregar Usuario";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteUsers(string email)
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;

            var user = await _unitWork.Usuarios.GetUserByEmailAsync<UserViewModel>(email, accessToken);

            if (user.Email == email)
            {
                return Json($"El e-mail {email} ya existe");
            }

            return Json(true);
        }

        [Breadcrumb("Usuarios")]
        [Breadcrumb("Editar Usuario")]
        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<ActionResult> Edit(string id)
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;

            var roles = Roles.TraerRoles();
            var selecList = new List<SelectListItem>();
            roles.ForEach(i => selecList.Add(new SelectListItem(i.Name, i.Id.ToString())));

            ViewBag.Title = "Editar Usuario";

            var user = await  _unitWork.Usuarios.GetUserByAsync<UserViewModel>(id, accessToken);
            var model = new UserEditDto()
            {
                Email = user.Email,
                Nombre = user.Nombre,
                PhoneNumber = user.PhoneNumber,
                Status = Convert.ToString((int)user.Estado),
                Role = user.Role,
                Roles = selecList
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserEditDto model)
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;

            var roles = Roles.TraerRoles();
            var selecList = new List<SelectListItem>();
            roles.ForEach(i => selecList.Add(new SelectListItem(i.Name, i.Id.ToString())));

            if (ModelState.IsValid)
            {
                try
                {

                    var user = await _unitWork.Usuarios.GetUserByAsync<UserViewModel>(model.Id, accessToken);
                    
                    user.Id = model.Id;
                    user.Nombre = model.Nombre;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Estado = (EstadoEnum)Enum.Parse(typeof(EstadoEnum), model.Status);
                    user.Role = model.Role;

                    await _unitWork.Usuarios.UpdateUser<ResponseDto>(user, accessToken);


                    TempData["SuccessMessage"] = "Usuario Actualizado Correctamente";

                    return RedirectToAction("Index", "Users");
                }
                catch (Exception e)
                {
                    return Json(new { success = false, msg = e.Message });
                }
            }
            else
            {

                model = new UserEditDto()
                {
                    Role = "Usuario",
                    Roles = selecList
                };

                ViewBag.Title = "Editar Usuario";
                return View(model);
            }
        }

        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<ActionResult> Delete(string id)
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;

            ViewBag.Title = "Eliminar Usuario";

            var user = await _unitWork.Usuarios.GetUserByAsync<UserViewModel>(id, accessToken);

            return PartialView(user);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmedAsync(UserViewModel model)
        {
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;

            var objFromDb = await _unitWork.Usuarios.GetUserByAsync<UserViewModel>(model.Id, accessToken);

            if (objFromDb == null)
            {
                return Json(new { success = false, msg = "Usuario no encontrado" });
            }
           
            var result = await _unitWork.Usuarios.DeleteUserAsync<ResponseDto>(objFromDb.Id, accessToken);
            if (result.success)
            {
                return Json(new { success = true, msg = result.msg });
            }

            return Json(new { success = false, msg = result.msg });

        }

        [Authorize(Roles = "Administrador,Usuario,Empleado")]
        public ActionResult CambiarContraseña()
        {
            ViewBag.Title = "Cambiar Contraseña";

            var model = new ChangePasswordViewModel()
            {
              OldPassword="",
              NewPassword = "",
              ConfirmPassword = ""
            };

            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> CambiarContraseña(ChangePasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;

                    var result = await _unitWork.Usuarios.ChangePassword<ResponseDto>(model, accessToken, userId);
                    if (result.success)
                    {
                        return Ok(new { success = false, msg = result.msg });
                    }
                    return Json(new { success = false, msg = result.msg });
                }
                catch (Exception e)
                {
                    return Json(new { success = false, msg = e.Message });
                }
            }
            else
            {
                ViewBag.Title = "Cambiar Contraseña";
                return PartialView(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Permisos(string id)
        {

            var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var accessToken = HttpContext.User.Claims.First(c => c.Type == "access_token").Value;
            var modulos = await _unitWork.Usuarios.GetModuleUserAsync<List<ModulosUserViewModel>>(accessToken,Convert.ToInt32(SDRutas.AplicacionId) ,userId);

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
