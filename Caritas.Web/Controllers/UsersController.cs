
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
            //var model = new TableUIExt()
            //{
            //    Metodo = "GET",
            //    IsServerSide = "false",
            //    CreateUrl = "Users",
            //    IsOnModalCreate = false,
            //    WidthAcciones = "15%",
            //    ViewOrder = new OrderUI[] {
            //        new OrderUI() { Order = 2 },
            //    },
            //    Dom = "Bfrtip",
            //    ApiUrl = "/Users/GetAll",
            //    Fields = new FieldUI[] {
            //        new FieldUI() { Label = "Usuario", Data = "email", ColumWidth = "10%" },
            //        new FieldUI() { Label = "Nombre", Data = "nombre", ColumWidth = "30%" },
            //        new FieldUI() { Label = "Teléfono", Data = "phoneNumber", ColumWidth = "10%" },
            //        new FieldUI() { Label = "Rol Principal", Data = "role", ColumWidth = "10%" },
            //        new FieldUI() { Label = "Status", Data = "status", ColumWidth = "10%" },
            //    },
            //    Methods = new MethodsUI[] {
            //        new MethodsUI() {Icono="fa fa-eye",titulo="Permisos",Url="/Users/Permisos",Clase="btn btn-info btn-sm" },
            //        new MethodsUI() {Icono="fa fa-eye",titulo="Detalles",Url="/Users/Detail",Clase="btn btn-info btn-sm" },
            //        new MethodsUI() {Icono="fa fa-pencil",titulo="Editar",Url="/Users/Edit",Clase="btn btn-primary btn-sm",IsOnModal=false },
            //        new MethodsUI() {Icono="fa fa-trash",titulo="Borrar",Url="/Users/Delete",Clase="btn btn-danger btn-sm" },
            //    }
            //};

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
            var modulos = await _unitWork.Usuarios.GetModuleUserAsync<List<ModulosUserViewModel>>(accessToken,Convert.ToInt32(SDRutas.CompaniaId) ,userId);

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
                    sTabla += "<td>" + item.Id + "<input type ='hidden' name = 'modulos[" + item.Id + "][idmodulo]' value = '" + item.Id + "' required /> </td>";
                    sTabla += "<td>" + item.Titulo + "<input type ='hidden' name = 'rol[" + item.Id + "][idrol]' value = '" + item.Permisos.ApplicationModuloUserId + "' required /> </td>";
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
    }
}
