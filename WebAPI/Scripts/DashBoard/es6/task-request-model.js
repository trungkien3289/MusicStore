import { TaskRequestStatusEnum } from '../../Common/es6/enum';
export default class TaskRequestModel {
    constructor(taskRequestModel, isJoin) {
        this.Id = taskRequestModel ? taskRequestModel.Id : 0;
        this.Description = taskRequestModel ? taskRequestModel.Description : 0;
        this.Status = taskRequestModel ? taskRequestModel.Status : 0;
        this.EstimatedTime = taskRequestModel ? taskRequestModel.EstimatedTime : 0;
        this.Assignee = taskRequestModel ? taskRequestModel.Assignee : null;
        this.Task = taskRequestModel ? taskRequestModel.Task : null;
        this.Project = taskRequestModel ? taskRequestModel.Project : null;
        this.CanJoin = taskRequestModel ? (taskRequestModel.Status == TaskRequestStatusEnum.Active && taskRequestModel.Assignee == null && !isJoin) : false;
    }
}