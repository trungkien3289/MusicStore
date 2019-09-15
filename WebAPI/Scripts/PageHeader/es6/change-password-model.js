export default class ChangePasswordModel {
    constructor(model) {
        this.OldPassword = ko.observable(model ? model.OldPassword : "").extend({ required: true });
        this.NewPassword = ko.observable(model ? model.NewPassword : "").extend({ required: true });
        this.ConfirmNewPassword = ko.observable(model ? model.ConfirmNewPassword : "").extend({ required: true })
            .extend({ areSame: { params: this.NewPassword, message: "Confirm password must match password" } });

        this.errors = ko.validation.group(this);
    }
}