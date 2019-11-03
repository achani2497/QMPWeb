$("#crearGuardarropa").click(function () {

    var idUsuario = parseInt($("#idUsuario").val());
    var nombreGuardarropa = $("#nombreGuardarropa").val();

    var json = {}

    json["idUsuario"] = idUsuario;
    json["nombreGuardarropa"] = nombreGuardarropa;

    $.ajax({
        async: false,
        type: "POST",
        url: "/Guardarropas/crearGuardarropa",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(json),
        success: function (response) {
            if (response.success) {
                alertify.success("Guardarropa creado!", 10)
            } else {
                alertify.error("No se pudo crear el guardarropas")
            }
        }
    })

})

function cargarInfoGuardarropa(nombreGuardarropa, idGuardarropa){

    $("#nuevoNombreGuardarropa").val(nombreGuardarropa)
    $("#idGuardarropa").val(idGuardarropa)

} 