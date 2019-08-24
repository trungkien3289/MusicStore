export default class ProjectDetailsModel {
    constructor(projectModel) {
        this.Id = projectModel ? projectModel.Id: 0;
        this.Description = projectModel ? projectModel.Description : 0;
        this.Name = projectModel ? projectModel.Name : 0;
        this.StartDate = projectModel ? new Date(projectModel.StartDate) : 0;
        this.EndDate = projectModel ? new Date(projectModel.EndDate) : 0;
        this.Status = projectModel ? projectModel.Status : 0;
        this.Developers = projectModel ? projectModel.Developers : 0;
        this.Leaders = projectModel ? projectModel.Leaders : 0;
    }
}