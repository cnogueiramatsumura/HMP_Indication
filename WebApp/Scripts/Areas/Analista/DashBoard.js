$(function () {
    $("#Actions").change(function () {
        var val = $(this).val();
        HideAll();
        $("." + val).removeClass("d-none");
    })
})


function HideAll() {
    $(".tohide").addClass("d-none");
}