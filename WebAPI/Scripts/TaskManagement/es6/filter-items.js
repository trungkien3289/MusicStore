export class SingleFilterItem {
    constructor(isEnable, initValue) {
        this.IsEnable = ko.observable(isEnable != null ? isEnable : false);
        this.Value = ko.observable(initValue != null ? initValue: null);
    }
}

export class RangeFilterItem {
    constructor(isEnable) {
        this.IsEnable = ko.observable(isEnable != null ? isEnable : false);
        this.From = ko.observable();
        this.To = ko.observable();
    }
}

export class ListFilterItem {
    constructor(isEnable, values, options) {
        this.IsEnable = ko.observable(isEnable != null ? isEnable : false);
        this.Values = ko.observableArray(values != null ? values : []);
        this.Options = ko.observableArray(options != null ? options : []);
    }
}

export class SingleConditionFilterItem {
    constructor(isEnable, initValue, operator) {
        this.IsEnable = ko.observable(isEnable != null ? isEnable : false);
        this.Value = ko.observable(initValue != null ? initValue : null);
        this.Operator = ko.observable(operator != null ? operator : null);
    }
}