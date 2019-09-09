import axios from 'axios';
import ProjectDetailsModel from './project-details-model';
import TaskDetailsModel from './task-details-model';
import TaskRequestModel from './task-request-model';
import DashBoardModel from './dashboard-model';
import TaskRequestProjectModel from './taskrequest-project-model';
import { format } from 'date-fns';
import Utils from '../../Common/es6/utils';
import * as Toastr from 'toastr';

$(document).ready(function () {
    var dashboardManagement = new DashboardManagement(applicationPath);
    ko.applyBindings(dashboardManagement, document.body);
});

const ProjectDisplayMode = {
    DASHBOARD: 0,
    PROJECT_DETAILS: 1,
    TASK_DETAILS: 2,
    TASK_REQUEST: 3,
};

export default class DashboardManagement {
    constructor(applicationPath) {
        this.applicationPath = applicationPath;
        // configure toastr
        Toastr.options.closeButton = true;
        Toastr.options.closeMethod = 'fadeOut';
        Toastr.options.closeDuration = 300;
        this._service = new Service(applicationPath);
        this.taskRequestProjects = ko.observableArray([]);
        this.buildLeftPanel();
        this.getDashBoardData();
        this.dashboard = ko.observable(new DashBoardModel());
        this.currentProject = ko.observable(new ProjectDetailsModel());
        this.currentTask = ko.observable(new TaskDetailsModel());
        this.currentTaskRequest = ko.observable(new TaskRequestModel());
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
        this.joinTaskRequestBtnClick = this.joinTaskRequestHandler.bind(this);
    }

    buildLeftPanel() {
        var self = this;
        Utils.showLoading();
        this._service.getTaskRequestProjects()
            .then(response => {
                let projectModels = response.data.map(prj => {
                    return new TaskRequestProjectModel(prj);
                });
                self.taskRequestProjects(projectModels);
                self.bindEventProjectTaskPanel();
            });
    }

    joinTaskRequestHandler() {
        var self = this;
        let taskRequestId = self.currentTaskRequest().Id;
        self._service.requestJoinTaskRequest(taskRequestId)
            .then(response => {
                Toastr.success("Join task request successfully.");
                // reload task request details
                self._service.getTaskRequestDetails(taskRequestId)
                    .then(response => {
                        self.updateTaskRequestDetailsUI(response.data.TaskRequestDetails, response.data.IsJoin);
                        $(".left-panel .tree-node-btn").removeClass("selected");
                        $(e.target).addClass("selected");
                    });
                // update status of task request on left panel
            });
    }

    bindEventProjectTaskPanel() {
        var self = this;
        $(".left-panel .btn-project-item").bind("click", function(e) {
            var projectId = $(this).data("id");
            self._service.getProjectDetails(projectId)
            .then(response => {
                self.displayMode(ProjectDisplayMode.PROJECT_DETAILS);
                var data = response.data;
                data.StartDate = format(new Date(data.StartDate), "dd/MM/yyyy hh:mm:ss");
                data.EndDate = format(new Date(data.EndDate), "dd/MM/yyyy hh:mm:ss");
                var projectDetails = new ProjectDetailsModel(data);
                self.currentProject(new ProjectDetailsModel(projectDetails));

                $(".left-panel .tree-node-btn").removeClass("selected");
                $(this).addClass("selected");
            });
        });

        $(".project-task-panel .btn-task-item").bind("click", function(e) {
            var taskId = $(this).data("id");
            self._service.getTaskDetails(taskId)
                .then(response => {
                    self.displayMode(ProjectDisplayMode.TASK_DETAILS);
                    var data = response.data;
                    data.StartDate = format(new Date(data.StartDate), "dd/MM/yyyy hh:mm:ss");
                    data.EndDate = format(new Date(data.EndDate), "dd/MM/yyyy hh:mm:ss");
                    var taskDetails = new TaskDetailsModel(data);
                    self.currentTask(new TaskDetailsModel(taskDetails));

                    $(".left-panel .tree-node-btn").removeClass("selected");
                    $(this).addClass("selected");
                });
        });

        $(".project-request-panel .btn-task-request-item").bind("click", function(e){
            var taskRequestId = $(this).data("id");
            self._service.getTaskRequestDetails(taskRequestId)
                .then(response => {
                    self.updateTaskRequestDetailsUI(response.data.TaskRequestDetails, response.data.IsJoin);
                    $(".left-panel .tree-node-btn").removeClass("selected");
                    $(this).addClass("selected");
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
        this._service.getDashBoardData().then(response => {
            self.displayMode(ProjectDisplayMode.DASHBOARD);
            self.dashboard(new DashBoardModel(response.data));
        });
    }

    convertDateTime(dateString, format) {
        return format(new Date(dateString), format);
    }

    updateTaskRequestDetailsUI(taskRequestData, isJoin) {
        var self = this;
        self.displayMode(ProjectDisplayMode.TASK_REQUEST);
        taskRequestData.Task.StartDate = self.convertDateTime(taskRequestData.Task.StartDate, "dd/MM/yyyy hh:mm:ss");
        taskRequestData.Task.EndDate = self.convertDateTime(taskRequestData.Task.EndDate, "dd/MM/yyyy hh:mm:ss");
        self.currentTaskRequest(new TaskRequestModel(taskRequestData, isJoin));
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

    getDashBoardData() {
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
            `${this._apiBaseUrl}task/requests/${taskRequestId}/byuser`,
            {
                withCredentials: true
            }
        );
    }

    requestJoinTaskRequest(taskRequestId) {
        return axios.post(
            `${this._apiBaseUrl}taskrequest/${taskRequestId}/requestjoin`,
            {
                withCredentials: true
            }
        );
    }

    getTaskRequestProjects() {
        return axios.get(
            `${this._apiBaseUrl}dashboard/projects/withtaskrequest`,
            {
                withCredentials: true
            }
        );
    }
}
