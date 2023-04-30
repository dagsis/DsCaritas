
var divLoading = document.querySelector("#divLoading");

document.addEventListener('DOMContentLoaded', function () {

    divLoading.style.display = "none";

    if (document.querySelector("#formPerfil")) {
        let formLogin = document.querySelector("#formPerfil");
        formLogin.onsubmit = function (e) {
            e.preventDefault();
            let strNombre = document.querySelector('#Nombre').value;
            let strEmail = document.querySelector('#Email').value;

            if (strNombre == "" || strEmail == "") {
                swal("Por favor", "El nombre es obligatorio", "error");
                return false;
            } else {
                divLoading.style.display = "flex";
                var request = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject('Microsoft.XMLHTTP');
                var ajaxUrl = '/Users/PerfilEdit';
                var formData = new FormData(formLogin);
                request.open("POST", ajaxUrl, true);
                request.send(formData);
                request.onreadystatechange = function () {
                    if (request.readyState != 4) return;
                    if (request.status == 200) {
                        var objData = JSON.parse(request.responseText);
                        if (objData.status == "success") {
                            swal({
                                title: "",
                                text: objData.msg,
                                type: "success",
                                confirmButtonText: "Ok",
                                closeOnConfirm: false,
                            }, function (isConfirm) {
                                if (isConfirm) {
                                    window.location.href = '/';
                                }
                            });
                           
                        } else {
                            swal("Atención", objData.msg, "error");
                        }
                    } else {
                        swal("Atención", "Error en el Proceso", "error");
                    }
                    divLoading.style.display = "none";
                    return false;
                }
            }
        }
    }
   
}, false)
