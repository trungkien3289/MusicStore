import axios from 'axios';
import Utils from '../../Common/es6/utils';
import * as Toastr from 'toastr';
import * as moment from 'moment';
import AddEditProjectViewModel from './addedit-project-viewmodel';

$(document).ready(function () {
    this.project = new ProjectManagement(applicationPath);
    ko.applyBindings(this.project, document.body);
});

export default class ProjectManagement {
    constructor(applicationPath) {
        this._service = new Service(applicationPath);
        this._projectEditorDialog = null;
        this._ptStartDate = null;
        this._ptEndDate = null;

        this.projects = ko.observableArray([]);
        this.addEditProjectModel = ko.observable(new AddEditProjectViewModel());
        this.availableDevelopers = ko.observableArray([]);

        this.loadProjects();
        this.initComponents();

        this.onAddProject = () => {
            var self = this;

            this.addEditProjectModel(new AddEditProjectViewModel());
            this.getAvailableDevelopers(function () {
                self._projectEditorDialog.open();
            });
        };
        this.onDeleteProject = (id) => {
            self = this;

            this._service.deleteProject(id).then(
                response => {
                    if (response.status === 200) {
                        self.loadProjects();
                        Toastr.success('Delete the project successfuly.');
                    }
                }
            );
        };
        this.onUpdateProject = (id) => {
            var self = this;

            this._service.getProjectById(id).then(
                response => {
                    var data = response.data;

                    data.StartDate = new Date(data.StartDate);
                    data.EndDate = new Date(data.EndDate);
                    data.Leaders = data.Leaders.map(dev => dev.UserId);
                    data.Developers = data.Developers.map(dev => dev.UserId);
                    console.log(data);

                    this.addEditProjectModel(new AddEditProjectViewModel(data));
                    this.getAvailableDevelopers(function () {
                        self._projectEditorDialog.open();
                    });
                }
            );
        };
        this.onCancelSaveProject = () => {
            this._projectEditorDialog.close();
        };
        this.addProject = () => {
            var self = this;
            var newRequestModel = ko.toJS(self.addEditProjectModel());

            console.log(newRequestModel);
            self._service.addProject(newRequestModel).then(response => {
                if (response.status === 200) {
                    self._projectEditorDialog.close();
                    self.loadProjects();
                    Toastr.success('Add the project successfuly.');
                }
            });
        };
        this.updateProject = () => {
            var self = this;
            var newRequestModel = ko.toJS(self.addEditProjectModel());

            console.log(newRequestModel);
            self._service.updateProject(newRequestModel).then(response => {
                if (response.status === 200) {
                    self._projectEditorDialog.close();
                    self.loadProjects();
                    Toastr.success('Update the project successfuly.');
                }
            });
        };
        this.onSaveProject = () => {
            var self = this;

            if (self.addEditProjectModel().errors().length > 0) return;
            if (self.addEditProjectModel().IsEdit()) {
                self.updateProject();
            } else {
                self.addProject();
            }
        };

        this.errorMessage = ko.observable("");
    }

    initComponents() {
        var self = this;

        $('#projectEditor').modal({
            dismissible: false,
            onOpenStart: function () {
                //Init select
                $('#projectEditor select').formSelect();
                //Init datepicker
                $('#dpStartDate').datepicker({
                    //format: "dd/mm/yyyy",
                    setDefaultDate: true,
                    defaultDate: self.addEditProjectModel().StartDate() ? new Date(self.addEditProjectModel().StartDate()) : new Date()
                });
                $('#dpEndDate').datepicker({
                    //format: "dd/mm/yyyy",
                    setDefaultDate: true,
                    defaultDate: self.addEditProjectModel().EndDate() ? new Date(self.addEditProjectModel().EndDate()) : new Date()
                });
            }
        });

        this._projectEditorDialog = M.Modal.getInstance($("#projectEditor"));
    }

    loadProjects() {
        var self = this;

        this._service.getProjects()
            .then(response => {
                let projectModels = response.data.map(prj => {
                    return new AddEditProjectViewModel(prj);
                });
                self.projects(projectModels);
            });
    }

    getAvailableDevelopers(callback) {
        var self = this;

        return this._service.getAvailableDevelopers()
            .then(response => {
                if (response.status === 200) {
                    self.availableDevelopers(response.data);
                    callback();
                }
            }).catch(response => {
                self.errorMessage(response.response.data);
            });
    }
}

export class Service {
    constructor(applicationPath) {
        if (Utils.isStringNullOrEmpty(applicationPath)) {
            this._apiBaseUrl = `api/`;
        } else {
            this._apiBaseUrl = `${applicationPath}api/`;
        }
    }

    getProjects() {
        return axios.get(
            `${this._apiBaseUrl}projects`,
            {
                withCredentials: true
            }
        );
    }

    getProjectById(id) {
        return axios.get(
            `${this._apiBaseUrl}project/${id}`,
            {
                withCredentials: true
            }
        );
    }

    addProject(projectModelView) {
        return axios.post(
            `${this._apiBaseUrl}project`,
            projectModelView,
            {
                withCredentials: true
            }
        );
    }

    updateProject(projectModelView) {
        return axios.put(
            `${this._apiBaseUrl}project`,
            projectModelView,
            {
                withCredentials: true
            }
        );
    }

    deleteProject(id) {
        return axios.delete(
            `${this._apiBaseUrl}project/${id}`,
            {
                withCredentials: true
            }
        );
    }

    getAvailableDevelopers() {
        return axios.get(
            `${this._apiBaseUrl}developers/available`,
            {
                withCredentials: true
            }
        );
    }
}
