import { TaskStatusEnum } from '../../Common/es6/enum';
export default class AddEditTaskViewModel {
    constructor(task) {
        this.Id = task != null ? task.Id : null;
        this.ProjectId = ko.observable(task != null ? task.ProjectId : null).extend({ required: true });
        this.AssigneeId = ko.observable(task != null ? task.AssigneeId : null);
        this.Name = ko.observable(task != null ? task.Name : "").extend({ required: true }).extend({ minLength: 3 });
        this.Description = ko.observable(task != null ? task.Description : "").extend({ required: true }).extend({ minLength: 3 });
        this.Status = ko.observable(task != null ? task.Status : TaskStatusEnum.New);
        this.StartDate = ko.observable(task != null ? task.StartDate : new Date());
        this.EndDate = ko.observable(task != null ? task.EndDate : new Date());
        this.EstimatedTime = ko.observable(task != null ? task.EstimatedTime : 0);

        this.errors = ko.validation.group(this);
    }
}