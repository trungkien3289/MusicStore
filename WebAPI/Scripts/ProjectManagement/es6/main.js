import axios from 'axios';
import TaskDetailsModel from '../../TaskRequest/es6/task-details-taskrequest';
import AddEditTaskRequestViewModel from '../../TaskRequest/es6/addedit-taskrequest-viewmodel';
import TaskRequestModel from '../../TaskRequest/es6/task-request-model';
import ProjectModel from '../../TaskRequest/es6/project-model';
import ProjectDetailsModel from '../../DashBoard/es6/project-details-model';
import Utils from '../../Common/es6/utils';
import * as Toastr from 'toastr';
import AddEditProjectViewModel from './addedit-project-viewmodel';
import { format } from 'date-fns';

$(document).ready(function () {
    ko.validation.init({
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        messageTemplate: null
    }, true);
    var taskRequestManagement = new ProjectManagement(applicationPath);
    ko.applyBindings(taskRequestManagement, document.body);
});

const ProjectManagementDisplayMode = {
    EMPTY: 0,
    PROJECT_DETAIL: 1,
    TASK_REQUEST: 2
};

export default class ProjectManagement {
    constructor(applicationPath) {
        this.applicationPath = applicationPath;
        // configure toastr
        Toastr.options.closeButton = true;
        Toastr.options.closeMethod = 'fadeOut';
        Toastr.options.closeDuration = 300;
        this._service = new Service(applicationPath);
        this._projectEditorDialog = null;
        this._dialog = null;
        this.buildLeftPanel();
        this.initComponents();
        // viewmodel properties
        this.displayMode = ko.observable(ProjectManagementDisplayMode.EMPTY);
        this.isShowProjectDetail = ko.computed(function () {
            return this.displayMode() == ProjectManagementDisplayMode.PROJECT_DETAIL;
        }, this);
        this.isShowTaskRequest = ko.computed(function () {
            return this.displayMode() == ProjectManagementDisplayMode.TASK_REQUEST;
        }, this);
        this.isShowEmpty = ko.computed(function () {
            return this.displayMode() == ProjectManagementDisplayMode.EMPTY;
        }, this);
        // project tree
        this.projects = ko.observableArray([]);
        // task details
        this.currentTask = ko.observable(new TaskDetailsModel());
        this.currentProject = ko.observable(new ProjectDetailsModel());
        //this.isShowContentPanel = ko.observable(false);
        this.isTaskAssignee = ko.computed(function () {
            return this.currentTask().Assignee != null;
        }, this);
        //Project add/edit
        this.addEditProjectModel = ko.observable(new AddEditProjectViewModel());
        // task request add/edit viewmodel
        this.addEditTaskRequestModel = ko.observable(new AddEditTaskRequestViewModel());
        this.availableDevelopers = ko.observableArray([]);
        this.isShowAddEditTaskRequestDialog = ko.observable(false);

        //Project Event
        this.onAddProject = () => {
            var self = this;

            this.addEditProjectModel(new AddEditProjectViewModel());
            this.getAvailableDevelopers(function () {
                self._projectEditorDialog.open();
            });
        };
        this.onCancelSaveProject = () => {
            this._projectEditorDialog.close();
        };
        this.onSaveProject = () => {
            var self = this;

            if (self.addEditProjectModel().errors().length > 0) return;
            if (self.addEditProjectModel().IsEdit()) {
                self.updateProject();
            } else {
                self.addProject();
            }
        };
        this.addProject = () => {
            var self = this;
            var newRequestModel = ko.toJS(self.addEditProjectModel());

            self._service.addProject(newRequestModel).then(response => {
                if (response.status === 200) {
                    self._projectEditorDialog.close();
                    self.buildLeftPanel();
                    setTimeout(function () {
                        self.selectProject(response.data.Id);
                    }, 400);
                    Toastr.success('Add the project successfuly.');
                }
            });
        };
        this.updateProject = () => {
            var self = this;
            var newRequestModel = ko.toJS(self.addEditProjectModel());

            self._service.updateProject(newRequestModel).then(response => {
                if (response.status === 200) {
                    self._projectEditorDialog.close();
                    self.buildLeftPanel();
                    setTimeout(function () {
                        self.selectProject(response.data.Id);
                    }, 400);
                    Toastr.success('Update the project successfuly.');
                }
            });
        };
        this.onUpdateProject = () => {
            var self = this;
            var projectId = self.getCurrentProjectIdByProject();

            self._service.getProject(projectId).then(
                response => {
                    var data = response.data;

                    data.StartDate = new Date(data.StartDate);
                    data.EndDate = new Date(data.EndDate);
                    data.Leaders = data.Leaders.map(dev => dev.UserId);
                    data.Developers = data.Developers.map(dev => dev.UserId);
                    console.log(data);

                    this.addEditProjectModel(new AddEditProjectViewModel(data));
                    this.getAvailableDevelopers(function () {
                        self._projectEditorDialog.open();
                    });
                }
            );
        };
        this.onDeleteProject = () => {
            var self = this;
            var projectId = self.getCurrentProjectIdByProject();

            self._service.deleteProject(projectId).then(
                response => {
                    if (response.status === 200) {
                        self.buildLeftPanel();
                        self.displayMode(ProjectManagementDisplayMode.EMPTY);
                        Toastr.success('Delete the project successfuly.');
                    }
                }
            );
        };

        //Task Event
        this.onAddTask = () => {
            var self = this;

            self.addEditProjectModel(new AddEditProjectViewModel());
            self._taskEditorDialog.open();
        };
        this.onCancelSaveTask = () => {
            this._taskEditorDialog.close();
        };
        this.onSaveTask = () => {
            var self = this;

            if (self.addEditProjectModel().errors().length > 0) return;
            if (self.addEditProjectModel().IsEdit()) {
                self.updateTask();
            } else {
                self.addNewTask();
            }
        };
        this.addNewTask = () => {
            var self = this;
            var newRequestModel = ko.toJS(self.addEditProjectModel());

            newRequestModel.ProjectId = this.getCurrentProjectIdByProject();
            self._service.addTask(newRequestModel).then(response => {
                console.log(response);
                if (response.status === 200) {
                    self._taskEditorDialog.close();
                    self.buildLeftPanel();
                    setTimeout(function () {
                        self.selectProject(newRequestModel.ProjectId);
                    }, 400);
                    Toastr.success('Add the task successfuly.');
                }
            });
        };
        this.updateTask = () => {
            var self = this;
            var newRequestModel = ko.toJS(self.addEditProjectModel());

            self._service.updateTask(newRequestModel).then(response => {
                if (response.status === 200) {
                    self._taskEditorDialog.close();

                    let taskId = self.getCurrentTaskId();
                    if (taskId) {
                        self.getTask(taskId);
                    } else {
                        Toastr.error("There is no selected task");
                    }

                    Toastr.success('Update the task successfuly.');
                }
            });
        };
        this.onUpdateTask = () => {
            var self = this;
            var taskId = self.getCurrentTaskId();

            self._service.getTask(taskId).then(
                response => {
                    var data = response.data;

                    data.StartDate = new Date(data.StartDate);
                    data.EndDate = new Date(data.EndDate);

                    this.addEditProjectModel(new AddEditProjectViewModel(data));
                    self._taskEditorDialog.open();
                }
            );
        };
        this.onDeleteTask = () => {
            var self = this;
            var taskId = self.getCurrentTaskId();

            self._service.deleteTask(taskId).then(
                response => {
                    if (response.status === 200) {
                        self.buildLeftPanel();
                        setTimeout(function () {
                            self.selectProject(self.getCurrentProjectId());
                        }, 400);
                        Toastr.success('Delete the task successfuly.');
                    }
                }
            );
        };

        this.errorMessage = ko.observable("");
    }

    initComponents() {
        var self = this;

        $('#projectEditor').modal({
            dismissible: false,
            onOpenStart: function () {
                //Init select
                $('#projectEditor select').formSelect();
                //Init datepicker
                $('#dpStartDate').datepicker({
                    //format: "dd/mm/yyyy",
                    setDefaultDate: true,
                    defaultDate: self.addEditProjectModel().StartDate() ? new Date(self.addEditProjectModel().StartDate()) : new Date()
                });
                $('#dpEndDate').datepicker({
                    //format: "dd/mm/yyyy",
                    setDefaultDate: true,
                    defaultDate: self.addEditProjectModel().EndDate() ? new Date(self.addEditProjectModel().EndDate()) : new Date()
                });
            }
        });
        this._projectEditorDialog = M.Modal.getInstance($("#projectEditor"));

        $('#taskEditor').modal({
            dismissible: false,
            onOpenStart: function () {
                //Init select
                $('#taskEditor select').formSelect();
                //Init datepicker
                $('#dpStartDate').datepicker({
                    //format: "dd/mm/yyyy",
                    setDefaultDate: true,
                    defaultDate: self.addEditProjectModel().StartDate() ? new Date(self.addEditProjectModel().StartDate()) : new Date()
                });
                $('#dpEndDate').datepicker({
                    //format: "dd/mm/yyyy",
                    setDefaultDate: true,
                    defaultDate: self.addEditProjectModel().EndDate() ? new Date(self.addEditProjectModel().EndDate()) : new Date()
                });
            }
        });
        this._taskEditorDialog = M.Modal.getInstance($("#taskEditor"));
    }

    buildLeftPanel() {
        var self = this;
        this._service.getProjects()
            .then(response => {
                let projectModels = response.data.map(prj => {
                    return new ProjectModel(prj);
                });
                self.projects(projectModels);
                self.bindEventProjectTaskPanel();
            });
    }

    getCurrentTaskId() {
        return this.currentTask() != null ? this.currentTask().Id : null;
    }

    getCurrentProjectId() {
        return this.currentTask() != null ? this.currentTask().ProjectId : null;
    }

    getCurrentProjectIdByProject() {
        return this.currentProject() != null ? this.currentProject().Id : null;
    }

    bindEventProjectTaskPanel() {
        var self = this;
        $(".project-task-panel .btn-task-item").bind("click", function (e) {
            var taskId = $(this).data("taskid");
            self.getTask(taskId);
            $(".left-panel .tree-node-btn").removeClass("selected");
            $(this).addClass("selected");
        });

        $(".left-panel .btn-project-item").bind("click", function (e) {
            var projectId = $(this).data("projectid");
            self.selectProject(projectId);
        });
    }

    selectProject(projectId) {
        var self = this;
        self._service.getProject(projectId)
            .then(response => {
                self.displayMode(ProjectManagementDisplayMode.PROJECT_DETAIL);
                var data = response.data;
                data.StartDate = format(new Date(data.StartDate), "dd/MM/yyyy hh:mm:ss");
                data.EndDate = format(new Date(data.EndDate), "dd/MM/yyyy hh:mm:ss");
                var projectDetails = new ProjectDetailsModel(data);
                self.currentProject(new ProjectDetailsModel(projectDetails));

                $(".left-panel .tree-node-btn").removeClass("selected");
                $(`[data-projectid=${projectId}]`).addClass("selected");
            });
    }

    getTask(taskId) {
        var self = this;
        axios.all([self._service.getTask(taskId), self._service.getTaskRequestOfTask(taskId)])
            .then(axios.spread(function (taskRes, taskRequestRes) {
                // Update Task Details panel
                self.displayMode(ProjectManagementDisplayMode.TASK_REQUEST);
                let data = taskRes.data;
                data.StartDate = format(new Date(data.StartDate), "dd/MM/yyyy hh:mm:ss");
                data.EndDate = format(new Date(data.EndDate), "dd/MM/yyyy hh:mm:ss");
                self.currentTask(new TaskDetailsModel(data));
            }));
    }

    getAvailableDevelopers(callback) {
        var self = this;
        return self._service.getAvailableDevelopers()
            .then(response => {
                self.availableDevelopers(response.data);
                callback();
            });
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

    getProjects() {
        return axios.get(
            `${this._apiBaseUrl}projects/withtaskrequest`,
            {
                withCredentials: true
            }
        );
    }
    getProject(id) {
        return axios.get(
            `${this._apiBaseUrl}projects/${id}`,
            {
                withCredentials: true
            }
        );
    }
    addProject(projectModelView) {
        return axios.post(
            `${this._apiBaseUrl}project`,
            projectModelView,
            {
                withCredentials: true
            }
        );
    }
    updateProject(projectModelView) {
        return axios.put(
            `${this._apiBaseUrl}project`,
            projectModelView,
            {
                withCredentials: true
            }
        );
    }
    deleteProject(id) {
        return axios.delete(
            `${this._apiBaseUrl}project/${id}`,
            {
                withCredentials: true
            }
        );
    }

    getTask(taskId) {
        return axios.get(
            `${this._apiBaseUrl}tasks/${taskId}`,
            {
                withCredentials: true
            }
        );
    }
    addTask(taskModelView) {
        return axios.post(
            `${this._apiBaseUrl}task`,
            taskModelView,
            {
                withCredentials: true
            }
        );
    }
    updateTask(taskModelView) {
        return axios.put(
            `${this._apiBaseUrl}task`,
            taskModelView,
            {
                withCredentials: true
            }
        );
    }
    deleteTask(id) {
        return axios.delete(
            `${this._apiBaseUrl}task/${id}`,
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

    getAvailableDevelopers() {
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