export default class TaskDetailsModel {
    constructor(taskModel) {
        this.Id = taskModel ? taskModel.Id : 0;
        this.Name = taskModel ? taskModel.Name : 0;
        this.Description = taskModel ? taskModel.Description : 0;
        this.StartDate = taskModel ? taskModel.StartDate : 0;
        this.EndDate = taskModel ? taskModel.EndDate : 0;
        this.Status = taskModel ? taskModel.Status : 0;
        this.EstimatedTime = taskModel ? taskModel.EstimatedTime : 0;
        this.Assignee = taskModel ? taskModel.Assignee : null;
    }
}