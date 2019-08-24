export default class DashBoardModel {
    constructor(dashboardModel) {
        this.TotalProjects = dashboardModel ? dashboardModel.TotalProjects : 0;
        this.TotalTasks = dashboardModel ? dashboardModel.TotalTasks : 0;
        this.TotalTaskRequests = dashboardModel ? dashboardModel.TotalTaskRequests : 0;
    }
}