import axios from 'axios';
import TaskDetailsModel from './task-details-taskrequest';
import AddEditTaskRequestViewModel from './addedit-taskrequest-viewmodel';
import TaskRequestModel from './task-request-model';
import ProjectModel from './project-model';
import * as moment from 'moment';
import Utils from '../../Common/es6/utils';

$(document).ready(function () {
    var taskRequestManagement = new TaskRequestManagement(applicationPath);
    ko.applyBindings(taskRequestManagement);
});

export default class TaskRequestManagement {
    constructor(applicationPath) {
        this.applicationPath = applicationPath;
        this._service = new Service(applicationPath);
        this._dialog = null;
        this.buildLeftPanel();
        this.initComponents();
        // viewmodel properties
        // project tree
        this.projects = ko.observableArray([]);
        // task details
        this.currentTask = ko.observable(new TaskDetailsModel());
        this.currentTaskRequest = ko.observable(new TaskRequestModel());
        this.isShowContentPanel = ko.observable(false);
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
                if (response.status === 200) {
                    let projectModels = response.data.map(prj => {
                        return new ProjectModel(prj);
                    });
                    self.projects(projectModels);
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

    okBtnDialogClickHandler() {
        if (this.addEditTaskRequestModel().IsEdit) {
            this.updateTaskRequest();
        } else {
            this.createTaskRequest();
        }
    }

    createTaskRequest() {
        var self = this;
        // call api
        var newRequestModel = ko.toJS(this.addEditTaskRequestModel());
        //newRequestModel.Developers = newRequestModel.Developers.map(dev => dev.UserId);
        this._service.createTaskRequest(newRequestModel)
            .then(function (response) {
                if (response.status === 200) {
                    self.showDialog(false);
                    //reload page
                    let taskId = self.getCurrentTaskId();
                    if (taskId) {
                        self.getTaskDetails(taskId);
                    } else {
                        throw new Error("There is no selected task");
                    }
                }
            }).catch(function (error) {
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

    updateTaskRequest() {
        var self = this;
        // call api
        var updatedRequestModel = ko.toJS(this.addEditTaskRequestModel());
        //updatedRequestModel.Developers = updatedRequestModel.Developers.map(dev => dev.UserId);
        this._service.updateTaskRequest(updatedRequestModel)
            .then(function (response) {
                if (response.status === 200) {
                    self.showDialog(false);
                    //reload page
                    let taskId = self.getCurrentTaskId();
                    if (taskId) {
                        self.getTaskDetails(taskId);
                    } else {
                        throw new Error("There is no selected task");
                    }
                }
            }).catch(function (error) {
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

    pickUpDeveloperHandler(taskRequestId, userId) {
        var self = this;
        this._service.assigneDeveloperForTaskRequest(taskRequestId, userId)
            .then(function (response) {
            if (response.status === 200) {
                // notify assign developer successful.
                alert("Assign developer successful.");
                let taskId = self.getCurrentTaskId();
                if (taskId) {
                    self.getTaskDetails(taskId);
                } else {
                    throw new Error("There is no selected task");
                }
            }
        }).catch(function (error) {
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

    unassignUpDeveloperHandler(taskRequestId, userId) {
        var self = this;
        this._service.unassigneDeveloperForTaskRequest(taskRequestId, userId)
            .then(function (response) {
            if (response.status === 200) {
                // notify assign developer successful.
                alert("Unassign developer successful.");
                let taskId = self.getCurrentTaskId();
                if (taskId) {
                    self.getTaskDetails(taskId);
                } else {
                    throw new Error("There is no selected task");
                }
            }
        }).catch(function (error) {
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

    getCurrentTaskId() {
        return this.currentTask() != null ? this.currentTask().Id : null;
    }

    getCurrentProjectId() {
        return this.currentTask() != null ? this.currentTask().ProjectId : null;
    }

    bindEventProjectTaskPanel() {
        var self = this;
        $(".project-task-panel .btn-task-item").bind("click", (e) => {
            var taskId = $(e.target).data("id");
            self.getTaskDetails(taskId);
            $(".project-task-panel .btn-task-item").removeClass("selected");
            $(e.target).addClass("selected");
        });
    }

    getTaskDetails(taskId) {
        var self = this;
        axios.all([self._service.getTaskDetails(taskId), self._service.getTaskRequestOfTask(taskId)])
            .then(axios.spread(function (taskRes, taskRequestRes) {
                
                if (taskRes.status == 200 && taskRequestRes.status == 200) {
                    // Update Task Details panel
                    self.isShowContentPanel(true);
                    let data = taskRes.data;
                    data.StartDate = moment(new Date(data.StartDate)).format("DD/MM/YYYY hh:mm:ss");
                    data.EndDate = moment(new Date(data.EndDate)).format("DD/MM/YYYY hh:mm:ss");
                    self.currentTask(new TaskDetailsModel(data));
                    // Update Task Request Details panel
                    self.hasTaskRequest(taskRequestRes.data != null);
                    self.currentTaskRequest(new TaskRequestModel(taskRequestRes.data));
                }
            }));
    }

    convertDateTime(dateString, format) {
        return moment(new Date(dateString)).format(format);
    }

    getAvailableDevelopers(callback){
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
        if (Utils.isStringNullOrEmpty(applicationPath)) {
            this._apiBaseUrl = `api/`;
        } else {
            this._apiBaseUrl = `${applicationPath}api/`;
        }
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
}
