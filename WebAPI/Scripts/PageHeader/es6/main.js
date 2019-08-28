import axios from 'axios';
import Utils from '../../Common/es6/utils';

$(document).ready(function () {
    new PageHeader(applicationPath);
});

export default class PageHeader {
    constructor(applicationPath) {
        if (Utils.isStringNullOrEmpty(applicationPath)) {
            this._apiBaseUrl = `api/`;
        } else {
            this._apiBaseUrl = `${applicationPath}api/`;
        }
        this.bindEvents();
    }
    bindEvents() {
        var self = this;

        $("#btn_SignOut").click(function () {
            //self.requestSignOut().then(response => {
            //    console.log(response);
            //    if (response.status === 200) {
            //        self.deleteAllCookies();
            //        window.location.replace("/");
            //    }
            //}).catch(error => {
            //    self.errorMessage(error.response.data);
            //    });

            self.deleteAllCookies();
            window.location.replace(`${self.applicationPath}/`);
        });
    }

    deleteAllCookies() {
        var cookies = document.cookie.split(";");

        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i];
            var eqPos = cookie.indexOf("=");
            var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
            document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
        }
    }

    requestSignOut() {
        return axios.post(
            `${this._apiBaseUrl}signout`, 
            {
                withCredentials: true
            }
        );
    }
}