﻿export default class Utils {
    static getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    static isStringNullOrEmpty(string) {
        return (string == null || string.trim() == "");
    }

    static showLoading() {
        $("#loading").show();
    }

    static hideLoading() {
        $("#loading").hide();
    }
}