﻿$(document).ready(function () {
    //$('#file-upload').change(function () {
    //    debugger;
    //    var i = $(this).prev('label').clone();
    //    var file = $('#file-upload')[0].files[0].name;
    //    $(this).prev('label').text(file);
    //});


    document.getElementById("file-upload").onchange = function (e) {
        // Creamos el objeto de la clase FileReader
        let reader = new FileReader();

        // Leemos el archivo subido y se lo pasamos a nuestro fileReader
        reader.readAsDataURL(e.target.files[0]);

        // Le decimos que cuando este listo ejecute el código interno
        reader.onload = function () {
            let preview = document.getElementById('preview'),
                image = document.createElement('img');

            image.src = reader.result;

            preview.innerHTML = '';
            preview.append(image);
        };
    }

});


