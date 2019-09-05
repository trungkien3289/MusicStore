import axios from 'axios';
import Utils from '../../Common/es6/utils';
import * as Toastr from 'toastr';
import * as moment from 'moment';
import AddEditProjectViewModel from './addedit-project-viewmodel';

$(document).ready(function () {
    $('.modal').modal();

    this.project = new ProjectManagement(applicationPath);
    ko.applyBindings(this.project, document.body);
});

export default class ProjectManagement {
    constructor(applicationPath) {
        this._service = new Service(applicationPath);
        this._projectEditorDialog = null;

        this.projects = ko.observableArray([]);
        this.addEditProjecttModel = ko.observable(new AddEditProjectViewModel());
        this.availableDevelopers = ko.observableArray([]);

        this.loadProjects();
        this.initComponents();

        this.newProject = () => {
            var self = this;

            this.getAvailableDevelopers(function () {
                self._projectEditorDialog.open();
            });
        };
        this.updatePoject = (Id) => {
            var self = this;

            console.log(Id);

            this.getAvailableDevelopers(function () {
                self._projectEditorDialog.open();
            });
        };
        this.cancelSaveProject = () => {
            this.addEditProjecttModel(new AddEditProjectViewModel());
            this._projectEditorDialog.close();
        };
        this.saveProject = () => {
            var self = this;

            var newRequestModel = ko.toJS(this.addEditProjecttModel());
            console.log(newRequestModel);
            self._service.addProject(newRequestModel).then(response => {
                console.log(response);
                if (response.status === 200) {
                    location.reload();
                    Toastr.info('Are you the 6 fingered man?');
                }
            });

            this.addEditProjecttModel(new AddEditProjectViewModel());
            this._projectEditorDialog.close();
        };

        this.errorMessage = ko.observable("");
    }

    initComponents() {
        $('#projectEditor').modal({
            dismissible: false,
            onOpenStart: function () {
                //Init Dropdown
                $('#projectEditor select').formSelect();
                $('.datepicker').datepicker();
            }
        });
        this._projectEditorDialog = M.Modal.getInstance($("#projectEditor"));
    }

    loadProjects() {
        var self = this;

        console.log(self);
        self._service.getProjectList()
            .then(response => {
                console.log(response.data);
                let projectModels = response.data.map(prj => {
                    return new AddEditProjectViewModel(prj);
                });
                self.projects(projectModels);
            });
    }

    getAvailableDevelopers(callback) {
        var self = this;

        return self._service.getAvailableDevelopers()
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

    getProjectList() {
        return axios.get(
            `${this._apiBaseUrl}projects`,
            {
                withCredentials: true
            }
        );
    }

    addProject(projectModelView) {
        return axios.post(
            `${this._apiBaseUrl}addProject`,
            projectModelView,
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
