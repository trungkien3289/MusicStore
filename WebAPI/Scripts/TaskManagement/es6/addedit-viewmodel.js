import AddEditTaskViewModel from './addedit-task-viewmodel';
export default class AddEditViewModel {
    constructor(isEdit, taskModel) {
        this.IsEdit = ko.observable(isEdit != null ? isEdit : false); 
        this.Model = ko.observable(new AddEditTaskViewModel(taskModel));
        this.DialogTitle = ko.computed(function () {
            return this.IsEdit() == true ? "Edit Task" : "Add Task";
        }, this);
    }
}