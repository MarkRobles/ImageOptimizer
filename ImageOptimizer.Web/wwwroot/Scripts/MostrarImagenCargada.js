﻿$(document).ready(function () {
    $('#file-upload').change(function () {
        debugger;
        var i = $(this).prev('label').clone();
        var file = $('#file-upload')[0].files[0].name;
        $(this).prev('label').text(file);
    });

});

