import axios from 'axios';
import TaskDetailsModel from './task-details-taskrequest';
import AddEditTaskRequestViewModel from './addedit-taskrequest-viewmodel';
import * as moment from 'moment';

$(document).ready(function () {
    var taskRequestManagement = new TaskRequestManagement(applicationPath);
    ko.applyBindings(taskRequestManagement);
});

export default class TaskRequestManagement {
    constructor(applicationPath) {
        this.applicationPath = applicationPath;
        this._service = new Service(applicationPath);
        this._dialog = null;
        this.bindEventProjectTaskPanel();
        this.initComponents();
        // viewmodel properties
        // task details
        this.currentTask = ko.observable(new TaskDetailsModel());
        this.isShowContentPanel = ko.observable(false);
        // task request add/edit viewmodel
        this.addEditTaskRequestModel = ko.observable(new AddEditTaskRequestViewModel());
        this.availableDevelopers = ko.observableArray([]);
        this.isShowAddEditTaskRequestDialog = ko.observable(false);
        this.addTaskRequestBtnClick = this.addTaskRequestHandler.bind(this);
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
    // event handler of view model
    addTaskRequestHandler() {
        var self = this;
        this.getAvailableDevelopers(function () {
            var taskId = self.currentTask() != null ? self.currentTask().Id : null;
            var projectId = self.currentTask() != null ? self.currentTask().ProjectId : null;
            self.addEditTaskRequestModel(new AddEditTaskRequestViewModel(projectId, taskId));
            self.showDialog(true);
        });
    }

    okBtnDialogClickHandler() {
        var self = this;
        // validate model

        // call api
        var newRequestModel = ko.toJS(this.addEditTaskRequestModel());
        newRequestModel.Developers = newRequestModel.Developers.map(dev => dev.Id);
        this._service.createTaskRequest(newRequestModel).then(response => {
            if (response.status === 200) {
                alert("Add task request successful.");
                self.showDialog(false);
            }
        }).catch(response => {
            self.errorMessage(response.response.data);
        });
    }

    bindEventProjectTaskPanel() {
        var self = this;
        $(".project-task-panel .btn-task-item").bind("click", (e) => {
            var taskId = $(e.target).data("id");
            self._service.getTaskDetails(taskId).then(response => {
                console.log(response);
                if (response.status === 200) {
                    self.isShowContentPanel(true);
                    var data = response.data;
                    data.StartDate = moment(new Date(data.StartDate)).format("DD/MM/YYYY hh:mm:ss");
                    data.EndDate = moment(new Date(data.EndDate)).format("DD/MM/YYYY hh:mm:ss");
                    var taskDetails = new TaskDetailsModel(data);
                    self.currentTask(new TaskDetailsModel(taskDetails));
                }
            }).catch(response => {
                //show error
                self.errorMessage(response.response.data);
            });
        });
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
        this._apiBaseUrl = `${applicationPath}/api/`;
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
}
