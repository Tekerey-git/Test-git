// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function AddModel(el) {
    var model = {
        Name: "MyName",
        Surname: "MySurname"
    }
    $.ajax({
        url: '/Commanders/TestCopy',
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            alert(x + '\n' + y + '\n' + z);
        }
    });

}