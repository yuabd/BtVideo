function htmlEncode(n) { return $("<div/>").text(n).html() }
function htmlDecode(n) { return $("<div/>").html(n).text() }
function heartBeat() { $.ajax({ url: baseUrl + "heartbeat.axd" }) }
function kendoGridErrorHandler(n) { if (n.errors) { var t = "Errors:\n"; $.each(n.errors, function (n, i) { "errors" in i && $.each(i.errors, function () { t += this + "\n" }) }); alert(t) } }
function callHitHandler(n) { return $.ajax({ url: baseUrl + "hit.axd", contentType: "application/json; charset=utf-8", data: { id: n } }), !1 }
function resizeImages() { $(".post-content img").removeAttr("height"); $(".post-content img").removeAttr("width"); $(".post-content img").addClass("img-responsive img-thumbnail") }
var baseUrl = "", rator, appInsights;
//$(function () {
//    baseUrl = $("#HiddenCurrentUrl").val();
//    baseUrl = baseUrl.substring(0, baseUrl.indexOf("Dummy"));
//    baseUrl.match(/\/$/) || (baseUrl += "/")
//});
$(document).on("click", ".number-spinner a", function () {
    var n = $(this), t = n.closest(".number-spinner").find("input").val().trim(), i = 0;
    i = n.attr("data-dir") == "up" ? parseInt(t) + 1 : t > 0 ? parseInt(t) - 1 : 0;
    n.closest(".number-spinner").find("input").val(i)
});
//$("#btnSearch").click(function () {
//    var n = baseUrl + "search/keyword/" + $("#txtSearch").val();
//    window.location.href = n
//});
$(function () {
    $('[data-toggle="popover"]').popover();
    $('[data-toggle="tooltip"]').tooltip();
    //$("#a-sign-in").click(function () { $("#page-content, .ediblog-header, footer .container").addClass("blur-15") });
    //$("#signInModal").on("hidden.bs.modal", function () {
    //    $("#page-content, .ediblog-header, footer .container").removeClass("blur-15")
    //});
    $("input#search, #search-mobile").focus(function () {
        $(this).attr("placeholder", "")
    }).blur(function () { $(this).attr("placeholder", "Search") })
});

commentBlowUp = function () { alert("Comment failed.") };
commentSuccess = function () { $("#form0").slideUp(); $("#thx-for-comment").slideDown() };
commentComplete = function () {
    $("#form0")[0].reset(); $("#form0").html("");
    $(".comment-form-containter").hide()
};
rator = {
    registerRatingButtons: function ()
    {
        $(".btn-ratings").click(function () {
            var n = $(this).data("postid");
            $.getJSON(baseUrl + "post/like?postid=" + n, function (n) {
                if (n.IsSuccess)
                {
                    var t = parseInt($(".likehits-num").text(), 10);
                    $(".likehits-num").html(++t);
                    $(".btn-ratings").attr("disabled", "disabled");
                    $(".ratemessage").show()
                } else toastr.warning(n.Message)
            })
        })
    }
};