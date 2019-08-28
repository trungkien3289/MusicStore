import axios from 'axios';

$(document).ready(function () {
    ko.applyBindings(new RegisterPage(host, applicationPath));
});

export default class RegisterPage {
    constructor(host, applicationPath) {
        this.bindEvents();
        this.email = ko.observable("");
        this.userName = ko.observable("");
        this.password = ko.observable("");
        this.rePassword = ko.observable("");
        this.errorMessage = ko.observable("");
        this.host = host;
        if (Utils.isStringNullOrEmpty(applicationPath)) {
            this._apiBaseUrl = `api/`;
        } else {
            this._apiBaseUrl = `${applicationPath}api/`;
        }
    }
    bindEvents() {
        var self = this;
        $("#btnRegister").click(function () {
            if (!self.validate()) return;
            self.requestRegister().then(response => {
                console.log(response);
                if (response.status === 200) {
                    var token = response.request.getResponseHeader('Token');
                    var tokenExpiry = response.request.getResponseHeader('TokenExpiry');
                    //sessionStorage.setItem('accessToken', data.access_token);
                    var myDate = new Date();
                    myDate.setMonth(myDate.getSeconds() + tokenExpiry);
                    document.cookie = `Token=${token}; TokenExpiry=${tokenExpiry};expires=${myDate};domain=${self.host};`;
                    window.location.replace("/");
                }
            }).catch(error => {
                self.errorMessage(error.response.data);
            });
        });
    }

    validate() {
        if (this.userName().trim() == "" || this.password().trim() == "" || this.email().trim() == "") {
            this.errorMessage("UserName, Password and Email is required.");
            return false;
        }

        if (!this.validateEmail(this.email())) {
            this.errorMessage("Email invalid.");
            return false;
        }

        if (this.password() != this.rePassword()) {
            this.errorMessage("Password is not matched.");
            return false;
        }

        return true;
    }

    validateEmail(email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }

    requestRegister() {
        return axios.post(
            `${this._apiBaseUrl}post/register`,
            {
                userName: this.userName(),
                password: this.password(),
                email: this.email()
            }
        );
    }
}