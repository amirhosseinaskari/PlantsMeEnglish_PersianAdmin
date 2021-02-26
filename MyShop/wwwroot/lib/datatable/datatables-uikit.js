﻿/*!
 DataTables UIkit 3 integration
*/
var $jscomp = $jscomp || {}; $jscomp.scope = {}; $jscomp.findInternal = function (a, b, c) { a instanceof String && (a = String(a)); for (var e = a.length, d = 0; d < e; d++) { var k = a[d]; if (b.call(c, k, d, a)) return { i: d, v: k } } return { i: -1, v: void 0 } }; $jscomp.ASSUME_ES5 = !1; $jscomp.ASSUME_NO_NATIVE_MAP = !1; $jscomp.ASSUME_NO_NATIVE_SET = !1; $jscomp.SIMPLE_FROUND_POLYFILL = !1;
$jscomp.defineProperty = $jscomp.ASSUME_ES5 || "function" == typeof Object.defineProperties ? Object.defineProperty : function (a, b, c) { a != Array.prototype && a != Object.prototype && (a[b] = c.value) }; $jscomp.getGlobal = function (a) { return "undefined" != typeof window && window === a ? a : "undefined" != typeof global && null != global ? global : a }; $jscomp.global = $jscomp.getGlobal(this);
$jscomp.polyfill = function (a, b, c, e) { if (b) { c = $jscomp.global; a = a.split("."); for (e = 0; e < a.length - 1; e++) { var d = a[e]; d in c || (c[d] = {}); c = c[d] } a = a[a.length - 1]; e = c[a]; b = b(e); b != e && null != b && $jscomp.defineProperty(c, a, { configurable: !0, writable: !0, value: b }) } }; $jscomp.polyfill("Array.prototype.find", function (a) { return a ? a : function (a, c) { return $jscomp.findInternal(this, a, c).v } }, "es6", "es3");
(function (a) { "function" === typeof define && define.amd ? define(["jquery", "datatables.net"], function (b) { return a(b, window, document) }) : "object" === typeof exports ? module.exports = function (b, c) { b || (b = window); c && c.fn.dataTable || (c = require("datatables.net")(b, c).$); return a(c, b, b.document) } : a(jQuery, window, document) })(function (a, b, c, e) {
    var d = a.fn.dataTable; a.extend(!0, d.defaults, {
        dom: "<'row uk-grid'<'uk-width-1-2'l><'uk-width-1-2'f>><'row uk-grid dt-merge-grid'<'uk-width-1-1'tr>><'row uk-grid dt-merge-grid'<'uk-width-2-5'i><'uk-width-3-5'p>>",
        renderer: "uikit"
    }); a.extend(d.ext.classes, { sWrapper: "dataTables_wrapper uk-form dt-uikit", sFilterInput: "uk-form-small uk-input", sLengthSelect: "uk-form-small uk-select", sProcessing: "dataTables_processing uk-panel" }); d.ext.renderer.pageButton.uikit = function (b, e, v, p, l, r) {
        var k = new d.Api(b), w = b.oClasses, m = b.oLanguage.oPaginate, x = b.oLanguage.oAria.paginate || {}, g, f, t = 0, u = function (c, d) {
            var e, n = function (b) { b.preventDefault(); a(b.currentTarget).hasClass("disabled") || k.page() == b.data.action || k.page(b.data.action).draw("page") };
            var q = 0; for (e = d.length; q < e; q++) {
                var h = d[q]; if (a.isArray(h)) u(c, h); else {
                    f = g = ""; switch (h) {
                        case "ellipsis": g = '<i class="uk-icon-ellipsis-h"></i>'; f = "uk-disabled disabled"; break; case "first": g = '<i class="uk-icon-angle-double-left"></i> ' + m.sFirst; f = 0 < l ? "" : " uk-disabled disabled"; break; case "previous": g = '<i class="uk-icon-angle-left"></i> ' + m.sPrevious; f = 0 < l ? "" : "uk-disabled disabled"; break; case "next": g = m.sNext + ' <i class="uk-icon-angle-right"></i>'; f = l < r - 1 ? "" : "uk-disabled disabled"; break; case "last": g =
                            m.sLast + ' <i class="uk-icon-angle-double-right"></i>'; f = l < r - 1 ? "" : " uk-disabled disabled"; break; default: g = h + 1, f = l === h ? "uk-active" : ""
                    }if (g) { var p = a("<li>", { "class": w.sPageButton + " " + f, id: 0 === v && "string" === typeof h ? b.sTableId + "_" + h : null }).append(a(-1 != f.indexOf("disabled") || -1 != f.indexOf("active") ? "<span>" : "<a>", { href: "#", "aria-controls": b.sTableId, "aria-label": x[h], "data-dt-idx": t, tabindex: b.iTabIndex }).html(g)).appendTo(c); b.oApi._fnBindAction(p, { action: h }, n); t++ }
                }
            }
        }; try { var n = a(e).find(c.activeElement).data("dt-idx") } catch (y) { } u(a(e).empty().html('<ul class="uk-pagination uk-pagination-right uk-flex-right"/>').children("ul"),
            p); n && a(e).find("[data-dt-idx=" + n + "]").focus()
    }; return d
});