import { TaskRequestStatusEnum } from '../../Common/es6/enum';

export default class TaskRequestProjectModel {
    constructor(projectModel) {
        this.Id = projectModel ? projectModel.Id : 0;
        this.Name = projectModel ? projectModel.Name : "";
        this.Description = projectModel ? projectModel.Description : "";
        this.StartDate = projectModel ? projectModel.StartDate : "";
        this.EndDate = projectModel ? projectModel.EndDate : "";
        this.Status = ko.observable(projectModel ? projectModel.Status : TaskRequestStatusEnum.New);
        this.TaskRequests = projectModel ? projectModel.TaskRequests : [];
        this.ProjectButtonHRef = ko.computed(function () {
            return `#project${this.Id}`;
        }, this);
        this.ProjectExpandPanelId = ko.computed(function () {
            return `project${this.Id}`;
        }, this);
    }
}