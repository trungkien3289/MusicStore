import axios from 'axios';
import TaskDetailsModel from './task-details-taskrequest';
import * as moment from 'moment';

$(document).ready(function () {
    var taskRequestManagement = new TaskRequestManagement();
    ko.applyBindings(taskRequestManagement);
});

export default class TaskRequestManagement {
    constructor() {
        this._service = new Service();
        this.bindEventProjectTaskPanel();
        this.currentTask = ko.observable(new TaskDetailsModel());
        this.isShowContentPanel = ko.observable(false);
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
                self.isShowContentPanel(false);
            });
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
