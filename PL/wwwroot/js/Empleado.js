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
                    "<td>" + "<button class='btn btn-warning' onclick='SelectProduct(" + employees.idEmpleado + ")' >" + "<img src='../pencil-square.svg'>" + "</img>" + 'Editar' + "</button>" + "</td>" +
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

function SelectProduct(IdEmpleado) {
    //ProductID = $("#ProductID").val();

    console.log("Entro al SelectProduct");
    console.log(IdEmpleado)
    if (IdEmpleado > 0) {
        GetById(IdEmpleado);
    } else {
    }
}

//Peticíón GetById
function GetById(IdEmpleado) {
    console.log('Hola desde JS File');

    $.ajax({
        type: 'GET',
        url: 'http://localhost:5186/api/EmpleadoAPI/GetById/' + IdEmpleado,
        //contentType: 'application/json;',
        dataType: 'json',
        success: function (result) { //200 OK 
            console.log('Entro al success getById');
            console.log(result);
            console.log('objects' + result.object);

            const userData = result.object;
            console.log(userData.productID);

            if (userData) {
                //Manda a llamar mi método y paso los datos.
                cargarDatosFormulario(userData);

                //Defino la ruta base del controlador/acción
                var controlador = "Empleado";
                var accion = "Form";

                // 2. Construir la URL completa
                var urlRedireccion = "/" + controlador + "/" + accion + "?IdEmpleado=" + IdEmpleado;

                // 3. Redirecciona el navegador
                window.location.href = urlRedireccion;

                console.log('Datos cargados en el formulario.');
            } else {
                console.warn('El objeto de datos no se encontró en result.object o es nulo.');
            }
        },
        error: function (result) {
            alert('Error en la consulta.' + result.ErrorMessage);
        }
    });
}; //GetById

//Cargo los datos en mi formulario
function cargarDatosFormulario(data) {
    // Mapeo los valores mediante los IDs.

    // 1. Campos ocultos/ID
    if ($("#IdEmpleado").length) {
        $("#IdEmpleado").val(data.idEmpleado || '');
    }

    if ($("#txtNombre").length) {
        $("#txtNombre").val(data.nombre || '');
    }

    if ($("#txtApellidoPaterno").length) {
        $("#txtApellidoPaterno").val(data.apellidoPaterno || '');
    }

    if ($("#txtApellidoMaterno").length) {
        $("#txtApellidoMaterno").val(data.apellidoMaterno || '');
    }

    if ($("#txtFechaNacimiento").length) {
        $("#txtFechaNacimiento").val(data.fechaNacimiento || '');
    }

    if ($("#txtRFC").length) {
        $("#txtRFC").val(data.rfc || '');
    }

    if ($("#txtNSS").length) {
        $("#txtNSS").val(data.nss || '');
    }

    if ($("#txtCURP").length) {
        $("#txtCURP").val(data.curp || '');
    }

    if ($("#txtFechaIngreso").length) {
        $("#txtFechaIngreso").val(data.fechaIngreso || '');
    }

    if ($("#txtIdDepartamento").length) {
        $("#txtIdDepartamento").val(data.departamento.idDepartamento || '');
    }

    if ($("#txtSalarioBase").length) {
        $("#txtSalarioBase").val(data.salarioBase || '');
    }

    if ($("#txtNoFaltas").length) {
        $("#txtNoFaltas").val(data.noFaltas || '');
    }
}

