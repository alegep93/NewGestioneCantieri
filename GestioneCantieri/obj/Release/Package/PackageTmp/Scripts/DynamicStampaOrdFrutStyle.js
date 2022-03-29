$(function () {
    var text = $("select.form-control option:selected").text();

    if (text !== "")
        $(".ddlContainer").addClass("col-6");
    else
        $(".ddlContainer").addClass("offset-md-3 col-6");
});