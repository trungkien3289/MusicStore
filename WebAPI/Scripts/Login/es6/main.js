import axios from 'axios';
import Utils from '../../Common/es6/utils';

$(document).ready(function () {
    ko.applyBindings(new LoginPage(returnUrl, host, applicationPath));

    $("#UserName").focus();
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
        this.applicationPath = applicationPath;

        this.initAxios();
    }
    initAxios() {
        var self = this;
        // Add a request interceptor
        axios.interceptors.request.use((config) => {
            // Do something before request is sent
            Utils.showLoading();
            return config;
        }, (error) => {
            Utils.hideLoading();
            // Do something with request error
            return Promise.reject(error);
        });

        // Add a response interceptor
        axios.interceptors.response.use((response) => {
            Utils.hideLoading();
            // Do something with response data
            return response;
        }, (error) => {
            if (error.response) {
                if (error.response.status == 401) {
                    let returnUrl = Utils.isStringNullOrEmpty(self.applicationPath) ? "/" : `${self.applicationPath}`;
                    window.location.replace(returnUrl);
                }
                Toastr.error(error.response.data.Message, "Error");
                console.log(error.response.data);
                console.log(error.response.status);
                console.log(error.response.headers);
            } else if (error.request) {
                Toastr.error(error.request, "Error");
                console.log(error.request);
            } else {
                Toastr.error(`Error ${error.message}`, "Error");
                console.log('Error', error.message);
            }
            console.log(error.config);
            Utils.hideLoading();
            // Do something with response error
            return Promise.reject(error);
        });
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

        $("#Password")
            .on("keyup", function (event) {
                event.preventDefault();
                if (event.keyCode === 13) {
                    $("#btnLogin").click();
                }
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
        let authentication = this.authorizationHeader();
        return axios.post(
            `${this._apiBaseUrl}get/token`,
            {
                userName: this.userName(),
                password: this.password()
            },
            {
                headers: { 'Authorization': authentication }
            },
            
                //withCredentials: true
        );
    }
}