import axios from 'axios';
import TaskDetailsModel from './task-details-taskrequest';
import AddEditTaskRequestViewModel from './addedit-taskrequest-viewmodel';
import TaskRequestModel from './task-request-model';
import ProjectModel from './project-model';
import ProjectDetailsModel from '../../DashBoard/es6/project-details-model';
import * as moment from 'moment';
import Utils from '../../Common/es6/utils';
import * as Toastr from 'toastr';

$(document).ready(function () {
    ko.validation.init({
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        messageTemplate: null
    }, true);
    var taskRequestManagement = new TaskRequestManagement(applicationPath);
    ko.applyBindings(taskRequestManagement, document.body);
});

const TaskRequestManagementDisplayMode = {
    EMPTY: 0,
    PROJECT_DETAIL: 1,
    TASK_REQUEST: 2
};

export default class TaskRequestManagement {
    constructor(applicationPath) {
        this.applicationPath = applicationPath;
        // configure toastr
        Toastr.options.closeButton = true;
        Toastr.options.closeMethod = 'fadeOut';
        Toastr.options.closeDuration = 300;
        this._service = new Service(applicationPath);
        this._dialog = null;
        this.buildLeftPanel();
        this.initComponents();
        // viewmodel properties
        this.displayMode = ko.observable(TaskRequestManagementDisplayMode.EMPTY);
        this.isShowProjectDetail = ko.computed(function () {
            return this.displayMode() == TaskRequestManagementDisplayMode.PROJECT_DETAIL;
        }, this);
        this.isShowTaskRequest = ko.computed(function () {
            return this.displayMode() == TaskRequestManagementDisplayMode.TASK_REQUEST;
        }, this);
        this.isShowEmpty = ko.computed(function () {
            return this.displayMode() == TaskRequestManagementDisplayMode.EMPTY;
        }, this);
        // project tree
        this.projects = ko.observableArray([]);
        // task details
        this.currentTask = ko.observable(new TaskDetailsModel());
        this.currentTaskRequest = ko.observable(new TaskRequestModel());
        this.currentProject = ko.observable(new ProjectDetailsModel());
        //this.isShowContentPanel = ko.observable(false);
        this.hasTaskRequest = ko.observable(false);
        this.isTaskAssignee = ko.computed(function () {
            return this.currentTask().Assignee != null;
        }, this);
        // task request add/edit viewmodel
        this.addEditTaskRequestModel = ko.observable(new AddEditTaskRequestViewModel());
        this.availableDevelopers = ko.observableArray([]);
        this.isShowAddEditTaskRequestDialog = ko.observable(false);
        this.addTaskRequestBtnClick = this.addTaskRequestHandler.bind(this);
        this.editTaskRequestBtnClick = this.editTaskRequestHandler.bind(this);
        this.deleteTaskRequestBtnClick = this.deleteTaskRequestHandler.bind(this);
        this.pickUpDeveloperBtnClick = this.pickUpDeveloperHandler.bind(this);
        this.unassignDeveloperBtnClick = this.unassignUpDeveloperHandler.bind(this);
        this.okBtnDialogClick = this.okBtnDialogClickHandler.bind(this);
        this.errorMessage = ko.observable("");
    }

    initComponents() {
        $('#addTaskRequestDialog').modal({
            dismissible: false,
            onOpenStart: function () {
                $('#addTaskRequestDialog select').formSelect();
            }
        });
        this._dialog = M.Modal.getInstance($("#addTaskRequestDialog"));
    }

    buildLeftPanel() {
        var self = this;
        this._service.getProjectList()
            .then(response => {
                let projectModels = response.data.map(prj => {
                    return new ProjectModel(prj);
                });
                self.projects(projectModels);
                self.bindEventProjectTaskPanel();
            });
    }

    // event handler of view model
    addTaskRequestHandler() {
        var self = this;
        this.getAvailableDevelopers(function () {
            var taskId = self.getCurrentTaskId();
            var projectId = self.getCurrentProjectId();
            self.addEditTaskRequestModel(new AddEditTaskRequestViewModel(projectId, taskId));
            self.showDialog(true);
        });
    }

    editTaskRequestHandler() {
        var self = this;
        this.getAvailableDevelopers(function () {
            var taskId = self.getCurrentTaskId();
            var projectId = self.getCurrentProjectId();
            var taskRequest = ko.toJS(self.currentTaskRequest());
            taskRequest.Developers = taskRequest.Developers.map(dev => dev.UserId);
            self.addEditTaskRequestModel(new AddEditTaskRequestViewModel(projectId, taskId, taskRequest));
            self.showDialog(true);
        });
    }

    deleteTaskRequestHandler() {
        var self = this;
        var taskId = self.getCurrentTaskId();
        this._service.deleteTaskRequest(taskId).then(response => {
            if (response.status == 200) {
                Toastr.success("Delete Task request successfullly.");
                //reload page
                self.getTaskDetails(taskId);
            }
        });
    }

    okBtnDialogClickHandler() {
        if (this.addEditTaskRequestModel().errors().length > 0) return;
        if (this.addEditTaskRequestModel().IsEdit()) {
            this.updateTaskRequest();
        } else {
            this.createTaskRequest();
        }
    }

    createTaskRequest() {
        var self = this;
        // call api
        var newRequestModel = ko.toJS(this.addEditTaskRequestModel());
        this._service.createTaskRequest(newRequestModel)
            .then(function (response) {
                self.showDialog(false);
                //reload page
                let taskId = self.getCurrentTaskId();
                if (taskId) {
                    self.getTaskDetails(taskId);
                } else {
                    Toastr.error("There is no selected task");
                }
            });
    }

    updateTaskRequest() {
        var self = this;
        // call api
        var updatedRequestModel = ko.toJS(this.addEditTaskRequestModel());
        //updatedRequestModel.Developers = updatedRequestModel.Developers.map(dev => dev.UserId);
        this._service.updateTaskRequest(updatedRequestModel)
            .then(function (response) {
                self.showDialog(false);
                //reload page
                let taskId = self.getCurrentTaskId();
                if (taskId) {
                    self.getTaskDetails(taskId);
                } else {
                    Toastr.error("There is no selected task");
                }
            });
    }

    pickUpDeveloperHandler(taskRequestId, userId) {
        var self = this;
        this._service.assigneDeveloperForTaskRequest(taskRequestId, userId)
            .then(function (response) {
                Toastr.success("Assign developer successful.");
                let taskId = self.getCurrentTaskId();
                if (taskId) {
                    self.getTaskDetails(taskId);
                } else {
                    Toastr.error("There is no selected task");
                }
        });
    }

    unassignUpDeveloperHandler(taskRequestId, userId) {
        var self = this;
        this._service.unassigneDeveloperForTaskRequest(taskRequestId, userId)
            .then(function (response) {
                Toastr.success("Unassign developer successful.");
                let taskId = self.getCurrentTaskId();
                if (taskId) {
                    self.getTaskDetails(taskId);
                } else {
                    Toastr.error("There is no selected task");
                }
        });
    }

    getCurrentTaskId() {
        return this.currentTask() != null ? this.currentTask().Id : null;
    }

    getCurrentProjectId() {
        return this.currentTask() != null ? this.currentTask().ProjectId : null;
    }

    bindEventProjectTaskPanel() {
        var self = this;
        $(".project-task-panel .btn-task-item").bind("click", function(e){
            var taskId = $(this).data("id");
            self.getTaskDetails(taskId);
            $(".left-panel .tree-node-btn").removeClass("selected");
            $(this).addClass("selected");
        });

        $(".left-panel .btn-project-item").bind("click", function (e) {
            var projectId = $(this).data("id");
            self._service.getProjectDetails(projectId)
                .then(response => {
                    self.displayMode(TaskRequestManagementDisplayMode.PROJECT_DETAIL);
                    var data = response.data;
                    data.StartDate = moment(new Date(data.StartDate)).format("DD/MM/YYYY hh:mm:ss");
                    data.EndDate = moment(new Date(data.EndDate)).format("DD/MM/YYYY hh:mm:ss");
                    var projectDetails = new ProjectDetailsModel(data);
                    self.currentProject(new ProjectDetailsModel(projectDetails));

                    $(".left-panel .tree-node-btn").removeClass("selected");
                    $(this).addClass("selected");
                });
        });
    }

    getTaskDetails(taskId) {
        var self = this;
        axios.all([self._service.getTaskDetails(taskId), self._service.getTaskRequestOfTask(taskId)])
            .then(axios.spread(function (taskRes, taskRequestRes) {
                // Update Task Details panel
                self.displayMode(TaskRequestManagementDisplayMode.TASK_REQUEST);
                let data = taskRes.data;
                data.StartDate = moment(new Date(data.StartDate)).format("DD/MM/YYYY hh:mm:ss");
                data.EndDate = moment(new Date(data.EndDate)).format("DD/MM/YYYY hh:mm:ss");
                self.currentTask(new TaskDetailsModel(data));
                // Update Task Request Details panel
                self.hasTaskRequest(taskRequestRes.data != null);
                self.currentTaskRequest(new TaskRequestModel(taskRequestRes.data));
            }));
    }

    convertDateTime(dateString, format) {
        return moment(new Date(dateString)).format(format);
    }

    getAvailableDevelopers(callback){
        var self = this;
        return self._service.getAvailableDevelopers()
            .then(response => {
                self.availableDevelopers(response.data);
                callback();
            });
    }

    // show add/edit task request dialog
    showDialog(isShow){
        if (isShow === null) throw new Error("isShow parameter is null.");
        this.isShowAddEditTaskRequestDialog(isShow);
        if (isShow) {
            this._dialog.open();
        } else {
            this._dialog.close();
        }
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

    getProjectList() {
        return axios.get(
            `${this._apiBaseUrl}projects/withtaskrequest`,
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

    getProjectDetails(projectId) {
        return axios.get(
            `${this._apiBaseUrl}projects/${projectId}`,
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

    getAvailableDevelopers(){
        return axios.get(
            `${this._apiBaseUrl}developers/available`,
            {
                withCredentials: true
            }
        );
    }

    createTaskRequest(newTaskRequest) {
        return axios.post(
            `${this._apiBaseUrl}taskrequest/create`,
            newTaskRequest,
            {
                withCredentials: true
            }
        );
    }

    updateTaskRequest(updatedTaskRequest) {
        return axios.put(
            `${this._apiBaseUrl}taskrequest/update`,
            updatedTaskRequest,
            {
                withCredentials: true
            }
        );
    }

    getTaskRequestOfTask(taskId) {
        return axios.get(
            `${this._apiBaseUrl}tasks/${taskId}/taskrequest`,
            {
                withCredentials: true
            }
        );
    }

    assigneDeveloperForTaskRequest(taskRequestId, userId) {
        return axios.post(
            `${this._apiBaseUrl}taskrequest/${taskRequestId}/pickdeveloper/${userId}`,
            {
                withCredentials: true
            }
        );
    }

    unassigneDeveloperForTaskRequest(taskRequestId, userId) {
        return axios.post(
            `${this._apiBaseUrl}taskrequest/${taskRequestId}/unassigndeveloper`,
            {
                withCredentials: true
            }
        );
    }

    deleteTaskRequest(taskId) {
        return axios.delete(
            `${this._apiBaseUrl}task/${taskId}/taskrequest`,
            {
                withCredentials: true
            }
        );
    }
}
