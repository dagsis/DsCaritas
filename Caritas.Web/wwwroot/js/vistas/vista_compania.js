

$(document).ready(function () {

  

});



$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});

function DeleteItem(btn) {

  

    var table = document.getElementById('ExpTable');
    var rows = table.getElementsByTagName('tr');
    if (rows.length == 2) {
        return;
    }

    var btnIdx = btn.id.replaceAll('btnremove-', '');
    var idofIsDeleted = btnIdx + "__IsDeleted";

    var hidIsdelId = document.querySelector("[id$='" + idofIsDeleted + "']").id;
    document.getElementById(hidIsdelId).value = "true";

    $(btn).closest('tr').hide();
}


function AddItem(btn) {

    var table = document.getElementById('ExpTable');
    var rows = table.getElementsByTagName('tr');

    var rowOuterHtml = rows[rows.length - 1].outerHTML;

    var lastrowIdx = rows.length-2;  

    var nextrowIdx = eval(lastrowIdx) + 1;

    rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIdx + '_', '_' + nextrowIdx + '_');
    rowOuterHtml = rowOuterHtml.replaceAll('[' + lastrowIdx + ']', '[' + nextrowIdx + ']');
    rowOuterHtml = rowOuterHtml.replaceAll('-' + lastrowIdx, '-' + nextrowIdx);

    var newRow = table.insertRow();
    newRow.innerHTML = rowOuterHtml;

    var x = document.getElementsByTagName("INPUT");
    var cmb = document.getElementsByTagName("SELECT");

    for (var cnt = 0; cnt < x.length; cnt++) {
        if (x[cnt].type == "text" && x[cnt].id.indexOf('_' + nextrowIdx + '_') > 0)
            x[cnt].value = '';       
    }

    for (var cnt = 0; cnt < cmb.length; cnt++) {
        if (cmb[cnt].value > 0 && cmb[cnt].id.indexOf('_' + nextrowIdx + '_') > 0)
            cmb[cnt].value = 0
    }
   
  //  rebindvalidators();

}

function rebindvalidators() {
    var $form = $("#Compania-Form");
    $form.unbind();
    $form.data("validator", null);
    $.validator.unobtrusive.parse($form);
    $form.validate($form.data("unobtrusiveValidation").options);
}

$("#btnCuit").click(function () {
    var scuit = $("#Cuit").val();

    if (scuit == "") {
        return false;
    }
    $('#imagen').attr('src', '/img/programa/loading32.gif');
    $.ajax({
        type: 'POST',
        url: '/Generics/GetDatosAfip',
        dataType: 'json',
        data: { cuit: $("#Cuit").val() },
        success: function (data) {
            document.getElementById("Nombre").value = data.nombre;
            document.getElementById("Inicio").value = data.inicio;
            document.getElementById("Direccion").value = data.direccion;
            document.getElementById("ProvinciaId").value = data.idProvincia == 0 ? "15" : data.idProvincia;
            $("select#TipoIvaId option")
                .each(function () { this.selected = (this.text == data.categoriaIva); });
            document.getElementById("Ciudad").value = data.localidad;
            document.getElementById("Actividad").value = data.actividad;
            document.getElementById("EstadoAfip").value = data.estado;
            document.getElementById("TipoPersona").value = data.tipoPersona;
            $('#imagen').attr('src', '');
        },
        error: function (ex) {
            alert('Error al recuperar el cuit.' + ex);
        }
    });
    return false;
});