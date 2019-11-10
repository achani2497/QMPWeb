function cargarInfoUpdateGuardarropa(nombreViejo, idGuardarropa){

    $("#nombreViejoGuardarropa").val(nombreViejo)
    $("#nuevoNombreGuardarropa").val(nombreViejo)
    $("#idGuardarropa").val(idGuardarropa)

} 

function cargarInfoParaCompartirGuardarropa(idGuardarropa){
    $("#idGuardarropaACompartir").val(idGuardarropa)
}

function cargarSelectsParaCreacionDePrenda(idUsuario){

    $('#guardarropasDelUsuario').empty();
    $('#tiposDeTela').empty();
    $('#tiposDePrenda').empty();

    $('#guardarropasDelUsuario').append('<option selected>Seleccioná algún guardarropa</option>');
    $('#tiposDePrenda').append('<option selected>Seleccioná el tipo de prenda</option>');
    $('#tiposDeTela').append('<option selected>Seleccioná el tipo de tela de la prenda</option>');

    $.ajax({
        type: 'POST',
        url: '/Usuario/TraerGuardarropasDelUsuario',
        data: ('idUsuario='+idUsuario),
        success: (response) => {
            if(response.length > 0){
                response.forEach(function(element){
                    $('#guardarropasDelUsuario').append("<option value="+element.id_guardarropa+">"+element.nombreGuardarropas+"</option>")
                })
            } else {
                console.log("El usuario no tiene guardarropas todavia")
            }
        }
    })

    $.ajax({
        type: 'GET',
        url: '/Prendas/TraerTelas',
        success: (response) => {
            response.forEach(function(tela){
                $('#tiposDeTela').append("<option value="+tela.id_tela+">"+tela.descripcion+"</option>")
            })
        }
    })

    $.ajax({
        type: 'GET',
        url: '/Prendas/TraerTiposDePrenda',
        success: (response) => {
            response.forEach(function(tipoDePrenda){
                $('#tiposDePrenda').append("<option value="+tipoDePrenda.id_tipoPrenda+">"+tipoDePrenda.descripcion+"</option>")
            })
        }
    })

}