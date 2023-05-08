var divLoading = document.querySelector("#divLoading");

document.addEventListener('DOMContentLoaded', function () {

    tableDatos = $('#tableRoles')  
        .DataTable({       
  //      "aProcessing":true,
		//"aServerSide": true,
        "ajax": {
            "url": "/Roles/GetAll",          
            },
        "Dom ": "Bfrtip",
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.10.19/i18n/Spanish.json"
        },
            "columns": [           
            { "data": "id", "width": "20%" },
            { "data": "name", "width": "20%" }, 
            { "data": "status", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                 <button onclick=fntPermisos("/Roles/Permisos/${data}") class="btn btn-success btn-sm" title ="Permisos">
                                    <i class="fa fa-lock"></i>
                                </button>
                            </div>
                           `;
                }, "width": "15%"
            }
        ],
        "resonsieve":"true",
        "bDestroy": true,
        "iDisplayLength": 10,
        "order": [[1, "asc"]]
        });   

   
});

function fntAbrir(url) {

    var pagina_actual = tableDatos.page();

    //Guardamos la página actual en el local storage
    localStorage.setItem("pagina_actual", pagina_actual);

    $.ajaxSetup({ cache: false });
    openmodal(url);
}


//function openModal() {
//    document.querySelector('#idRol').value = "";
//    document.querySelector('.modal-header').classList.replace("headerUpdate", "headerRegister");
//    document.querySelector('#btnActionForm').classList.replace("btn-info", "btn-primary");
//    document.querySelector('#btnText').innerHTML = "Guardar";
//    document.querySelector('#titleModal').innerHTML = "Nuevo Rol";
//    document.querySelector("#formRol").reset();

//    $('#modalFormRol').modal('show');
//}

function fntPermisos(url) {
    var idrol = idrol;
    var request = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject('Microsoft.XMLHTTP');
    var ajaxUrl = url;
    request.open("GET", ajaxUrl, true);
    request.send();

    request.onreadystatechange = function () {
        if (request.readyState == 4 && request.status == 200) {
            var objData = JSON.parse(request.responseText);
            document.querySelector('#contentAjax').innerHTML = objData.data;                  
            $('.modalPermisos').modal('show');

           document.getElementById("lblTitulo").innerHTML ="Permisos para el Rol: " + objData.titulo;
           document.querySelector('#formPermisos').addEventListener('submit', fntSavePermisos, false);
        }
    }
    
}

function fntSavePermisos(evnet) {
    evnet.preventDefault();

    divLoading.style.display = "flex";

    var request = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject('Microsoft.XMLHTTP');
    var ajaxUrl = '/Roles/SetPermisos';
    var formElement = document.querySelector("#formPermisos");
    var formData = new FormData(formElement);
    request.open("POST", ajaxUrl, true);
    request.send(formData);

    request.onreadystatechange = function () {
        if (request.readyState == 4 && request.status == 200) {
            var objData = JSON.parse(request.responseText);
            if (objData.status) {
                swal("Permisos de usuario", objData.msg, "success");
            } else {
                swal("Error", objData.msg, "error");
            }
        }

        divLoading.style.display = "none";
    }

}

