﻿import Person from './person';
import axios from 'axios';
import ProjectDetailsModel from './project-details-model';
import TaskDetailsModel from './task-details-model';
import TaskRequestModel from './task-request-model';
import DashBoardModel from './dashboard-model';
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
        this.getDashBoardData();
        this.dashboard = ko.observable(new DashBoardModel());
        this.currentProject = ko.observable(new ProjectDetailsModel());
        this.currentTask = ko.observable(new TaskDetailsModel());
        this.currentTaskRequest = ko.observable(new TaskRequestModel());
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
                    var data = response.data;
                    data.StartDate = moment(new Date(data.StartDate)).format("DD/MM/YYYY hh:mm:ss");
                    data.EndDate = moment(new Date(data.EndDate)).format("DD/MM/YYYY hh:mm:ss");
                    var projectDetails = new ProjectDetailsModel(data);
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

        $(".project-request-panel .btn-task-request-item").bind("click", (e) => {
            var taskRequestId = $(e.target).data("id");
            self._service.getTaskRequestDetails(taskRequestId).then(response => {
                console.log(response);
                if (response.status === 200) {
                    self.displayMode(ProjectDisplayMode.TASK_REQUEST);
                    var data = response.data;
                    data.Task.StartDate = self.convertDateTime(data.Task.StartDate, "DD/MM/YYYY hh:mm:ss");
                    data.Task.EndDate = self.convertDateTime(data.Task.EndDate, "DD/MM/YYYY hh:mm:ss");
                    var taskRequestDetails = new TaskRequestModel(data);
                    self.currentTaskRequest(new TaskRequestModel(taskRequestDetails));
                }
            });
        });

    }

    getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    getDashBoardData() {
        var self = this;
        this._service.getDashBoardData(this.getCookie("Token")).then(response => {
            console.log(response);
            if (response.status === 200) {
                self.displayMode(ProjectDisplayMode.DASHBOARD);
                self.dashboard(new DashBoardModel(response.data));
            }
        });
    }

    convertDateTime(dateString, format) {
        return moment(new Date(dateString)).format(format);
    }
}

export class Service {
    constructor() {
        this._apiBaseUrl = `/api/`;
    }

    getDashBoardData(token) {
        return axios.get(
            `${this._apiBaseUrl}dashboard/data`,
            {
                withCredentials: true
            }
        );
    }

    getProjectDetails(projectId) {
        return axios.get(
            `${this._apiBaseUrl}projects/${projectId}`,
            {
                withCredentials: true
            }
        );
    }

    getTaskDetails(taskId) {
        return axios.get(
            `${this._apiBaseUrl}tasks/${taskId}`,
            {
                withCredentials: true
            }
        );
    }

    getTaskRequestDetails(taskRequestId) {
        return axios.get(
            `${this._apiBaseUrl}task/requests/${taskRequestId}`,
            {
                withCredentials: true
            }
        );
    }
}
