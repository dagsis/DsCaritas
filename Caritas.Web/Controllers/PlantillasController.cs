using AutoMapper;
using Caritas.Insfrastructure.Model;
using Caritas.Insfrastructure.Models;
using Caritas.Web.DTOs;
using Caritas.Web.Extensions;
using Caritas.Web.Services;
using DsCommon.IUnitOfWorkPatern;
using DsCommon.ModelsTable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Net;
using System.Text;

namespace Caritas.Web.Controllers
{
    [Authorize(Roles = "Administrador,Usuario,Empleado")]
    public class PlantillasController : Controller
    {
        private readonly IUnitOfWork _unitWork;
        private readonly IAuthorizationService _authorizationService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IServiceManagement _serviceManagement;

        IMapper _mapper;
        private int contador = 0;

        public PlantillasController(IUnitOfWork unitWork,
               IAuthorizationService authorizationService,
               IWebHostEnvironment hostEnvironment,
               IMapper mapper,
               IServiceManagement serviceManagement)
        {
            _unitWork = unitWork;
            _authorizationService = authorizationService;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
            _serviceManagement = serviceManagement;
        }

        public IActionResult Index()
        {
            bool bCreate = true;
            bool bEdit = true;
            bool bDelete = true;

            var model = new TableUIExt()
            {
                Metodo = "POST",
                CreateUrl = "Plantillas",
                IsOnPermisoCreate = bCreate,
                IsServerSide = "true",
                IsOnModalCreate = false,
                WidthAcciones = "15%",
                ViewOrder = new OrderUI[] {
                    new OrderUI() { Order = 1 },
                },
                Dom = "Bfrtip",
                ApiUrl = "/Plantillas/GetAll",
                Fields = new FieldUI[] {
                    new FieldUI() { Label = "Id", Data = "id", ColumWidth = "5%" },
                    new FieldUI() { Label = "Descripción", Data = "descripcion", ColumWidth = "35%" },
                    new FieldUI() { Label = "Plantilla", Data = "plantilla", ColumWidth = "35%" },
                },
                Methods = new MethodsUI[] {
                    new MethodsUI() {Icono="fa fa-eye",titulo="Procesar",Url="/Plantillas/Detail",Clase="btn btn-info btn-sm",IsOnModal=false },
                 //   new MethodsUI() {Icono="fa fa-pencil",titulo="Parametros",Url="/Plantillas/Edit",Clase="btn btn-primary btn-sm",IsOnModal=false,Permiso = bEdit },
                    //new MethodsUI() {Icono="fa fa-trash",titulo="Borrar",Url="/Plantillas/Delete",Clase="btn btn-danger btn-sm",Permiso = bDelete },
                }
            };

            ViewBag.titulo = "Listado de Plantillas";
            ViewData["Title"] = "Plantillas";
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
        [Authorize(Roles = "Administrador,Usuario,Empleado")]
        public async Task<IActionResult> Detail(int id)
        {
            CalendarioDto calendario;

            var allObj = new object();
            string vista = "";
            switch (id)
            {
                case 1:
                    allObj = await _unitWork.Repository<Calendario>().GetByIdAsync(id);
                    ViewBag.Title = "Plantilla Email";
                    vista = "Detail";
                    calendario = _mapper.Map<CalendarioDto>(allObj);
                    break;
                case 2:
                    allObj = await _unitWork.Repository<Calendario>().GetByIdAsync(id);
                    ViewBag.Title = "Facturación";
                    vista = "Facturacion";
                    calendario = _mapper.Map<CalendarioDto>(allObj);
                    break;
                default:
                    calendario = _mapper.Map<CalendarioDto>(allObj);
                    break;
            }

            return View(vista, calendario);
        }



        [HttpGet]
        [Breadcrumb("Plantilas")]
        [Breadcrumb("Parametros")]
        public async Task<IActionResult> Edit(int id)
        {

            var allObj = new object();
            var calendario = new CalendarioDto();

            string vista = "";
            switch (id)
            {
                case 1:
                    allObj = await _unitWork.Repository<Calendario>().GetByIdAsync(id);
                    ViewBag.Title = "Parametros Avisos";
                    vista = "EditAvisos";
                    break;
                case 2:
                    allObj = await _unitWork.Repository<Calendario>().GetByIdAsync(id);
                    calendario = _mapper.Map<CalendarioDto>(allObj);
                    ViewBag.Title = "Parametros Facturación - Edición";
                    vista = "EditFacturacion";
                    break;
                default:
                    break;
            }

            return View(vista, calendario);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CalendarioDto model)
        {

            if (ModelState.IsValid)
            {
                switch (model.Id)
                {
                    case 1:
                        break;
                    case 2:
                        var calendario = _mapper.Map<Calendario>(model);
                        await _unitWork.Repository<Calendario>().UpdateAsync(calendario);
                        break;
                    default:
                        break;
                }
              
                TempData["SuccessMessage"] = "Registro Actualizado Correctamente";
                return RedirectToAction("Index", "Plantillas");
            }
            else
            {
                switch (model.Id)
                {
                    case 1:
                        break;
                    case 2:
                        ViewBag.Title = "Parametros Facturación - Edición";
                        break;
                    default:
                        break;
                }
               
                return View(model);
            }

        }


        [HttpPost]
        public async Task<JsonResult> GetText()
        {
            List<Aviso> listaFiltrada = new List<Aviso>();

            var aviso = await _unitWork.Repository<Aviso>().GetAsync(null, x => x.OrderBy(y => y.Cliente), "", true);



            //int cliente = 0;
            //foreach (var item in aviso)
            //{ 

            //    while (item.Cliente != cliente)
            //    {
            //        cliente = item.Cliente;
            //        var avisos = await _unitWork.Repository<Aviso>().GetAsync(x => x.Cliente == item.Cliente, null, "", true);                  
            //        var dataPdf = await DescargarPdf(avisos);
            //    }

            //}
            return Json(aviso);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Usuario")]
        public async Task<JsonResult> Enviar(int cliente)
        {
            var avisos = await _unitWork.Repository<Aviso>().GetAsync(x => x.Cliente == cliente, null, "", true);

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


            var enviar = await _serviceManagement.PostMail("carlos@dagsistemas.com.ar", cliente, nombre, subject, messageBody);
            return Json(new { cantidad = avisos.Count - 1, resultado = "Ok" });
        }


        [HttpPost]
        [Authorize(Roles = "Administrador,Usuario,Empleado")]
        public async Task<JsonResult> GetFacturacion(CalendarioDto model)
        {

            string decodificado = WebUtility.HtmlDecode(model.Observacion);

            var calobj = await _unitWork.Repository<Calendario>().GetByIdAsync(model.Id);

           
            calobj.Observacion = decodificado;

            await _unitWork.Repository<Calendario>().UpdateAsync(calobj);

            List<Aviso> listaFiltrada = new List<Aviso>();

              var aviso = await _unitWork.Repository<Aviso>().GetAsync(null, x => x.OrderBy(y => y.Cliente), "", true);

     //       var aviso = await _unitWork.Repository<Aviso>().GetAsync(x => x.Cliente == 6725, x => x.OrderBy(y => y.Cliente), "", true);

            int cliente = 0;
            foreach (var item in aviso)
            {

                while (item.Cliente != cliente)
                {
                    cliente = item.Cliente;
                    var avisos = await _unitWork.Repository<Aviso>().GetAsync(x => x.Cliente == item.Cliente, null, "", true);
                    var dataPdf = await DescargarPdf(avisos, model.Vencimiento.ToString("dd/MM/yyyy"), model.VencimientoProc.ToLongDateString(),model.FechaAPartir.ToString("dd/MM/yyyy"));
                }

            }
            return Json(aviso);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Usuario,Empleado")]
        public async Task<JsonResult> EnviarFactura(int cliente)
        {
            var calendario = await _unitWork.Repository<Calendario>().GetAsync(x => x.Id == 2);

            var avisos = await _unitWork.Repository<Aviso>().GetAsync(x => x.Cliente == cliente, null, "", true);

            var PathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                      + "templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                      + Path.DirectorySeparatorChar.ToString() + "Factura.html";


            var subject = $"Aviso Vencimiento - Panteon Ntra Sra de la Merced - {avisos[0].Cliente}";

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

            string emailTo = avisos[0].Email;
            string nombre = avisos[0].Nombre + ' ' + avisos[0].Apellido;
            string tipo = "";
            string codigo = avisos[0].CPagoElectronico;
            decimal importe = 0;

            foreach (var item in avisos)
            {
                tipo = tipo + (item.Tipo == "N" ? "Nicho " : "Urna ") + item.Nicho + " / " + item.Piso + "° desde " + item.FecDesde.ToString("dd/MM/yyyy") + " hasta " + item.FecHasta.ToString("dd/MM/yyyy")+ " importe a abonar:" + item.Importe.ToString("C2") + "<br>";
                importe = importe + item.Importe;
            }

            string messageBody = string.Format(HtmlBody,
                subject,
                nombre,
                tipo,
                importe.ToString("N2"),
                codigo,
                calendario[0].Observacion,
                avisos[0].Valor_Vencido,
                avisos[0].Valor_a_Vencer,
                calendario[0].FechaAPartir.ToString("dd/MM/yyyy"),
                avisos[0].SiguienteValor_Vencido,
                avisos[0].SiguienteValor_a_Vencer
                );

            // emailTo = "betobiancheri@gmail.com";
           // emailTo = "carlos@dagsistemas.com.ar";

            var enviar = await _serviceManagement.PostMail(emailTo, cliente, nombre, subject, messageBody);
            return Json(new { cantidad = avisos.Count - 1, resultado = "Ok" });
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Usuario,Empleado")]
        public async Task<IActionResult> DescargarPdf(IReadOnlyList<Aviso> model,string vencimiento,string proximo,string FechaAPartir)
        {
            //var cliente = await _unitWork.Repository<Cliente>().GetByIdAsync(model[0].Cliente);
            //if (cliente != null)
            //{
                var rutaImagen1 = Path.Combine(_hostEnvironment.WebRootPath, "assets/logoAviso1.png");
                var rutaFoster = Path.Combine(_hostEnvironment.WebRootPath, "assets/fosterAviso.png");

                byte[] imageData1 = System.IO.File.ReadAllBytes(rutaImagen1);
                byte[] imageData3 = System.IO.File.ReadAllBytes(rutaFoster);

                //  QuestPDF.Settings.License = LicenseType.Community;

                var data = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Margin(10);
                        // page content
                        page.Header().Row(row =>
                        {


                            //  row.ConstantItem(380).Height(100).Placeholder();
                            row.ConstantItem(380).Image(imageData1);
                            row.RelativeItem().Height(100).Column(col =>
                            {
                                col.Item().AlignRight().PaddingBottom(10).PaddingRight(10).Text("CONTIENE VENCIMIENTO").FontSize(11).SemiBold();
                                col.Item().AlignRight().PaddingRight(10).Text(model[0].Cliente + " - " + model[0].Nombre + ' ' + model[0].Apellido).FontSize(7);
                                col.Item().AlignRight().PaddingRight(10).Text(model[0].Domicilio).FontSize(7);
                                col.Item().AlignRight().PaddingRight(10).Text(" ").FontSize(7);
                                col.Item().AlignRight().PaddingRight(10).PaddingTop(5).Text("Buenos Aires, " + DateTime.Today.ToShortDateString()).FontSize(10);
                            });
                        });

                        page.Content().Row(row =>
                        {
                            row.RelativeItem().PaddingTop(20).PaddingLeft(40).PaddingRight(40).Column(col =>
                            {
                                col.Item().AlignCenter().PaddingBottom(3).Text("INFORME DE VENCIMIENTOS").SemiBold();
                                col.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(col =>
                                    {
                                        col.RelativeColumn(5);
                                        col.RelativeColumn();
                                    });
                                    table.Header(header =>
                                    {
                                        header.Cell().Border(1).AlignCenter().Padding(2).Text("C o n c e p t o").SemiBold();
                                        header.Cell().Border(1).AlignCenter().Padding(2).Text("Importe").SemiBold();
                                    });

                                    decimal total = 0;
                                    string tipo = "";
                                    foreach (var item in model)
                                    {
                                        var precio = item.Importe;
                                        total += precio;

                                        tipo = (item.Tipo == "N" ? "Nicho " : "Urna ") + item.Nicho + " / " + item.Piso + "° desde " + item.FecDesde.ToString("dd/MM/yyyy") + " hasta " + item.FecHasta.ToString("dd/MM/yyyy");

                                        table.Cell().BorderLeft(1).BorderRight(1).PaddingLeft(5).Text(tipo).FontSize(10);
                                        table.Cell().BorderLeft(1).BorderRight(1).AlignRight().PaddingRight(5).Text(precio.ToString("N2")).FontSize(10);

                                    }
                                    table.Footer(foster =>
                                    {
                                        foster.Cell().Border(1).AlignCenter().Padding(2).Text("Fecha de Vencimiento: " + vencimiento).SemiBold();
                                        foster.Cell().Border(1).AlignRight().PaddingTop(2).PaddingRight(5).Text(total.ToString("N2")).SemiBold();
                                    });
                                });

                                col.Item().Text("").FontSize(3);
                                col.Item().Background(Colors.Grey.Lighten3).Padding(5)
                                .Column(col =>
                                {
                                    col.Item().AlignCenter().Text("Si detecta que algunos de los períodos reclamados ya fue abonado por favor envie un e-mail a cobranzaspanteon@caritas.org.ar");
                                });

                                col.Item().PaddingBottom(3).Text(model[0].Valor_a_Vencer).SemiBold();
                                col.Item().Text(model[0].SiguienteValor_Vencido).SemiBold();
                                col.Item().Text("VALORES A PARTIR DEL " + FechaAPartir).SemiBold();
                                col.Item().Text(model[0].SiguienteValor_Vencido).SemiBold();
                                col.Item().PaddingBottom(3).Text(model[0].SiguienteValor_a_Vencer).SemiBold();

               

                                col.Item().PaddingBottom(3).Text("Se considera mes vencido desde 11 de cada mes").SemiBold();
                                col.Item().Background(Colors.Orange.Lighten3).Text("  Recuerde que las cuotas no abonadas se calculan al valor vigente al momento del pago.").SemiBold();
                                col.Item().PaddingTop(3).Text("FORMAS DE PAGO:").SemiBold();

                                col.Item().Row(row =>
                                {
                                    row.ConstantItem(94).Column(col =>
                                    {
                                        col.Item().PaddingTop(3).Text("1-Débito Automatico:").FontSize(9).SemiBold();

                                    });
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().PaddingTop(3).Text("Tarjetas de débito VISA: 5% de descuento sobre el valor de la cuota por un año.").FontSize(9);
                                        col.Item().PaddingTop(3).Text("Tarjetas de crédito MASTERCARD O VISA: 10% de descuento sobre el valor de la cuota por un año.").FontSize(9);
                                        col.Item().PaddingTop(3).Text("Para solicitar la adhesión debe escribir a: debitospanteon@caritasbsas.org.ar").SemiBold().FontSize(9);
                                    });
                                });
                                col.Item().Row(row =>
                                {
                                    row.ConstantItem(94).Column(col =>
                                    {
                                        col.Item().PaddingTop(3).Text("2-RAPIPAGO:").FontSize(9).SemiBold();

                                    });
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().PaddingTop(3).Text("Pagos sin facturas para Panteón Nuestra Señora de la Merced Código de Pago " + model[0].CPagoElectronico).FontSize(9);
                                    });
                                });
                                col.Item().Row(row =>
                                {
                                    row.ConstantItem(94).Column(col =>
                                    {
                                        col.Item().PaddingTop(3).Text("3-PAGO FACIL:").FontSize(9).SemiBold();

                                    });
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().PaddingTop(3).Text("Pagos sin facturas para PAGOSPYME EXPRESS Código CYD" + model[0].CPagoElectronico).FontSize(9);
                                    });
                                });
                                col.Item().Row(row =>
                                {
                                    row.ConstantItem(100).Column(col =>
                                    {
                                        col.Item().PaddingTop(3).Text("4-PAGO MIS CUENTAS:").FontSize(9).SemiBold();

                                    });
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().PaddingTop(3).Text("(Otros servicios) Panteón Nuestra Señora de la Merced Código de Pago " + model[0].CPagoElectronico).FontSize(9);
                                    });
                                });
                                col.Item().Row(row =>
                                {
                                    row.ConstantItem(100).Column(col =>
                                    {
                                        col.Item().PaddingTop(3).Text("5-PAGAR (RED LINK):").FontSize(9).SemiBold();

                                    });
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().PaddingTop(3).Text("(Asociaciones y clubes) Panteón Nuestra Señora de la Merced Código de Pago " + model[0].CPagoElectronico).FontSize(9);
                                    });
                                });
                                col.Item().PaddingTop(4).PaddingBottom(5).Text("No debe informar su pago, el mismo es identificado e informado por la compañía recaudadora.");

                                col.Item().Row(row =>
                                {
                                    row.ConstantItem(300).Column(col =>
                                    {
                                        col.Item().Border(1).AlignMiddle().AlignCenter().Text("Próximo Vencimiento " + proximo);
                                    });
                                });

                                col.Item().AlignRight().PaddingTop(3).PaddingRight(30).Text("La Administración").Underline().SemiBold();

                            });
                        });

                        page.Footer().Row(row =>
                        {
                            row.RelativeItem().Height(100).Image(imageData3);
                        });
                    });
                }).GeneratePdf();

                //Stream stream = new MemoryStream(data);
                //return File(stream, "application/pdf", "Aviso.pdf");

                var PathToFilePdf = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                      + "templates" + Path.DirectorySeparatorChar.ToString() + "PdfAvisos"
                      + Path.DirectorySeparatorChar.ToString() + model[0].Cliente + ".pdf";


            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using Stream streamToWriteTo = System.IO.File.Open(PathToFilePdf, FileMode.Create);

                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(streamToWriteTo);
            }
            // }

            return Ok();
        }
    }
}
