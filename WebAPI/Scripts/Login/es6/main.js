import axios from 'axios';
import Utils from '../../Common/es6/utils';

$(document).ready(function () {
    ko.applyBindings(new LoginPage(returnUrl, host, applicationPath));
});

export default class LoginPage {
    constructor(returnUrl, host, applicationPath) {
        this.bindEvents();
        this.userName = ko.observable("");
        this.password = ko.observable("");
        this.authorizationHeader = ko.computed(function () {
            return "BasicCustom " + btoa(this.userName() + ":" + this.password());
        }, this);
        this.errorMessage = ko.observable("");
        this.returnUrl = returnUrl;
        this.host = host;

        if (Utils.isStringNullOrEmpty(applicationPath)) {
            this._apiBaseUrl = `api/`;
        } else {
            this._apiBaseUrl = `${applicationPath}api/`;
        }
    }
    bindEvents() {
        var self = this;

        $("#btnLogin").click(function () {
            if (!self.validate()) return;
            self.requestLogin().then(response => {
                console.log(response);
                switch(response.status) {
                    case 200: {
                        var token = response.request.getResponseHeader('Token');
                        var tokenExpiry = response.request.getResponseHeader('TokenExpiry');
                        //sessionStorage.setItem('accessToken', data.access_token);
                        var myDate = new Date();
                        myDate.setMonth(myDate.getSeconds() + tokenExpiry);
                        document.cookie = `Token=${token}; TokenExpiry=${tokenExpiry};expires=${myDate};domain=${self.host};`;
                        window.location.replace(self.returnUrl);
                        break;
                    }
                    default: {
                        self.errorMessage("status is not handle");
                    }
                }

            }).catch(response => {
                switch (response.response.status) {
                    case 401: {
                        self.errorMessage("UserName or Password is invalid.");
                        break;
                    }
                    default: {
                        self.errorMessage(response.response.data);
                    }
                }
            });
        });
    }

    validate() {
        if (this.userName().trim() == "" || this.password().trim() == "") {
            this.errorMessage("UserName and Password is required.");
            return false;
        }

        return true;
    }

    requestLogin() {
        return axios.post(
            `${this._apiBaseUrl}get/token`,
            {
                userName: this.userName(),
                password: this.password()
            },
            {
                headers: {
                    Authorization: this.authorizationHeader()
                }
            }
        );
    }
}