export default class TaskTaskRequestModel {
    constructor(taskModel) {
        this.Id = taskModel ? taskModel.Id : 0;
        this.Name = taskModel ? taskModel.Name : "";
        this.Description = taskModel ? taskModel.Description : "";
        this.StartDate = taskModel ? taskModel.StartDate : "";
        this.EndDate = taskModel ? taskModel.EndDate : "";
        this.Status = taskModel ? taskModel.Status : "";
        this.EstimatedTime = taskModel ? taskModel.EstimatedTime : "";
        this.Assignee = taskModel ? taskModel.Assignee : null;
        this.TaskRequest = taskModel ? taskModel.TaskRequest : null;
    }
}