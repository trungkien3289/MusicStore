import axios from 'axios';
import Utils from '../../Common/es6/utils';
import UserProfile from './user-profile';
import ChangePasswordModel from './change-password-model';
import * as Toastr from 'toastr';

$(document).ready(function () {
    Utils.initKoValidation();
    var pageHeader = new PageHeader(applicationPath);
    ko.applyBindings(pageHeader, document.getElementById("profileDialog"));
});

export default class PageHeader {
    constructor(applicationPath) {
        if (Utils.isStringNullOrEmpty(applicationPath)) {
            this._apiBaseUrl = `api/`;
        } else {
            this._apiBaseUrl = `${applicationPath}api/`;
        }
        this.applicationPath = applicationPath;
        this._service = new Service(applicationPath);

        this.IsEditingAvartar = ko.observable(false);
        this.IsEditingProfile = ko.observable(false);
        this.IsChangingPassword = ko.observable(false);
        this.UserProfile = ko.observable(new UserProfile());
        this.ChangePasswordModel = ko.observable(new ChangePasswordModel());
        this.UserProfileData = null;
        this.initComponents();
        this.bindEvents();
        this.getUserProfile();
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
            let returnUrl = Utils.isStringNullOrEmpty(self.applicationPath) ? "/" : `${self.applicationPath}`;
            window.location.replace(returnUrl);
        });

        $("#btn_AccountSetting").bind("click", function () {
            self._dialog.open();
        });

        $("#btn_CloseProfileDialog").bind("click", function () {
            self._dialog.close();
        });

        $("#btnUploadAvartar").bind("click", function () {
            $("#pf_UploadImageFile").click();
        });

        $("#pf_UploadImageFile").on("change", function () {
            self.readFile(this);
        });

        $("#pf_SaveChangeAvartar").on("click", function (ev) {
            self._uploadAvartarControl.croppie('result', {
                type: 'canvas',
                size: 'viewport'
            }).then(function (resp) {
                console.log(resp);
                self._service.updateUserPhoto(resp).then(function () {
                    self.setUserPhotoClient(resp);
                    self.IsEditingAvartar(false);
                    $("#pf_UploadImageFile").val('');
                });
            });
        });

        $("#pf_CancelChangeAvartar").on("click", function () {
            self.IsEditingAvartar(false);
            $("#pf_UploadImageFile").val('');
        });

        $(".btn-remove-avatar").on("click", function () {
            self._service.resetUserPhoto().then((response) => {
                self.setUserPhotoClient(Utils.getDefaultUserPhoto());
            });
        });

        $("#pf_SaveChangeProfile").on("click", function () {
            if (self.UserProfile().errors().length > 0) {
                self.UserProfile().errors.showAllMessages();
            } else {
                self.updateProfile();
            }
        });

        $("#pf_CancelChangeProfile").on("click", function () {
            self.UserProfile(new UserProfile(self.UserProfileData));
            self.IsEditingProfile(false);
        });

        $("#pf_BtnEditProfile").on("click", function () {
            self.IsEditingProfile(true);
        });

        $("#pf_BtnChangePassword").on("click", function () {
            var changePasswordModel = ko.toJS(self.ChangePasswordModel());
            self._service.updatePassword(changePasswordModel).then(function () {
                Toastr.success("Change password successfully.");
                self.ChangePasswordModel(new ChangePasswordModel());
            });
        });

        $("#pf_BtnResetChangePassword").on("click", function () {
            self.ChangePasswordModel(new ChangePasswordModel());
        });
    }

    readFile(input) {
        var self = this;
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                self._uploadAvartarControl.croppie('bind', {
                    url: e.target.result
                }).then(function () {
                    self.IsEditingAvartar(true);
                });
            };

            reader.readAsDataURL(input.files[0]);
        }
        else {
            Toastr.error("Please select file or you're browser doesn't support the FileReader API");
        }
    }

    initComponents() {
        var self = this;
        $('#profileDialog').modal({
            dismissible: false,
            endingTop: '0%',
            inDuration: 500,
            outDuration: 500,
            onOpenStart: function () {
            }
        });
        this._dialog = M.Modal.getInstance($("#profileDialog"));

        this._uploadAvartarControl = $("#uploadAvartarControl").croppie({
            enableExif: true,
            viewport: {
                width: 200,
                height: 200,
                type: 'circle'
            },
            boundary: {
                width: 300,
                height: 300
            }
        });
    }

    deleteAllCookies() {
        //var cookies = document.cookie.split(";");

        //for (var i = 0; i < cookies.length; i++) {
        //    var cookie = cookies[i];
        //    var eqPos = cookie.indexOf("=");
        //    var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        //    document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
        //}

        document.cookie.split(";").forEach(function (c) { document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/"); });

        //var cookies = document.cookie.split(";");
        //for (var i = 0; i < cookies.length; i++)
        //    eraseCookie(cookies[i].split("=")[0]);
    }

    requestSignOut() {
        return axios.post(
            `${this._apiBaseUrl}signout`, 
            {
                withCredentials: true
            }
        );
    }

    getUserProfile() {
        var self = this;
        this._service.getUserProfile()
            .then(function (response) {
                self.UserProfile(new UserProfile(response.data));
                self.UserProfileData = response.data;
            });
    }

    updateProfile() {
        var self = this;
        // call api
        var editProfileModel = ko.toJS(this.UserProfile());
        this._service.updateUserProfile(editProfileModel)
            .then(function (response) {
                self.UserProfile(new UserProfile(response.data));
                self.UserProfileData = response.data;
                self.IsEditingProfile(false);
            });
    }

    setUserPhotoClient(photo) {
        this.UserProfile().Photo(photo);
        this.UserProfileData.Photo = photo;
    }
}

export class Service {
    constructor(applicationPath) {
        var self = this;
        this.applicationPath = applicationPath;
        if (Utils.isStringNullOrEmpty(applicationPath)) {
            this._apiBaseUrl = `api/`;
        } else {
            this._apiBaseUrl = `${applicationPath}api/`;
        }

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

    getUserProfile() {
        return axios.get(
            `${this._apiBaseUrl}users/profile`,
            {
                withCredentials: true
            }
        );
    }

    updateUserProfile(userProfile) {
        return axios.put(
            `${this._apiBaseUrl}users/profile`,
            userProfile,
            {
                withCredentials: true
            }
        );
    }

    updateUserPhoto(photo) {
        return axios.post(
            `${this._apiBaseUrl}users/updatephoto`,
            { Photo: photo },
            {
                withCredentials: true,
                headers: { 'Content-Type': 'application/json' }
            }
        );
    }

    resetUserPhoto() {
        return axios.put(
            `${this._apiBaseUrl}users/resetphoto`,
            {
                withCredentials: true
            }
        );
    }

    updatePassword(changePasswordModel) {
        return axios.put(
            `${this._apiBaseUrl}users/changepassword`,
            changePasswordModel,
            {
                withCredentials: true
            }
        );
    }
}