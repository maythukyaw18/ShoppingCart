// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$("#Login").click(function () {
    var Username = $("#UserName").val();
    var Password = $("#Password").val();
    if (Username.length === 0 || Password.length === 0) {
        alert("All fields are required");
        //$("#error").html("All fields are required");
        
    }
    
});


$("#Download").click(function () {
    alert("Game downloaded successfully");
});


