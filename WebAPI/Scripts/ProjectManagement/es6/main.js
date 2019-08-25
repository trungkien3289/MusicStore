import axios from 'axios';

$(document).ready(function () {
    $('.modal').modal();
    $('.datepicker').datepicker();
    $('select').formSelect();

    this.project = new ProjectModelView();
    ko.applyBindings(this.project);

});

export default class ProjectModelView {
    constructor(Id, Name, Description, StartDate, EndDate, Status, Leaders, Developers) {
        this._service = new Service();

        this.Id = ko.observable(Id);
        this.Name = ko.observable(Name);
        this.Description = ko.observable(Description);
        this.StartDate = ko.observable(StartDate);
        this.EndDate = ko.observable(EndDate);
        this.Status = ko.observable(Status);
        this.Leaders = ko.observable(Leaders);
        this.Developers = ko.observable(Developers);

        this.cancelSaveProject = () => {
            this.Id("");
            this.Name("");
            this.Description("");
            this.StartDate("");
            this.EndDate("");

            $('#projectEditor').modal('close');
        };
        this.saveProject = () => {
            var self = this;

            self._service.addProject({
                Name: self.Name(),
                Description: self.Description(),
                EndDate: self.EndDate(),
                StartDate: self.StartDate(),
                Status: self.Status()
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

            $('#projectEditor').modal('close');
        };
    }
}

export class Service {
    constructor() {
        this._apiBaseUrl = `/api/`;
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
}
