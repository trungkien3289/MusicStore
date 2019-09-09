import axios from 'axios';
import Utils from '../../Common/es6/utils';
import * as Toastr from 'toastr';
import TaskFilter from './task-filter';
import { DateOperatorOptions } from '../../Common/es6/enum';
import { ListTaskStatus } from '../../Common/es6/enum';
import AddEditViewModel from './addedit-viewmodel';
import AddEditTaskViewModel from './addedit-task-viewmodel';
import { format } from 'date-fns';

$(document).ready(function () {
    ko.validation.init({
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        messageTemplate: null
    }, true);
    var taskManagement = new TaskManagement(applicationPath);
    ko.applyBindings(taskManagement, document.body);
});

export default class TaskManagement {
    constructor(applicationPath) {
        var self = this;
        this.applicationPath = applicationPath;
        // init ui components
        this.initToarst();
        this.initComponents();
        this._service = new Service(applicationPath);
        this.isShowSubFilter = ko.observable(false);
        this.subFilterBtnClick = this.toogleSubFilterPanel.bind(this);
        // list task parameters
        this.tasks = ko.observableArray([]);
        this.currentPage = ko.observable(1);
        this.totalPages = ko.observable(1);
        this.numberItemsPerPage = ko.observable(10);
        this.totalItems = ko.observable(10);
        this.filter = ko.observable(new TaskFilter());
        this.availableDevelopers = ko.observableArray([]);
        this.projects = ko.observableArray([]);
        this.taskStatusOptions = ListTaskStatus;
        this.pagingControl = ko.observable({
            listPages: ko.computed(function () {
                var results = [];
                for (var i = 1; i <= self.totalPages(); i++) {
                    results.push(i);
                }

                return results;
            }, this),
            isNextEnable: ko.computed(function () {
                return self.totalPages() > self.currentPage();
            }, this),
            isPreviousEnable: ko.computed(function () {
                return self.currentPage() > 1;
            }, this)
        });
        this.dateOperatorOptions = DateOperatorOptions;
        // event handlers
        this.onBtnSearchClick = this.btnSearchClickHandler.bind(this);
        this.changePageClick = this.changePageClickHandler.bind(this);
        this.nextPageClick = this.nextPageClickHandler.bind(this);
        this.previousPageClick = this.previousPageClickHandler.bind(this);
        this.onNumberPerPageChange = this.onNumberPerPageChangeHandler.bind(this);

        // prepare data
        this.prepareData().then(response => {
            self.initFilters();
        });
        // add/edit datasource
        this.addEditViewModel = ko.observable(new AddEditViewModel(false));
        this.addTaskBtnClick = this.addTaskBtnClickHandler.bind(this);
        this.editTaskBtnClick = this.editTaskBtnClickHandler.bind(this);
        this.deleteTaskBtnClick = this.deleteTaskBtnClickHandler.bind(this);
        this.okBtnDialogClick = this.okBtnDialogClickHandler.bind(this);
    }

    //#region init ui
    initComponents() {
        var self = this;
        $('#addTaskDialog').modal({
            dismissible: false,
            onOpenStart: function () {
                $('#addTaskDialog select').formSelect();
                $('#addTaskDialog .datepicker').datepicker({
                    format: "dd/mm/yyyy",
                    defaultDate: new Date(),
                    parse: function (value, format) {
                        //var momentDate = moment(value, format);
                        //var jsDate = momentDate.toDate();
                        //return jsDate;
                    },
                });

                var startDateControl = M.Datepicker.getInstance($('#addTaskDialog #pk_StartDate'));
                var startDate = new Date(self.addEditViewModel().Model().StartDate());
                startDateControl.setDate(startDate);
                //$('#addTaskDialog #pk_StartDate').val(moment(startDate).format("DD/MM/YYYY"));
                $('#addTaskDialog #pk_StartDate').val(format(new Date(startDate), "dd/MM/yyyy"));

                var endDateControl = M.Datepicker.getInstance($('#addTaskDialog #pk_EndDate'));
                var endDate = new Date(self.addEditViewModel().Model().EndDate());
                endDateControl.setDate(endDate);
                //$('#addTaskDialog #pk_EndDate').val(moment(endDate).format("DD/MM/YYYY"));
                $('#addTaskDialog #pk_EndDate').val(format(new Date(endDate), "dd/MM/yyyy"));

                $("#addTaskDialog .validationMessage").css("display", "none");
            }
        });
        this._dialog = M.Modal.getInstance($("#addTaskDialog"));
    }

    initToarst() {
        Toastr.options.closeButton = true;
        Toastr.options.closeMethod = 'fadeOut';
        Toastr.options.closeDuration = 300;
    }

    toogleSubFilterPanel() {
        this.isShowSubFilter(!this.isShowSubFilter());
    }

    prepareData() {
        var self = this;
        return axios.all([self._service.getAvailableDevelopers(), self._service.getProjects(), self.getTasks()])
            .then(axios.spread(function (developersRes, projectsRes, taskRes) {
                self.availableDevelopers(developersRes.data);
                self.projects(projectsRes.data);
            }));
    }

    initFilters() {
        $('.sub-filter-panel select').formSelect();
        $('.datepicker').datepicker({
            format: "dd/mm/yyyy",
            //parse: function (value, format) {
            //    var momentDate = moment(value, format);
            //    var jsDate = momentDate.toDate();
            //    return jsDate;
            //}
        });
        $("#txt_Search").bind("keyup", function (e) {
            if (event.keyCode === 13) {
                // Cancel the default action, if needed
                event.preventDefault();
                // Trigger the button element with a click
                document.getElementById("Btn_Search").click();
            }
        });
    }

    //#endregion

    //#region event handlers

    btnSearchClickHandler() {
        this.getTasks();
    }

    changePageClickHandler(pageNumber) {
        this.currentPage(pageNumber);
        this.getTasks();
    }

    nextPageClickHandler() {
        this.currentPage(this.currentPage() + 1);
        this.getTasks();
    }

    previousPageClickHandler() {
        this.currentPage(this.currentPage() - 1);
        this.getTasks();
    }

    onNumberPerPageChangeHandler() {
        this.getTasks();
    }

    //#endregion add/edit task dialog
    addTaskBtnClickHandler() {
        this.addEditViewModel().IsEdit(false);
        this.addEditViewModel().Model(new AddEditTaskViewModel());
        this.showDialog(true);
    }

    editTaskBtnClickHandler(taskId) {
        var self = this;
        this._service.getTask(taskId).then(function (response) {
            self.addEditViewModel().IsEdit(true);
            self.addEditViewModel().Model(new AddEditTaskViewModel(response.data));
            self.showDialog(true);
        });
    }

    deleteTaskBtnClickHandler(taskId) {
        var self = this;
        this._service.deleteTask(taskId).then(function (response) {
            self.getTasks();
        });
    }

    okBtnDialogClickHandler() {
        if (this.addEditViewModel().Model().errors().length > 0) {
            this.addEditViewModel().Model().errors.showAllMessages();
        } else {
            if (this.addEditViewModel().IsEdit()) {
                this.updateTask();
            } else {
                this.createTask();
            }
        }
    }

    createTask() {
        var self = this;
        // call api
        var newTaskModel = ko.toJS(this.addEditViewModel().Model());
        this._service.createTask(newTaskModel)
            .then(function (response) {
                self.showDialog(false);
                self.getTasks();
            });
    }

    updateTask() {
        var self = this;
        // call api
        var updatedTaskModel = ko.toJS(this.addEditViewModel().Model());
        this._service.updateTask(updatedTaskModel)
            .then(function (response) {
                self.showDialog(false);
                self.getTasks();
            });
    }

    showDialog(isShow) {
        if (isShow === null) throw new Error("isShow parameter is null.");
        if (isShow) {
            this._dialog.open();
        } else {
            this._dialog.close();
        }
    }

    convertTaskToViewModel(task, dateFormat) {
        if (dateFormat) {
            //task.StartDate = moment(new Date(task.StartDate)).format(dateFormat);
            //task.EndDate = moment(new Date(task.EndDate)).format(dateFormat);
            task.StartDate = format(new Date(task.StartDate), dateFormat);
            task.EndDate = format(new Date(task.EndDate), dateFormat);
        } else {
            //task.StartDate = moment(new Date(task.StartDate));
            //task.EndDate = moment(new Date(task.EndDate));
            task.StartDate = new Date(task.StartDate);
            task.EndDate = new Date(task.EndDate);
        }
        return task;
    }

    convertTasksToViewModel(tasks) {
        var self = this;
        return tasks.map(function (task) {
            return self.convertTaskToViewModel(task, "dd/MM/yyyy");
        });
    }
    //#region 

    getTasks() {
        var self = this;
        var filterModel = ko.toJS(this.filter());
        var requestModel = {
            Filter: filterModel,
            CurrentPage: this.currentPage(),
            NumberItemsPerPage: this.numberItemsPerPage()
        };
        return this._service.getTasks(requestModel).then(function (response) {
            self.currentPage(response.data.CurrentPage);
            self.numberItemsPerPage(response.data.NumberItemsPerPage);
            self.totalPages(response.data.TotalPages);
            self.totalItems(response.data.TotalItems);
            self.tasks(self.convertTasksToViewModel(response.data.Tasks));
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

    getTasks(requestModel) {
        return axios.post(
            `${this._apiBaseUrl}tasks`,
            requestModel,
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

    getProjects() {
        return axios.get(
            `${this._apiBaseUrl}projects/available`,
            {
                withCredentials: true
            }
        );
    }

    createTask(newTask) {
        return axios.post(
            `${this._apiBaseUrl}tasks/create`,
            newTask,
            {
                withCredentials: true
            }
        );
    }

    updateTask(updatedTask) {
        return axios.put(
            `${this._apiBaseUrl}tasks/update`,
            updatedTask,
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

    deleteTask(taskId) {
        return axios.delete(
            `${this._apiBaseUrl}tasks/${taskId}`,
            {
                withCredentials: true
            }
        );
    }
}