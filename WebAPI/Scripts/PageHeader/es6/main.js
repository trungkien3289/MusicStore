import axios from 'axios';

$(document).ready(function () {
    new PageHeader();
});

export default class PageHeader {
    constructor() {
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
            window.location.replace("/");
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
            '/api/signout', 
            {
                withCredentials: true
            }
        );
    }
}