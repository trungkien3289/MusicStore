export default class TaskRequestModel {
    constructor(taskRequestModel) {
        this.Id = taskRequestModel ? taskRequestModel.Id : 0;
        this.Description = taskRequestModel ? taskRequestModel.Description : 0;
        this.Status = taskRequestModel ? taskRequestModel.Status : 0;
        this.EstimatedTime = taskRequestModel ? taskRequestModel.EstimatedTime : 0;
        this.Assignee = taskRequestModel ? taskRequestModel.Assignee : null;
        this.Task = taskRequestModel ? taskRequestModel.Task : null;
        this.Project = taskRequestModel ? taskRequestModel.Project : null;
    }
}