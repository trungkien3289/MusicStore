import Person from './person';
import axios from 'axios';
import ProjectDetailsModel from './project-details-model';
import TaskDetailsModel from './task-details-model';
import * as moment from 'moment';

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
        this.currentProject = ko.observable(new ProjectDetailsModel());
        this.currentTask = ko.observable(new TaskDetailsModel());
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
                    self.currentProject(new ProjectDetailsModel(projectDetails));
                }
            });
        });

        $(".project-task-panel .btn-task-item").bind("click", (e) => {
            var taskId = $(e.target).data("id");
            self._service.getTaskDetails(taskId).then(response => {
                console.log(response);
                if (response.status === 200) {
                    self.displayMode(ProjectDisplayMode.TASK_DETAILS);
                    var data = response.data;
                    data.StartDate = moment(new Date(data.StartDate)).format("DD/MM/YYYY hh:mm:ss");
                    data.EndDate = moment(new Date(data.EndDate)).format("DD/MM/YYYY hh:mm:ss");
                    var taskDetails = new TaskDetailsModel(data);
                    self.currentTask(new TaskDetailsModel(taskDetails));
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

    getTaskDetails(taskId) {
        return axios.get(`${this._apiBaseUrl}tasks/${taskId}`);
    }
}

