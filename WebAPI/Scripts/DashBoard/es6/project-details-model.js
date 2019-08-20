export default class ProjectDetailsModel {
    constructor(projectModel) {
        this.Id = projectModel != null ? projectModel.Id : null;
        this.Description = projectModel != null ? projectModel.Description : "";
        this.Name = projectModel != null ? projectModel.Name : "";
        this.StartDate = projectModel != null ? projectModel.StartDate : null;
        this.EndDate = projectModel != null ? projectModel.EndDate : null;
        this.Status = projectModel != null ? projectModel.Status : null; 
    }
}