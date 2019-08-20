import Person from './person';
import axios from 'axios';
import ProjectDetailsModel from './project-details-model'
$(document).ready(function () {
    // Raw catalog data - would come from the server
    var availableMeals = [
        { mealName: "Standard (sandwich)", price: 0 },
        { mealName: "Premium (lobster)", price: 34.95 },
        { mealName: "Ultimate (whole zebra)", price: 290 }
    ];

    // Class to represent a row in the reservations grid
    var seatReservation = function (name) {
        this.name = name;
        this.availableMeals = availableMeals;
        this.meal = ko.observable(availableMeals[0]);
        this.remove = function () { viewModel.seats.remove(this) }
        this.formattedPrice = ko.dependentObservable(function () {
            var price = this.meal().price;
            return price ? "$" + price.toFixed(2) : "None";
        }, this);
    }

    // Overall viewmodel for this screen, along with initial state
    var viewModel = {
        projectDetail: new ProjectDetailsModel(),
        seats: ko.observableArray([
            new seatReservation("Steve"),
            new seatReservation("Bert")
        ])
        , addSeat: function () {
            this.seats.push(new seatReservation());
        }
    };

    viewModel.totalSurcharge = ko.dependentObservable(function () {
        var total = 0;
        for (var i = 0; i < this.seats().length; i++)
            total += this.seats()[i].meal().price;
        return total;
    }, viewModel);

    ko.applyBindings(viewModel);

    var person = new Person("David", 20);
    person.speak();
    var dashboardManagement = new DashboardManagement(viewModel);
    dashboardManagement.init();
});

export default class DashboardManagement {
    constructor(viewModel) {
        this._viewModel = viewModel;
        this._service = new Service(this._viewModel);
    }

    init() {
        this.bindEventProjectTaskPanel();
    }

    bindEventProjectTaskPanel() {
        var self = this;
        $(".project-task-panel .btn-project-item").bind("click", (e) => {
            var projectId = $(e.target).data("id");
            self._service.getProjectDetails(projectId);
        });
    }
}

export class Service {
    constructor(viewModel) {
        this._apiBaseUrl = `/api/`;
        this._viewModel = viewModel;
    }

    getProjectDetails(projectId) {
        axios.get(`${this._apiBaseUrl}projects/${projectId}`).then(response => {
            console.log(response);
            if (response.status === 200) {
                var projectDetails = new ProjectDetailsModel(response.data);
                var model = {
                    projectDetail: projectDetails
                };

                ko.mapping.fromJS(this._viewModel, mapping);
                //this._viewModel.projectDetail(projectDetails);
            }
        });
    }
}

