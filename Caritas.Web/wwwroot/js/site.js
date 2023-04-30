// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

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

       //$("#DetProvincia").select2({
    //    language: "es",
    //    dropdownParent: $("#myModal"),
    //    placeholder: "Seleccione una Provincia",
    //    with: "resolve",
    //    minimumInputLength: 1,
    //    ajax: {
    //        url: "/Generics/GetProvincias",
    //        contentType: "application/json; charset=utf-8",
    //        data: function (params) {
    //            var query =
    //            {
    //                term: params.term,
    //            };
    //            return query;
    //        },
    //        processResults: function (result) {
    //            return {
    //                results: $.map(result, function (item) {
    //                    return {
    //                        id: item.id,
    //                        text: item.descripcion
    //                    };
    //                }),
    //            };
    //        }
    //    }
    //});
  
});
