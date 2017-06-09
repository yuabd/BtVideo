
/// <reference path="../../Scripts/jquery-1.4.4.js" />
/// <reference path="jquery.js" />
/// <reference path="jquery.easyui.js" />





$.extend({

	validatebox: {
		//        required: false,
		//        validType: null,
		//        missingMessage: 'This field is required.',
		//        invalidMessage: null,
		rules: {
			required: {
				required: true,
				validator: function (value) {
					if (value == null)
						return false;
					value = value.replace(/\s/, "");
					var len = $.trim(value).length;
					return len >= 1;
				},
				message: '{0}不能为空！'
			},
			requiredInput: {
				required: true,
				validator: function (value) {
					if (value == null)
						return false;
					value = value.replace(/\s/, "");
					var len = $.trim(value).length;
					return len >= 1;
				},
				message: '请输入{0}！'
			},
			email: {
				validator: function (value) {
					return /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i.test(value);
				},
				message: '请输入正确的email地址！'
			},
			url: {
				validator: function (value) {
					return /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(value);
				},
				message: '请输入合法的url地址！'
			},
			length: {
				param: null,
				validator: function (value) {
					var len = $.trim(value).length;
					var paramArray = this.param.split(",");
					return len >= paramArray[0] && len <= paramArray[1]
				},
				message: '只能输入{0}-{1}个字符！'
			},
			Phone: {
				validator: function (value) {
					return (/^1[3|5|8][0-9]\d{4,8}$/).test(value);
				},
				message: '请输入正确手机号码！'
			},
			TelePhone: {
				validator: function (value) {
					return (/^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$/).test(value);
				},
				message: '请输入正确电话号码！'
			},
			maxLen: {
				validator: function (value) {
					var maxLength = this.param;
					if (value.length > maxLength) {
						return false;
					}
					return true;
				},
				message: '最大长度必须小于{0}个字符！'
			},
			TelPhoneRequired: {
				required: true,
				validator: function (value) {
					return (/^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$/).test(value);
				},
				message: "请输入正确的电话号码!"
			},
			PurchaseSaleStockGrossDateRange: {
				required: true,
				validator: function (value) {
					if (value == null) return false;
					value = value.replace(/\s/, "");
					if (value.length < 1) return false;
					var now = new Date();
					var minDate = new Date(now.setMonth(now.getMonth() - 3));
					if ((Date.parse(value) - minDate) < 0) return false;
					return true;
				},
				message: "只能查询3个月内的订单！"
			}
		},
		methods: {
			//显示tip
			showTip: function (target, msg) {
				var box = target;
				var tag = $(box);
				var tip = $.data(box, 'validatebox').ruleArray.tip;
				if (!tip) {
					tip = $('<div class="validatebox-tip">' +
					'<span class="validatebox-tip-content">' +
					'</span>' +
					'<span class="validatebox-tip-pointer">' +
					'</span>' +
				    '</div>').appendTo('body');
					tip.mouseover(function () { $(this).hide(); });
					$.data(box, 'validatebox').ruleArray.tip = tip;
				}
				tip.find('.validatebox-tip-content').html(msg);
				if (tag.is(":hidden")) {
					var p = tag.prev(":input");
					if (p) {
						tip.css({
							display: 'block',
							left: p.eq(0).offset().left + p.eq(0).outerWidth(),
							top: p.eq(0).offset().top
						});
					}
				} else {
					tip.css({
						display: 'block',
						left: tag.offset().left + tag.outerWidth(),
						top: tag.offset().top
					});
				}
				if (!tag.hasClass("valid_error"))
					tag.addClass("valid_error");
			},
			//隐藏tip
			hideTip: function (target) {
				if ($.data(target, 'validatebox').ruleArray == null)
					return;
				var tip = $.data(target, 'validatebox').ruleArray.tip;
				if (tip) {
					tip.remove();
					$.data(target, 'validatebox').ruleArray.tip = null;
				}
				if ($(target).hasClass("valid_error"))
					$(target).removeClass("valid_error");
			},
			//克隆对象
			clone: function (myObj) {
				if (typeof (myObj) != 'object') return myObj;
				if (myObj == null) return myObj;
				var myNewObj = new Object();
				for (var i in myObj)
					myNewObj[i] = this.clone(myObj[i]);
				return myNewObj;
			},
			//初始化
			valid_load: function () {
				$this = this;
				var controls = $("[validtype]");
				controls.each(function (i, val) {
					var box = val;
					var ruleArray = $this.regeditValidate(box);
					$(box).bind('focus mouseover change keyup', function () {
						$this.tip(ruleArray, box);
					}).bind('blur mouseout', function () {
						$this.hideTip(box);
					});

				});
			},
			//渲染单个
			apply: function (box) {
				$this = this;
				var ruleArray = $this.regeditValidate(box);
				$(box).bind('focus mouseover change keyup', function () {
					$this.tip(ruleArray, box);
				}).bind('blur mouseout', function () {
					$this.hideTip(box);
				});
			},
			//注册
			regeditValidate: function (box) {
				$this = this;
				var validtype = $(box).attr("validtype");
				var validtypeItem = validtype.split("|");
				var ruleArray = [];
				$(validtypeItem).each(function (i, val) {
					var params = "";
					var key = "";
					if (val.indexOf("(") > 0) {
						var arr = val.split("(");
						var key = arr[0];
						params = arr[1].replace(")", "");
					}
					else {
						key = val;
					}
					if ($.validatebox.rules[key] == null) {
						return;
					}
					var rule = $this.clone($.validatebox.rules[key]);
					rule.param = params;
					$(rule.param.split(",")).each(function (i, v) {
						if (typeof (rule.message) == "string")
							rule.message = rule.message.replace("{" + i + "}", v)
						else
							rule.message = rule.message().replace("{" + i + "}", v)
					});
					ruleArray.push(rule);
				});
				if (ruleArray != null && ruleArray.length > 0) {
					if (!$.data(box, 'validatebox')) {
						$.data(box, 'validatebox', {});
					}
					$.data(box, 'validatebox').ruleArray = ruleArray;
				}
				return ruleArray;
			},
			//tip
			tip: function (ruleArray, box) {
				var valid = true;
				var msg = "";
				var required = false;
				for (var i = 0; i < ruleArray.length; i++) {
					if (ruleArray[i].required) {
						required = true;
					}
				}
				if (required == false && !$.validatebox.rules.required.validator($(box).val())) {
					this.hideTip(box);
					return true;
				}
				$(ruleArray).each(function (i, val) {
					var rule = val;
					if (!rule.validator($.trim($(box).val()))) {
						valid = false;
						msg = msg + "※ " + rule.message + "<br/>";
					}
				});

				if (!valid) {
					this.showTip(box, msg);
					return false;
				}
				else {
					this.hideTip(box);
					return true;
				}
			},
			//验证
			valid: function (container) {
				var controls = null;
				if (container) {
					controls = $(container).find("[validtype]");
				}
				else {
					controls = $("[validtype]");
				}
				$this = this;
				var valid = true;
				controls.each(function (i, val) {
					var ruleArray = $.data(val, 'validatebox').ruleArray;
					var temp = $this.tip(ruleArray, val);
					valid = valid && temp;
				});
				return valid;
			}
		},
		valid: function (container) {
			return this.methods.valid(container);
		},
		apply: function (jbox) {
			this.methods.apply(jbox.get(0));
		}

	}


});




$(function () {
	// 文档就绪
	$.validatebox.methods.valid_load();
});



$.extend($.validatebox.rules, {
	PositiveInteger: {
		validator: function (value) {
			return (/^\d+$/).test(value);
		},
		message: '请输入正整数！'
	},
	UppercaseLetters: {
		validator: function (value) {
			return (/^[A-Z]+$/).test(value);
		},
		message: '只允许大写的英文字母！'
	},
	QQ: {
		validator: function (value) {
			return (/^[1-9]*[1-9][0-9]*$/).test(value);
		},
		message: '请输入正确的qq号码！'
	},
	Eng: {
		validator: function (value) {
			return (/^[A-Za-z]+$/).test(value);
		},
		message: '只允许输入英文！'
	},
	EngNum: {
		validator: function (value) {
			return (/^[A-Za-z0-9]+$/).test(value);
		},
		message: '只允许输入英文和数字！'
	},
	EngNumUnderline: {
		validator: function (value) {
			return (/^\w+$/).test(value);
		},
		message: '只允许输入英文和数字和下划线！'
	},
	Chn: {
		validator: function (value) {
			return (/[\u4e00-\u9fa5]/).test(value);
		},
		message: '只允许输入中文！'
	},
	Num: {
		validator: function (value) {
			return (/^(-?\d+)(\.\d+)?$/).test(value);
		},
		message: '只允许输入数字！'
	},
	NumP2: {
		validator: function (value) {
			return (/^\d+(\.\d{1,2})?$/).test(value);
		},
		message: '只允许输入有两位小数的正实数！'
	},
	NumP3: {
		validator: function (value) {
			return (/^\d+(\.\d{1,3})?$/).test(value);
		},
		message: '只允许输入有三位小数的正实数！'
	},
	NumP4: {
		validator: function (value) {
			return (/^(?!(0[0-9]{0,}$))[0-9]{1,}[.]{0,}[0-9]{0,}$/).test(value);
		},
		message: '只允许输入大于零的数字！'
	},
	NotZeroNum: {
		validator: function (value) {
			return (/^\+?[1-9][0-9]*$/).test(value);
		},
		message: '只允许非零的正整数！'
	},
	PostCode: {
		validator: function (value) {
			return (/^[1-9][0-9]{5}$/).test(value);
		},
		message: '请输入有效的邮政编码！'
	},
	NumPercent: {
		validator: function (value) {
			return (/^(([1-9]|[1-9][0-9])(\.\d{1,2})?|100)$/).test(value);
		},
		message: '比例应介于1-100之间,且只允许输入两位小数!'
	},
	NotSelect: {
		validator: function (value) {
			if (value != "") {
				var ishave = value.indexOf("请选择");
				return ishave;
			}
		},
		message: '请选择某个值！'
	}, equalTo: {
		param: null,
		validator: function (value) {
			var id = this.param.split(",")[0].replace(/\s/, "");
			return $("#" + id).val() == value;
		},
		message: function () {
			var paramArray = this.param.split(",");
			if (paramArray && paramArray.length > 1) {
				return paramArray[1].replace(/\s/, "");
			} else {
				return "输入的值不匹配！";
			}
		}
	}, MaxTo: {
		param: null,
		validator: function (value) {
			var id = this.param.split(",")[0].replace(/\s/, "");
			return value * 1 >= $("#" + id).val() * 1;
		},
		message: function () {
			var paramArray = this.param.split(",");
			if (paramArray && paramArray.length > 1) {
				return paramArray[1].replace(/\s/, "");
			} else {
				return "输入的值不匹配！";
			}
		}
	}
    , MinTo: {
    	param: null,
    	validator: function (value) {
    		var id = this.param.split(",")[0].replace(/\s/, "");
    		return value * 1 <= $("#" + id).val() * 1;
    	},
    	message: function () {
    		var paramArray = this.param.split(",");
    		if (paramArray && paramArray.length > 1) {
    			return paramArray[1].replace(/\s/, "");
    		} else {
    			return "输入的值不匹配！";
    		}
    	}
    }
    , requiredOrOne: //只要其中一个必填
    {
    required: true,
    param: null,
    validator: function (value) {
    	var ids = this.param.split(",");
    	var res = $.validatebox.rules.required.validator(value);
    	for (var i = 0; i < ids.length; i++) {
    		res = res || $.validatebox.rules.required.validator($("#" + ids[i]).val());
    	}
    	return res;
    },
    message: function () {
    	var ids = this.param.split(",");
    	var msg = [];
    	for (var i = 0; i < ids.length; i++) {
    		msg.push($("#" + ids[i]).attr("vmsg"));
    	}
    	return "与" + msg.join(",") + "至少一项不能为空！";
    }
   }
});

//问题：验证失败的tip一直存在
//隐藏全部提示
function Hide_AllValid(container) {
	var controls = null;
	if (container) {
		controls = $(container).find("[validtype]");
	}
	else {
		controls = $("[validtype]");
	}
	controls.each(function (i, target) {
		if ($.data(target, 'validatebox').ruleArray == null)
			return;
		var tip = $.data(target, 'validatebox').ruleArray.tip;
		if (tip) {
			tip.remove();
			$.data(target, 'validatebox').ruleArray.tip = null;
		}
		if ($(target).hasClass("valid_error"))
			$(target).removeClass("valid_error");
	});
}


//$.validatebox.rules.UppercaseLetters.me
//<input type="text" validtype="length(3,6)|required(用户名)" /><p />
//<input type="text" validtype="QQ|Num" /><p />
//<input type="text" validtype="QQ|Num"  /><p />

// if (!$.validatebox.valid()) {            
//     return;
// }

//if (!$.validatebox.valid($("#tt"))) {

//如果多个块要做验证，那就在 $.validatebox.valid( 的时候，多写几个 
//$.validatebox.valid(
//然后 联合判断


//如果是js中过程中，需要使用验证，可以采用：
//if ($.validatebox.rules.length.validator("你传入的东西")) {
//    //do
//}

//这样的方式来传递参数
//$.validatebox.rules.length.param


//$("#dd").attr("validtype", "QQ");
//$.validatebox.apply($("#dd"));


//-----------------equalTo  比较验证----------------------
//<input type="text" id="tx1" /><p />
//<input type="text" validtype="equalTo(tx1,和tx1不匹配哦)"  />//如果是equalTo(tx1) 则显示默认提示

