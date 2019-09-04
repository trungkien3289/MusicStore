import { TaskRequestStatusEnum } from '../../Common/es6/enum';

export default class AddEditProjectViewModel {
    constructor(projectRequest) {
        this.Id = projectRequest !== undefined ? projectRequest.Id : null;
        this.Name = ko.observable(projectRequest !== undefined ? projectRequest.Name : "").extend({ required: true });
        this.Description = ko.observable(projectRequest !== undefined ? projectRequest.Description : "").extend({ required: true });
        this.StartDate = ko.observable(projectRequest ? projectRequest.StartDate : "");
        this.EndDate = ko.observable(projectRequest ? projectRequest.EndDate : "");
        this.Status = ko.observable(projectRequest ? projectRequest.EndDate : TaskRequestStatusEnum.New);
        this.Leaders = ko.observableArray(projectRequest !== undefined ? projectRequest.Leaders : []);
        this.Developers = ko.observableArray(projectRequest !== undefined ? projectRequest.Developers : []);
    }
}