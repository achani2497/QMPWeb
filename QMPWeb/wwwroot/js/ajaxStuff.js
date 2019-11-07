function cargarInfoUpdateGuardarropa(nombreViejo, idGuardarropa){

    $("#nombreViejoGuardarropa").val(nombreViejo)
    $("#nuevoNombreGuardarropa").val(nombreViejo)
    $("#idGuardarropa").val(idGuardarropa)

} 

function cargarInfoParaCompartirGuardarropa(idGuardarropa){
    $("#idGuardarropaACompartir").val(idGuardarropa)
}

function cargarPrendasDelGuardarropa(idGuardarropa, nombreGuardarropa){
    $("#nombreGuardarropaPrendas").text(nombreGuardarropa)
    console.log('Id del guardarropas del que voy a traer las prendas '+idGuardarropa)
    $.ajax({
        type: 'POST',
        url: '/Guardarropas/TraerPrendasDelGuardarropa',
        data: ('idGuardarropa='+idGuardarropa),
        success: (response) => {
            if(response.length > 0){
                $("#prendasGuardarropa").empty()
                response.forEach(function(element){
                    $("#prendasGuardarropa").append(`
                        <div class="col-md-4 mb-2">
                            <div class="card black-card">
                                <div class="card-body mx-auto">
                                    <p class="letra-de-titulo">Color principal: `+element.colorPrincipal+`</p>
                                    <button class="btn btn-danger btn-block btn-sm mt-4"><i class="far fa-trash-alt"></i> Eliminar prenda</button>
                                </div>
                            </div>
                        </div>
                    `)
                })
            }else{                
                $("#prendasGuardarropa").html(`
                    <div class="container">
                        <div class="col-lg-12 alert alert-danger alert-dismissible fade show mt-4 text-center">
                            <b> Este guardarropa todavia no tiene prendas!</b>
                        </div>
                    </div>
                `)
            }
        },
        failure:() => {
            console.log("No hay nada")
        }
    })

}