if (document.querySelector("#foto")) {
    let foto = document.querySelector("#foto");
    foto.onchange = function (e) {
        let uploadFoto = document.querySelector("#foto").value;
        let fileimg = document.querySelector("#foto").files;
        let nav = window.URL || window.webkitURL;
        let contactAlert = document.querySelector('#form_alert');
        if (uploadFoto != '') {
            let type = fileimg[0].type;
            let name = fileimg[0].name;
            if (type != 'image/jpeg' && type != 'image/jpg' && type != 'image/png') {
                contactAlert.innerHTML = '<p class="errorArchivo">El archivo no es válido.</p>';
                if (document.querySelector('#img')) {
                    document.querySelector('#img').remove();
                }
                document.querySelector('.delPhoto').classList.add("notBlock");
                foto.value = "";
                return false;
            } else {
                contactAlert.innerHTML = '';
                if (document.querySelector('#img')) {
                    document.querySelector('#img').remove();
                }
                document.querySelector('.delPhoto').classList.remove("notBlock");
                let objeto_url = nav.createObjectURL(this.files[0]);
                document.querySelector('.prevPhoto div').innerHTML = "<img id='img1' src=" + objeto_url + ">";
            }
        } else {
            alert("No selecciono foto");
            if (document.querySelector('#img')) {
                document.querySelector('#img').remove();
            }
        }
    }
}

if (document.querySelector(".delPhoto")) {
    let delPhoto = document.querySelector(".delPhoto");
    delPhoto.onclick = function (e) {
        //document.querySelector("#foto_remove").value = 1;
        removePhoto();
    }
}


function removePhoto() {
    document.querySelector('#foto1').value = "";
    document.querySelector('.delPhoto').classList.add("notBlock");
    if (document.querySelector('#img1')) {
        document.querySelector('#img1').remove();
    }
}