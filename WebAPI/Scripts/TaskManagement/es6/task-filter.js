import { SingleFilterItem, RangeFilterItem, ListFilterItem, SingleConditionFilterItem } from './filter-items';
import { ListTaskStatus } from '../../Common/es6/enum';

export default class TaskFilter {
    constructor() {
        this.SearchString = ko.observable("");
        this.Status = ko.observable(new ListFilterItem(false, [], ListTaskStatus));
        this.Assignee = ko.observable(new ListFilterItem(false, [], []));
        this.StartDate = ko.observable(new SingleConditionFilterItem());
        this.EndDate = ko.observable(new SingleConditionFilterItem());
        this.Project = ko.observable(new SingleFilterItem(false, 0));
    }
}