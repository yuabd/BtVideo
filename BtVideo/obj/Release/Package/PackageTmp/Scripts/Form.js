function getParUrl(url, par) {
	if (url.indexOf("?") == -1) {
		if (par) {
			url = url + "?" + par;
		}
		else {
			url = url;
		}

	}
	else {
		var baseUrl = url.split('?')[0];
		var paraString = url.split('?')[1];
		if (paraString.indexOf("ua=") == -1) {
			if (par) {
				url = baseUrl + "?" + paraString + "&" + par;
			}
			else {
				//				url = baseUrl + "?" + paraString + "&ua=" + parent.active_UserName();
				url = baseUrl + "?" + paraString;
			}

		}
	}
	return url;
}


$.extend({
    U1Ajax: function (url, postData, successHandle, Async, errorHandle) {
        if (url == null) {
            alert("Ajax 地址错误！");
        }
        url = getParUrl(url);

        var async = true;
        if (Async != null) {
            async = Async;
        }
        $.ajax({
            type: "POST",
            url: url,
            data: postData,
            async: async,
            success: function (result) {
                if (result.Tag == -999) {
                    alert(result.Message);
                    if (errorHandle)
                        errorHandle();
                    return;
                }
                if (successHandle) {
                    successHandle(result);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (errorHandle)
                    errorHandle();
            }
        });
    }

});




//$.U1Ajax(url, postData, function (result) { 

//}, Async);

//var postData = $("#testID").GetPostData();

$.fn.GetPostData = function () {
	var data = {};

	$(this).find("[col]").each(function (i, value) {

		var field = $(value).attr("col");

		if (value.tagName == "INPUT") {
			if (value.type == "checkbox") {
				if ($(value).attr("checked") == true) {
					if (data[field]) {
						data[field] = data[field] + "," + $(value).val();
					} else {
						data[field] = $(value).val();
					}
				}
			}
			else if (value.type == "radio") {
				if ($(value).attr("checked") == true) {
					data[field] = $(value).val();
				}
			}
			else {
				data[field] = $(value).val();
			}
		}

		else if (value.tagName == "SELECT") {
			data[field] = $(value).val();
		}
		else if (value.tagName == "DIV") {
			data[field] = $(value).html();
		}
		else if (value.tagName == "IMG") {
			data[field] = $(value).attr("src");
		}
		else if (value.tagName == "SPAN") {
			data[field] = $(value).html();
		}
		else if (value.tagName == "TEXTAREA") {
			data[field] = $(value).val();
		}

	});
	return data;
}


$.fn.GetExcelPostData = function () {
	var data = "";

	$(this).find("[col]").each(function (i, value) {

		var field = $(value).attr("col");
		if ((value.tagName == "INPUT" || value.tagName == "SELECT") && $(value).val() != "") {
			if (data == "") {
				data += "?" + field + "=" + $(value).val();
			} else {
				data += "&" + field + "=" + $(value).val();
			}
		}
		else if ((value.tagName == "DIV" || value.tagName == "SPAN") && $(value).html() != "") {
			if (data == "") {
				data += "?" + field + "=" + $(value).html();
			} else {
				data += "&" + field + "=" + $(value).html();
			}

		}
	});
	return data;
}

