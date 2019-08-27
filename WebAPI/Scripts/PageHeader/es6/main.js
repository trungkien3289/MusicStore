import axios from 'axios';

$(document).ready(function () {
    new PageHeader(applicationPath);
});

export default class PageHeader {
    constructor(applicationPath) {
        this.applicationPath = applicationPath;
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
            `${this.applicationPath}/api/signout`, 
            {
                withCredentials: true
            }
        );
    }
}