export default class UserProfile {
    constructor(userProfile) {
        this.UserId = ko.observable(userProfile ? userProfile.UserId : null);
        this.Name = ko.observable(userProfile ? userProfile.Name : "").extend({ required: true });
        this.UserName = ko.observable(userProfile ? userProfile.UserName : "");
        this.Email = ko.observable(userProfile ? userProfile.Email : "").extend({ required: true }).extend({ email: true });
        this.RoleId = ko.observable(userProfile ? userProfile.RoleId : null);
        this.Photo = ko.observable(userProfile ? userProfile.Photo : null);
        this.UserPhotoStyle = ko.computed(function () {
            return `rgba(82,92,105,.23) url('${this.Photo()}') center center no-repeat`;
        }, this);

        this.errors = ko.validation.group(this);
    }
}