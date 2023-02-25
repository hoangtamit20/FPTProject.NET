// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//your javascript goes here
$("body").on("click","#btnPdf",function(e){
    e.preventDefault();
    // var theText = $('#pdfContainer').html();
    // console.log(theText)
    var strHtml = $("#pdfContainer").html();
    strHtml = strHtml.replace(/</g, "StrTag").replace(/>/g, "EndTag");


    $("#pdfValue").val(strHtml)
    console.log($("#pdfValue").val())
    $("#formPDF").submit()
});