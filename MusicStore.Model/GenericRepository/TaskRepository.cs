using Helper;
using MusicStore.Model.DataModels;
using MusicStore.Model.MessageModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.GenericRepository
{
    public class TaskRepository : GenericRepository<fl_Task>
    {
        public TaskRepository(DbContext context): base(context)
        {

        }

        public IQueryable<fl_Task> Get(int? projectId, int? userId, int? status, int page = 1, int numberItemsPerPage = 10)
        {
            IQueryable<fl_Task> query = DbSet;
            if (projectId != null)
            {
                query = query.Where(t => t.ProjectId == projectId);
            }

            if (userId != null)
            {
                query = query.Where(t => t.AssigneeId == userId);
            }

            if (status != null)
            {
                query = query.Where(t => t.Status == status);
            }

            return query.OrderBy(t => t.Name).Skip((page - 1) * numberItemsPerPage).Take(numberItemsPerPage);
        }

        public GetTasksWithFiltersResponse GetWithFilters(GetTasksWithFiltersRequest filter, int userId)
        {
            IQueryable<fl_Task> query = DbSet;
            if (filter.Filter.Project.IsEnable)
            {
                query = query.Where(t => t.ProjectId == filter.Filter.Project.Value);
            }

            if (filter.Filter.Status.IsEnable)
            {
                query = query.Where(t => filter.Filter.Status.Values.Any(s => s == t.Status));
            }

            if (filter.Filter.Assignee.IsEnable)
            {
                query = query.Where(t => filter.Filter.Assignee.Values.Any(a => a == t.AssigneeId));
            }

            if (filter.Filter.StartDate.IsEnable && String.IsNullOrEmpty(filter.Filter.StartDate.Value))
            {
                DateTime startDate = DateTime.ParseExact(
                    filter.Filter.StartDate.Value,
                    Constants.DATE_FORMAT,
                    CultureInfo.InvariantCulture);
                switch (filter.Filter.StartDate.Operator)
                {
                    case (int)FilterOperatorEnum.Equals:
                    {
                        query = query.Where(t => t.StartDate == startDate);
                        break;
                    }
                    case (int)FilterOperatorEnum.GreaterThan:
                    {
                        query = query.Where(t => t.StartDate > startDate);
                        break;
                    }
                    case (int)FilterOperatorEnum.GreaterThanOrEquals:
                    {
                        query = query.Where(t => t.StartDate >= startDate);
                        break;
                    }
                    case (int)FilterOperatorEnum.LessThan:
                    {
                        query = query.Where(t => t.StartDate < startDate);
                        break;
                    }
                    case (int)FilterOperatorEnum.LessThanOrEquals:
                    {
                        query = query.Where(t => t.StartDate <= startDate);
                        break;
                    }
                }
            }

            if (filter.Filter.EndDate.IsEnable && String.IsNullOrEmpty(filter.Filter.EndDate.Value))
            {
                DateTime endDate = DateTime.ParseExact(
                    filter.Filter.EndDate.Value,
                    Constants.DATE_FORMAT,
                    CultureInfo.InvariantCulture);
                switch (filter.Filter.EndDate.Operator)
                {
                    case (int)FilterOperatorEnum.Equals:
                        {
                            query = query.Where(t => t.EndDate == endDate);
                            break;
                        }
                    case (int)FilterOperatorEnum.GreaterThan:
                        {
                            query = query.Where(t => t.EndDate > endDate);
                            break;
                        }
                    case (int)FilterOperatorEnum.GreaterThanOrEquals:
                        {
                            query = query.Where(t => t.EndDate >= endDate);
                            break;
                        }
                    case (int)FilterOperatorEnum.LessThan:
                        {
                            query = query.Where(t => t.EndDate < endDate);
                            break;
                        }
                    case (int)FilterOperatorEnum.LessThanOrEquals:
                        {
                            query = query.Where(t => t.EndDate <= endDate);
                            break;
                        }
                }
            }

            if (!String.IsNullOrEmpty(filter.Filter.SearchString.Trim()))
            {
                var searchString = filter.Filter.SearchString.Trim();
                query = query.Where(t => t.Name.Contains(searchString));
            }

            var count = query.Count();
            var totalPages = (int)Math.Ceiling((double)count / filter.NumberItemsPerPage);
            ICollection<fl_Task> tasks;
            GetTasksWithFiltersResponse result;
            if (filter.NumberItemsPerPage == 0)
            {
                tasks = query.OrderBy(t => t.Name).ToList();
                result = new GetTasksWithFiltersResponse()
                {
                    CurrentPage = 1,
                    NumberItemsPerPage = filter.NumberItemsPerPage,
                    TotalPages = 1,
                    TotalItems = count,
                    Tasks = tasks
                };
            }
            else
            {
                tasks = query.OrderBy(t => t.Name)
                    .Skip((filter.CurrentPage - 1) * filter.NumberItemsPerPage)
                    .Take(filter.NumberItemsPerPage)
                    .ToList();
                result = new GetTasksWithFiltersResponse()
                {
                    CurrentPage = filter.CurrentPage,
                    NumberItemsPerPage = filter.NumberItemsPerPage,
                    TotalPages = totalPages,
                    TotalItems = count,
                    Tasks = tasks
                };
            }

            return result;
        }
    }
}