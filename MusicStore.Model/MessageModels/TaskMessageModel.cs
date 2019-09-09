using MusicStore.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.MessageModels
{
    public class GetTasksWithFiltersRequest
    {
        public GetTasksFilterModel Filter { get; set; }
        public int CurrentPage { get; set; }
        public int NumberItemsPerPage { get; set; }
    }

    public class GetTasksWithFiltersResponse
    {
        public int CurrentPage { get; set; }
        public int NumberItemsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public ICollection<fl_Task> Tasks { get; set; }
    }

    public class GetTasksFilterModel
    {
        public string SearchString { get; set; }
        public ListFilterItem Status { get; set; }
        public ListFilterItem Assignee { get; set; }
        public DateTimeSingleFilterItem StartDate { get; set; }
        public DateTimeSingleFilterItem EndDate { get; set; }
        public SingleFilterItem Project { get; set; }
    }

    public class ListFilterItem
    {
        public bool IsEnable { get; set; }
        public int[] Values { get; set; }
    }

    public class TaskStatusFilterItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class DateTimeSingleFilterItem
    {
        public bool IsEnable { get; set; }
        public string Value { get; set; }
        public int Operator { get; set; }
    }

    public class SingleFilterItem
    {
        public bool IsEnable { get; set; }
        public int Value { get; set; }
    }

}
