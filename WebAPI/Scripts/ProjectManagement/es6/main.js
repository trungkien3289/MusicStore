import axios from 'axios';

$(document).ready(function () {
    $('.modal').modal();
    $('.datepicker').datepicker();

    this.project = new ProjectModelView(applicationPath);
    ko.applyBindings(this.project);

});

export default class ProjectModelView {
    constructor(applicationPath) {
        this._service = new Service(applicationPath);
        this._projectEditorDialog = null;

        this.initComponents();

        this.Id = ko.observable("");
        this.Name = ko.observable("");
        this.Description = ko.observable("");
        this.StartDate = ko.observable();
        this.EndDate = ko.observable();
        this.Status = ko.observable();
        this.Leaders = ko.observable([]);
        this.Developers = ko.observable([]);
        this.availableDevelopers = ko.observableArray([]);

        this.newProject = () => {
            var self = this;

            this.getAvailableDevelopers(function () {
                self._projectEditorDialog.open();
            });
        };
        this.cancelSaveProject = () => {
            this.Id("");
            this.Name("");
            this.Description("");
            this.StartDate("");
            this.EndDate("");

            this._projectEditorDialog.close();
        };
        this.saveProject = () => {
            var self = this;

            console.log({
                Name: self.Name(),
                Description: self.Description(),
                EndDate: self.EndDate(),
                StartDate: self.StartDate(),
                Status: self.Status(),
                Leaders: self.Leaders(),
                Developers: self.Developers()
            });
            self._service.addProject({
                Name: self.Name(),
                Description: self.Description(),
                EndDate: self.EndDate(),
                StartDate: self.StartDate(),
                Status: self.Status(),
                //Leaders: self.Leaders(),
                //Developers: self.Developers()
            }).then(response => {
                console.log(response);
                if (response.status === 200) {
                    location.reload();
                }
            });

            this.Id("");
            this.Name("");
            this.Description("");
            this.StartDate("");
            this.EndDate("");

            this._projectEditorDialog.open();
        };

        this.errorMessage = ko.observable("");
    }

    initComponents() {
        $('#projectEditor').modal({
            dismissible: false,
            onOpenStart: function () {
                $('#projectEditor select').formSelect();
            }
        });
        this._projectEditorDialog = M.Modal.getInstance($("#projectEditor"));
    }

    getAvailableDevelopers(callback) {
        var self = this;

        return self._service.getAvailableDevelopers()
            .then(response => {
                if (response.status === 200) {
                    self.availableDevelopers(response.data);
                    callback();
                }
            }).catch(response => {
                self.errorMessage(response.response.data);
            });
    }
}

export class Service {
    constructor(applicationPath) {
        this._apiBaseUrl = `${applicationPath}/api/`;
    }

    addProject(projectModelView) {
        return axios.post(
            `${this._apiBaseUrl}addProject`,
            projectModelView,
            {
                withCredentials: true
            }
        );
    }

    getAvailableDevelopers() {
        return axios.get(
            `${this._apiBaseUrl}developers/available`,
            {
                withCredentials: true
            }
        );
    }
}
