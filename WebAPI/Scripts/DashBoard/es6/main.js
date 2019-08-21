import Person from './person';
import axios from 'axios';
import ProjectDetailsModel from './project-details-model';
$(document).ready(function () {
    var dashboardManagement = new DashboardManagement();
    ko.applyBindings(dashboardManagement);
});

export default class DashboardManagement {
    constructor() {
        this._service = new Service();
        this.init();
        this.curentProject = ko.observable(new ProjectDetailsModel(), document.getElementById("projectPanel"));
        this.ProjectStatus = "Karl";
    }

    init() {
        this.bindEventProjectTaskPanel();
    }
    bindEventProjectTaskPanel() {
        var self = this;
        $(".project-task-panel .btn-project-item").bind("click", (e) => {
            var projectId = $(e.target).data("id");
            self._service.getProjectDetails(projectId).then(response => {
                console.log(response);
                if (response.status === 200) {
                    var projectDetails = new ProjectDetailsModel(response.data);
                    self.curentProject(new ProjectDetailsModel(projectDetails));
                }
            });
        });
    }
}

export class Service {
    constructor() {
        this._apiBaseUrl = `/api/`;
    }

    getProjectDetails(projectId) {
        return axios.get(`${this._apiBaseUrl}projects/${projectId}`);
    }
}

