import { TaskRequestStatusEnum } from '../../Common/es6/enum';
export default class AddEditTaskRequestViewModel {
    constructor(projectId, taskId, taskRequest) {
        this.ProjectId = projectId;
        this.TaskId = taskId;
        this.Description = ko.observable(taskRequest != null ? taskRequest.Description : "");
        this.Developers = ko.observableArray(taskRequest != null ? taskRequest.Developers : []);
        this.Status = ko.observable(taskRequest != null ? taskRequest.Status : TaskRequestStatusEnum.New);
        this.Id = taskRequest != null ? taskRequest.Id : null;
        this.IsEdit = ko.computed(function () {
            return this.Id != null;
        }, this);
    }
}