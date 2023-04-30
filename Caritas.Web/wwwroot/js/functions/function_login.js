
var divLoading = document.querySelector("#divLoading");

document.addEventListener('DOMContentLoaded', function () {
	divLoading.style.display = "none";
    if (document.querySelector("#formLogin")) {
        let formLogin = document.querySelector("#formLogin");
        formLogin.onsubmit = function (e) {
            e.preventDefault();
            let strEmail = document.querySelector('#Email').value;
			let strPassword = document.querySelector('#Password').value;

			if (strEmail == "" || strPassword == "") {
				swal("Por favor", "Escribe usuario y contraseña.", "error");
				return false;
			} else {
				divLoading.style.display = "flex";
				var request = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject('Microsoft.XMLHTTP');
				var ajaxUrl = '/Account/login';
				var formData = new FormData(formLogin);
				request.open("POST", ajaxUrl, true);
				request.send(formData);
				request.onreadystatechange = function () {
					if (request.readyState != 4) return;
					if (request.status == 200) {
						var objData = JSON.parse(request.responseText);
						if (objData.status == "success") {							
							window.location.href = '/';
						} else {
							swal("Atención", objData.msg, "error");
							document.querySelector('#Password').value = "";
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

	if (document.querySelector("#formRecetPass")) {
		let formRecetPass = document.querySelector("#formRecetPass");
		formRecetPass.onsubmit = function (e) {
			e.preventDefault();

			let strEmail = document.querySelector("#Email").value;
			if ((strEmail) == "") {
				swal("Por favor", "Escriba un correo electronico", "error");
				return false;
			} else {

				divLoading.style.display = "flex";

				var request = (window.XMLHttpRequest) ?
					new XMLHttpRequest() :
					new ActiveXObject('Microsoft.XMLHTTP');
				var ajaxUrl = '/Account/ForgotPassword';
				var formData = new FormData(formRecetPass);
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
								confirmButtonText: "Iniciar sessión",
								closeOnConfirm: false,
							}, function (isConfirm) {
								if (isConfirm) {
									window.location = '/Account/Login';
								}
							});
						}
						else {
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

	if (document.querySelector("#formCambiarPass")) {
		let formCambiarPass = document.querySelector("#formCambiarPass");
		formCambiarPass.onsubmit = function (e) {
			e.preventDefault();

			let strEmail = document.querySelector("#Email").value;
			let strPassword = document.querySelector("#Password").value;
			let strPasswordConfirm = document.querySelector("#ConfirmPassword").value;
			if (strEmail == "")
			{
				swal("Por favor", "Escriba su Email", "error");
				return false;
			}
			if (strPassword == "" || strPasswordConfirm == "") {
				swal("Por favor", "Escriba la nueva Contraseña", "error");
				return false;
			} else {
				if (strPassword.length < 6) {
					swal("Atención", "La contraseña debe tener un mínimo de 6 caracteres.", "info");
					return false;
				}
				if (strPassword != strPasswordConfirm) {
					swal("Atención", "Las contraseñas no son iguales.", "error");
					return false;
				}
				divLoading.style.display = "flex";
				var request = (window.XMLHttpRequest) ?
					new XMLHttpRequest() :
					new ActiveXObject('Microsoft.XMLHTTP');
				var ajaxUrl = '/Account/ResetPassword';
				var formData = new FormData(formCambiarPass);
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
								confirmButtonText: "Iniciar sessión",
								closeOnConfirm: false,
							}, function (isConfirm) {
								if (isConfirm) {
									window.location = '/Account/Login';
								}
							});
						} else {
							swal("Atención", objData.msg, "error");
						}
					} else {
						swal("Atención", "Error en el proceso", "error");
					}
					divLoading.style.display = "none";
					return false;
				}
			}

		}
	}
},false)