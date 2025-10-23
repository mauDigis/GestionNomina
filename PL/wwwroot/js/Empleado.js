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