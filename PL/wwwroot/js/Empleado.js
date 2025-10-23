var empleadoDelete = '@Url.Action("Delete", "Empleado")' //Helper de razor
function EmpleadoDelete(IdEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno) {

    var IdEmpleado = document.getElementById("txtIdEmpleado");
    var Nombre = document.getElementById("txtNombre");
    var ApellidoPaterno = document.getElementById("txtApellidoPaterno");
    var ApellidoMaterno = document.getElementById("txtApellidoMaterno");

    console.log(IdEmpleado);
    console.log(Nombre);
    console.log(ApellidoPaterno);
    console.log(ApellidoMaterno);

    const btnEliminar = document.querySelector('#btnEliminarEmpleado').setAttribute('href', empleadoDelete + '?IdEmpleado=' + IdEmpleado);

}

function PrevisualizarImagen(event) {
    var output = document.getElementById('imgUsuario'); //Obtiene mi imagen de mi id="imgUsuario"
    output.src = URL.createObjectURL(event.target.files[0]);
    output.onload = function () {
        URL.revokeObjectURL(output.src) // free memory
    }
}

//PETICIONES AJAX

$(document).ready(function () { //click

    //Aqui mando a llamar a mi funcion
    GetAll();
});



function GetAll() {
    console.log('Hola Js');

    $.ajax({
        type: 'GET',
        url: 'http://localhost:5186/api/EmpleadoAPI/GetAll',
        dataType: 'json',
        success: function (result) { //200 OK
            console.log('Entro al success');
            $('#tableEmployees tbody').empty();
            console.log(result);
            console.log('objects' + result.objects);

            $.each(result.objects, function (i, employees) {

                var imageSrc = "";

                if (employees.imagen64) {
                    //Guardo y leo mi imagen a base64.
                    imageSrc = "data:image/*;base64," + employees.imagen64;
                }

                var FilasDinamicas =
                    "<tr>" +

                    "<td>" + "<img src='" + imageSrc + "' alt='ImagenEmpleado' style='width: 80px; height: 80px;'>" + "</td>" +
                    "<td id='IdEmpleado' class='text-center'>" + employees.idEmpleado + "</td>" +
                    "<td class='text-center'>" + employees.nombre + ' ' + employees.apellidoPaterno + ' ' + employees.apellidoMaterno + "</td>" +
                    "<td class='text-center'>" + employees.fechaNacimiento + "</td>" +
                    "<td class='text-center'>" + employees.rfc + "</td>" +
                    "<td class='text-center'>" + employees.nss + "</td>" +
                    "<td class='text-center'>" + employees.curp + "</td>" +
                    "<td class='text-center'>" + employees.fechaIngreso + "</td>" +
                    "<td class='text-center'>" + employees.departamento.descripcion + "</td>" +
                    "<td class='text-center'>" + employees.salarioBase + "</td>" +
                    "<td class='text-center'>" + employees.noFaltas + "</td>" +
                    "<td>" + "<button class='btn btn-warning' >" + "<img src='../pencil-square.svg'>" + "</img>" + 'Editar' + "</button>" + "</td>" +
                    "<td>" + "<button class='btn btn-danger deletebtn' >" + "<img src='../trash.svg'>" + "</img>" + 'Eliminar' + "</button>" + "</td>" +

                    "</tr>";

                console.log(FilasDinamicas);
                $("#tableEmployees tbody").append(FilasDinamicas);
            });
        },
        error: function (result) {
            alert('Error en la consulta.' + result.ErrorMessage);
        }
    });
}

//function GetAll() {
//    console.log('Hola desde JS File');

//    $.ajaxSetup({
//        beforeSend: () => {
//            $('#tablaProducts').hide();
//            $("#loading").show();
//        },
//        complete: () => {
//            $("#loading").hide();
//            $('#tablaProducts').show();
//        }
//    });

//    $.ajax({
//        type: 'GET',
//        url: 'http://localhost:5186/api/EmpleadoAPI/GetAll',
//        //contentType: 'application/json;',
//        dataType: 'json',
//        success: function (result) { //200 OK
//            console.log('Entro al success');
//            $('#tablaProducts tbody').empty();
//            console.log(result);
//            console.log('objects' + result.objects);

//            $.each(result.objects, function (i, products) {

//                var Filasdinamicas =
//                    "<tr>" +
//                    "<td>" + "<button class='btn btn-warning' onclick='SelectProduct(" + products.productID + ")' >" + "<img src='../pencil-square.svg'>" + "</img>" + 'Editar' + "</button>" + "</td>" +
//                    //"<td>" + "<button class='btn btn-danger deletebtn'  onclick='Delete(" + products.productID + ")'  >" + "<img src='../trash.svg'>" + "</img>" + 'Eliminar' + "</button>" + "</td>" +
//                    "<td>" + "<button class='btn btn-danger deletebtn'  data-product-id='" + products.productID + "'  >" + "<img src='../trash.svg'>" + "</img>" + 'Eliminar' + "</button>" + "</td>" +
//                    "<td id='productID' class='text-center'>" + products.productID + "</td>" +
//                    "<td class='text-center'>" + products.productName + "</td>" +
//                    "<td class='text-center'>" + products.supplier.supplierID + "</td>" +
//                    "<td class='text-center'>" + products.category.categoryID + "</td>" +
//                    "<td class='text-center'>" + products.quantityPerUnit + "</td>" +
//                    "<td class='text-center'>" + products.unitPrice + "</td>" +
//                    "<td class='text-center'>" + products.unitsInStock + "</td>" +
//                    "<td class='text-center'>" + products.unitsOnOrder + "</td>" +
//                    "<td class='text-center'>" + products.reorderLevel + "</td>" +
//                    "<td class='text-center'>" + products.discontinued + "</td>" +
//                    "</tr>";

//                console.log(Filasdinamicas);
//                $("#tablaProducts tbody").append(Filasdinamicas);
//            });
//        },
//        error: function (result) {
//            alert('Error en la consulta.' + result.ErrorMessage);
//        }
//    });
//}; //GetAll