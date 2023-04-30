$(document).ready(function () {
    $.ajaxSetup({ cache: true });
    // busca los elementos el atributo data-modal y le suscribe el evento click
    $('button[data-toggle="ajax-modal"]').on('click', function (e) {
        // Abre la ventana modal con el formulario solicitado 
        var url = $(this).data('url');
     
        openmodal(url);
        return false;
    });

    $('#modalGenerica').on('hidden.bs.modal', function () {
        $('#contentModal').html('');
    })

});



function fntAbrir(url) {
    var pagina_actual = tableDatos.page();

    //Guardamos la página actual en el local storage
    localStorage.setItem("pagina_actual", pagina_actual);

    $.ajaxSetup({ cache: false });
    openmodal(url);
}


function openmodal(url) {
    // Hace una petición get y carga el formulario en la ventana modal
    $('#contentModal').load(url, function () {

        $('#modalGenerica').modal('show')
      
        bindForm(this);

    });
}
function bindForm(dialog) {
    // Suscribe el formulario en la ventana modal con el evento submit
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                // Si la petición es satisfactoria, se recarga la página actual
                if (result.success) {
                    if (result.action === undefined) {
                        swal("Atención", result.msg, "success");
                        tableDatos.ajax.reload(function () {
                            $("#modalGenerica").modal('hide');
                            var pagina = localStorage.getItem("pagina_actual");

                            // Pregutnamos si existe el item
                            if (pagina != undefined) {
                                //Decimos a la table que cargue la página requerida
                                tableDatos.page(parseInt(pagina)).draw('page');

                                //Eliminamos el item para que no genere conflicto a la hora de dar click en otro botón detalle
                                localStorage.removeItem("pagina_actual");
                            }
                        });
                    } else {                       
                        $("#modalGenerica").modal('hide');
                       
                        window.location.href = result.action;                        
                    }
                   
                } else {
                    if (result.msg != null) {
                         swal("Atención", result.msg, "error");
                          $("#modalGenerica").modal('hide');
                    }
                   

                    $('#contentModal').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}


