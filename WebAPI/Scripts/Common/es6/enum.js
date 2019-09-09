export const TaskRequestStatusEnum = {
    New : 0,
    Active : 1,
    Close : 2
}

export const TaskStatusEnum = {
    New: 0,
    Inprogress: 1,
    Completed: 2,
    Close: 3,
    Feedback: 4
}

export const DateOperatorOptions =
    [
        { Name: ">", Value: 0 },
        { Name: ">=", Value: 1 },
        { Name: "<", Value: 2 },
        { Name: "<=", Value: 3 },
        { Name: "=", Value: 4 },
    ];

export const ListTaskStatus = [
    {
        Name: "New",
        Value: 0
    },
    {
        Name: "Inprogress",
        Value: 1
    },
    {
        Name: "Completed",
        Value: 2
    },
    {
        Name: "Close",
        Value: 3
    },
    {
        Name: "Feedback",
        Value: 4
    }
];