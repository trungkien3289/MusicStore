import Person from './person';
import axios from 'axios';
import ProjectDetailsModel from './project-details-model';
$(document).ready(function () {
    var dashboardManagement = new DashboardManagement();
    ko.applyBindings(dashboardManagement);
});

const ProjectDisplayMode = {
    DASHBOARD: 0,
    PROJECT_DETAILS: 1,
    TASK_DETAILS: 2,
    TASK_REQUEST: 3,
};

export default class DashboardManagement {
    constructor() {
        this._service = new Service();
        this.bindEventProjectTaskPanel();
        this.curentProject = ko.observable(new ProjectDetailsModel());
        this.ProjectStatus = "Karl";
        this.displayMode = ko.observable(ProjectDisplayMode.DASHBOARD);
        this.isShowProjectDetails = ko.computed(function () {
            return this.displayMode() == ProjectDisplayMode.PROJECT_DETAILS;
        }, this);
        this.isShowTaskDetails = ko.computed(function () {
            return this.displayMode() == ProjectDisplayMode.TASK_DETAILS;
        }, this);
        this.isShowTaskRequest = ko.computed(function () {
            return this.displayMode() == ProjectDisplayMode.TASK_REQUEST;
        }, this);
        this.isShowDashboard = ko.computed(function () {
            return this.displayMode() == ProjectDisplayMode.DASHBOARD;
        }, this);
    }
    bindEventProjectTaskPanel() {
        var self = this;
        $(".project-task-panel .btn-project-item").bind("click", (e) => {
            var projectId = $(e.target).data("id");
            self._service.getProjectDetails(projectId).then(response => {
                console.log(response);
                if (response.status === 200) {
                    self.displayMode(ProjectDisplayMode.PROJECT_DETAILS);
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

