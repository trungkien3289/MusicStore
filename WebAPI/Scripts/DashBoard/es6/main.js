import Person from './person';
import axios from 'axios';
import ProjectDetailsModel from './project-details-model';
import TaskDetailsModel from './task-details-model';
import TaskRequestModel from './task-request-model';
import DashBoardModel from './dashboard-model';
import TaskRequestProjectModel from './taskrequest-project-model';
import * as moment from 'moment';
import Utils from '../../Common/es6/utils';

$(document).ready(function () {
    var dashboardManagement = new DashboardManagement(applicationPath);
    ko.applyBindings(dashboardManagement);
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
        this._service.getTaskRequestProjects()
            .then(response => {
                if (response.status === 200) {
                    let projectModels = response.data.map(prj => {
                        return new TaskRequestProjectModel(prj);
                    });
                    self.taskRequestProjects(projectModels);
                    self.bindEventProjectTaskPanel();
                }
            })
            .catch(error => {
                if (error.response) {
                    console.log(error.response.data);
                    console.log(error.response.status);
                    console.log(error.response.headers);
                } else if (error.request) {
                    console.log(error.request);
                } else {
                    console.log('Error', error.message);
                }
                console.log(error.config);
            });
    }

    joinTaskRequestHandler() {
        var self = this;
        let taskRequestId = self.currentTaskRequest().Id;
        self._service.requestJoinTaskRequest(taskRequestId)
        .then(response => {
            if (response.status === 200) {
                // reload task request details
                self._service.getTaskRequestDetails(taskRequestId)
                    .then(response => {
                        console.log(response);
                        if (response.status === 200) {
                            self.updateTaskRequestDetailsUI(response.data.TaskRequestDetails, response.data.IsJoin);
                            $(".left-panel .tree-node-btn").removeClass("selected");
                            $(e.target).addClass("selected");
                        }
                    }).catch(error => {
                        if (error.response) {
                            console.log(error.response.data);
                            console.log(error.response.status);
                            console.log(error.response.headers);
                        } else if (error.request) {
                            console.log(error.request);
                        } else {
                            console.log('Error', error.message);
                        }
                        console.log(error.config);
                    });
                // update status of task request on left panel
            }
        }).catch(error => {
            if (error.response) {
                console.log(error.response.data);
                console.log(error.response.status);
                console.log(error.response.headers);
            } else if (error.request) {
                console.log(error.request);
            } else {
                console.log('Error', error.message);
            }
            console.log(error.config);
        });
    }

    bindEventProjectTaskPanel() {
        var self = this;
        $(".left-panel .btn-project-item").bind("click", (e) => {
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

                    $(".left-panel .tree-node-btn").removeClass("selected");
                    $(e.target).addClass("selected");
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

                    $(".left-panel .tree-node-btn").removeClass("selected");
                    $(e.target).addClass("selected");
                }
            });
        });

        $(".project-request-panel .btn-task-request-item").bind("click", (e) => {
            var taskRequestId = $(e.target).data("id");
            self._service.getTaskRequestDetails(taskRequestId)
                .then(response => {
                console.log(response);
                if (response.status === 200) {
                    self.updateTaskRequestDetailsUI(response.data.TaskRequestDetails, response.data.IsJoin);
                    $(".left-panel .tree-node-btn").removeClass("selected");
                    $(e.target).addClass("selected");
                    }
                }).catch(error => {
                    if (error.response) {
                        console.log(error.response.data);
                        console.log(error.response.status);
                        console.log(error.response.headers);
                    } else if (error.request) {
                        console.log(error.request);
                    } else {
                        console.log('Error', error.message);
                    }
                    console.log(error.config);
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

    updateTaskRequestDetailsUI(taskRequestData, isJoin) {
        var self = this;
        self.displayMode(ProjectDisplayMode.TASK_REQUEST);
        taskRequestData.Task.StartDate = self.convertDateTime(taskRequestData.Task.StartDate, "DD/MM/YYYY hh:mm:ss");
        taskRequestData.Task.EndDate = self.convertDateTime(taskRequestData.Task.EndDate, "DD/MM/YYYY hh:mm:ss");
        self.currentTaskRequest(new TaskRequestModel(taskRequestData, isJoin));
    }
}

export class Service {
    constructor(applicationPath) {
        if (Utils.isStringNullOrEmpty(applicationPath)) {
            this._apiBaseUrl = `api/`;
        } else {
            this._apiBaseUrl = `${applicationPath}api/`;
        }
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
