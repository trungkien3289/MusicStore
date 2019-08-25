import { TaskRequestStatus } from '../../Common/es6/enum';

export default class TaskRequestModel {
    constructor(taskRequestModel) {
        this.Id = taskRequestModel ? taskRequestModel.Id : 0;
        this.Description = taskRequestModel ? taskRequestModel.Description : "";
        this.Status = taskRequestModel ? taskRequestModel.Status : TaskRequestStatus.New;
    }
}