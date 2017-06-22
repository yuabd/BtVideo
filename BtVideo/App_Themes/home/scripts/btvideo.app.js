function htmlEncode(n) { return $("<div/>").text(n).html() }
function htmlDecode(n) { return $("<div/>").html(n).text() }
function heartBeat() { $.ajax({ url: baseUrl + "heartbeat.axd" }) }
function kendoGridErrorHandler(n) { if (n.errors) { var t = "Errors:\n"; $.each(n.errors, function (n, i) { "errors" in i && $.each(i.errors, function () { t += this + "\n" }) }); alert(t) } }
//function callHitHandler(n) { return $.ajax({ url: baseUrl + "hit.axd", contentType: "application/json; charset=utf-8", data: { id: n } }), !1 }
function resizeImages() { var w = $(".movie-img img").eq(1).width(); var h = (250 * w) / 177; $(".movie-img").height(h); $(".post-content img").removeAttr("height"); $(".post-content img").removeAttr("width"); $(".post-content img").addClass("img-responsive img-thumbnail") }
var baseUrl = "", rator, appInsights;

$(document).on("click", ".number-spinner a", function () {
    var n = $(this), t = n.closest(".number-spinner").find("input").val().trim(), i = 0;
    i = n.attr("data-dir") == "up" ? parseInt(t) + 1 : t > 0 ? parseInt(t) - 1 : 0;
    n.closest(".number-spinner").find("input").val(i);
});
$(function () {
    $('[data-toggle="popover"]').popover();
    $('[data-toggle="tooltip"]').tooltip();

    $("input#search, #search-mobile").focus(function () {
        $(this).attr("placeholder", "")
    }).blur(function () { $(this).attr("placeholder", "Search") });
    resizeImages();
    $("img").lazyload({
        //placeholder: "images/loading.gif",
        effect: "fadeIn"
    });
    //$("#btn-search").click(function () {
    //    var n = "/search/" + $("#txtSearch").val();
    //    window.location.href = n;
    //});

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
            $.getJSON("/like/" + n, function (n) {
                if (n.IsSuccess)
                {
                    var t = parseInt($(".likehits-num").text(), 10);
                    $(".likehits-num").html(++t);
                    $(".btn-ratings").attr("disabled", "disabled");
                    $(".ratemessage").show()
                } else alert(n.Message)
            })
        })
    }
};