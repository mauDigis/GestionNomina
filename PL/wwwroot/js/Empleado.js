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

function SaveEmpleado() {
    IdEmpleado = $("#IdEmpleado").val();

    console.log("Entro al SaveProduct");
    console.log(IdEmpleado)
    if (IdEmpleado < 1) {
        AddEmpleado();
    } else {
        UpdateEmpleado();
    }
}

function SelectEmpleado(IdEmpleado) {

    console.log("Entro al SelectEmpleado");
    console.log(IdEmpleado)

    if (IdEmpleado > 0) {
        GetById(IdEmpleado);
    } else {

    }
}
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
                    "<td>" + "<button class='btn btn-warning' onclick='SelectEmpleado(" + employees.idEmpleado + ")' >" + "<img src='../pencil-square.svg'>" + "</img>" + 'Editar' + "</button>" + "</td>" +
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

            const empleadoData = result.object;
            console.log(empleadoData.idEmpleado);

            if (empleadoData) {
                //Manda a llamar mi método y paso los datos.
                cargarDatosFormulario(empleadoData);

                //Guardo mi modal en una variable.
                const modal = new bootstrap.Modal(document.getElementById('staticBackdrop'));

                //Lo muestro ya con los datos cargados.
                modal.show();

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

//Petición Add
function AddEmpleado() {

    //Conversion de imagen
    manejarImagenYContinuar(function (imagenBase64String) {

        //Mi json debe ser igual al Modelo que recibe mi servicio.

        var empleado = {
            idEmpleado: 0,
            nombre: $('#txtNombre').val(),
            apellidoPaterno: $('#txtApellidoPaterno').val(),
            apellidoMaterno: $('#txtApellidoMaterno').val(),
            fechaNacimiento: $('#txtFechaNacimiento').val(),
            rfc: $('#txtRFC').val(),
            nss: $('#txtNSS').val(),
            curp: $('#txtCURP').val(),
            fechaIngreso: $('#txtFechaIngreso').val(),
            departamento: {
                idDepartamento: parseInt($('#txtIdDepartamento').val()),
                "descripcion": "",
                "departamentos": []
            },
            salarioBase: parseFloat($('#txtSalarioBase').val()),
            noFaltas: parseInt($('#txtNoFaltas').val()),
            imagen: null,
            imagen64: imagenBase64String,
            empleados: []
        };

    });

    console.log("JSON ENVIADO:", empleado);

    $.ajax({
        type: 'POST',
        url: 'http://localhost:5186/api/EmpleadoAPI/Add',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(empleado),
        success: function (result) { //200 OK
            console.log('Entro al success');
            console.log(result);

            if (result.correct === true) {
                alert('Se agrego exitosamente el empleado.');

                //Eliminar datos del formulario
                eliminarDatosFormulario();

                GetAll();
            }

        },
        error: function (result) {
            alert('Error al agregar el empleado.' + result.ErrorMessage);
        }
    });

}; //AddEmpleado

//Petición Update
function UpdateEmpleado() {

    //Mi json debe ser igual al Modelo que recibe mi servicio.

    var empleado = {
        idEmpleado: parseInt($('#IdEmpleado').val()),
        nombre: $('#txtNombre').val(),
        apellidoPaterno: $('#txtApellidoPaterno').val(),
        apellidoMaterno: $('#txtApellidoMaterno').val(),
        fechaNacimiento: $('#txtFechaNacimiento').val(),
        rfc: $('#txtRFC').val(),
        nss: $('#txtNSS').val(),
        curp: $('#txtCURP').val(),
        fechaIngreso: $('#txtFechaIngreso').val(),
        departamento: {
            idDepartamento: parseInt($('#txtIdDepartamento').val()),
            "descripcion": "",
            "departamentos": []
        },
        salarioBase: $('#txtSalarioBase').val(),
        noFaltas: $('#txtNoFaltas').val(),
        imagen: $('#imgEmpleado').val()
    };

    console.log("JSON ENVIADO Update:", empleado);

    $.ajax({
        type: 'PUT',
        url: 'http://localhost:5186/api/EmpleadoAPI/Update',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(empleado),
        success: function (result) { //200 OK
            console.log('Entro al success Update');
            console.log(result);

            if (result.correct === true) {
                alert('Se actualizo exitosamente el empleado.');

                //Guardo mi modal en una variable.
                const modal = new bootstrap.Modal(document.getElementById('staticBackdrop'));
                //Lo oculto ya con los datos cargados.
                modal.hide();

                //Elimino datos del formulario.
                eliminarDatosFormulario();

                GetAll();
            }

        },
        error: function (result) {
            alert('Error al Actualizar el empleado.' + result.ErrorMessage);
        }
    });

}; //UpdateEmpleado


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

    // LOGICA PARA CARGAR LA IMAGEN
    var imagenBase64 = data.imagen64;
    var imagenElemento = $('#imgUsuario');

    if (imagenBase64) {
        // Si existe la Base64, construimos el Data URL
        // El navegador espera el formato: data:image/[tipo_mime];base64,[cadena_base64]
        var imageSrc = "data:image/*;base64," + imagenBase64;
        imagenElemento.attr('src', imageSrc);
    } else {
        // Si no hay Base64, asignamos la imagen por defecto
        imagenElemento.attr('src', '/Img/NoPhoto.png');
    }
}

function eliminarDatosFormulario() {

    const formElement = document.getElementById("formEmpleado");
    formElement.reset();
}

/**
 * Verifica si hay una nueva imagen y la convierte a Base64.
 * @param {function(string | null): void} callback - Función que recibe la cadena Base64 (o null).
 */
function manejarImagenYContinuar(callback) {
    
    var fileInput = $('input[name="ImageFile"]')[0];

    // 2. VERIFICAR SI HAY UN ARCHIVO ADJUNTO
    if (fileInput.files.length > 0) {

        // CASO CONTRARIO: El usuario subió una imagen. Convertir a Base64.
        var archivo = fileInput.files[0];
        var reader = new FileReader();
        reader.onload = function (evento) {

            // Elimina el prefijo Data URL ('data:image/*;base64,')
            var base64String = evento.target.result.split(',')[1];

            // Retornamos la cadena Base64 al callback
            callback(base64String);
        };

        reader.onerror = function () {
            console.error("Error al leer el archivo. Enviando null.");
            callback(null);
        };

        reader.readAsDataURL(archivo);

    } else {
        callback(null);
    }
}
function RedireccionarAForm() {
    window.location.href = "/EmpleadoAJAX/FormAJAX";
}