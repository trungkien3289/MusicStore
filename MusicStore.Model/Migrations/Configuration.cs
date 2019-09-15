namespace MusicStore.Model.Migrations
{
    using Helper;
    using MusicStore.Model.DataModels;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MusicStore.Model.DataContext.MainDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MusicStore.Model.DataContext.MainDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.UserRole.AddOrUpdate(x => x.Id,
                new DataModels.system_UserRole() { Id = 1, Name = "Admin" },
                new DataModels.system_UserRole() { Id = 2, Name = "User" }
            );

            var user1 = new DataModels.system_User()
            {
                UserId = 1,
                UserName = "admin",
                Password = "admin",
                Name = "Admin",
                Email = "admin@company.com",
                RoleId = 1,
                IsActive = true,
                Photo = Constants.DEFAULT_USER_PHOTO
            };
            var user2 = new DataModels.system_User()
            {
                UserId = 2,
                UserName = "smith",
                Password = "smith",
                Name = "Smith",
                Email = "smith@company.com",
                RoleId = 2,
                IsActive = true,
                Photo = Constants.DEFAULT_USER_PHOTO
            };
            var user3 = new DataModels.system_User()
            {
                UserId = 3,
                UserName = "david",
                Password = "david",
                Name = "David",
                Email = "david@company.com",
                RoleId = 2,
                IsActive = true,
                Photo = Constants.DEFAULT_USER_PHOTO
            };
            var user4 = new DataModels.system_User()
            {
                UserId = 4,
                UserName = "jimmy",
                Password = "jimmy",
                Name = "Jimmy",
                Email = "jimmy@company.com",
                RoleId = 2,
                IsActive = true,
                Photo = Constants.DEFAULT_USER_PHOTO
            };
            var user5 = new DataModels.system_User()
            {
                UserId = 5,
                UserName = "hand",
                Password = "hand",
                Name = "Hand",
                Email = "hand@company.com",
                RoleId = 2,
                IsActive = true,
                Photo = Constants.DEFAULT_USER_PHOTO
            };

            context.User.AddOrUpdate(x => x.UserId, 
                user1,
                user2,
                user3,
                user4,
                user5
            );

            // Task status = 0: New| 1: Inprogress| 2: Completed |3: Close |4: Feedback
            var task1 = new DataModels.fl_Task()
            {
                Id = 1,
                ProjectId = 1,
                Name = "Analyze requirements",
                Description = "Analyze requirements",
                Status = 0, //Inprogress
                AssigneeId = null,
                EndDate = new DateTime(2019, 8, 30),
                StartDate = new DateTime(2019, 8, 16),
                EstimatedTime = 1// hours
            };
            var task2 = new DataModels.fl_Task()
            {
                Id = 2,
                ProjectId = 1,
                Name = "Design Database",
                Description = "Design Database",
                Status = 0, //Inprogress
                AssigneeId = null,
                EndDate = new DateTime(2019, 8, 30),
                StartDate = new DateTime(2019, 8, 16),
                EstimatedTime = 1// hours
            };
            var task3 = new DataModels.fl_Task()
            {
                Id = 3,
                ProjectId = 1,
                Name = "Define project structure",
                Description = "Define project structure",
                Status = 0, //Inprogress
                AssigneeId = null,
                EndDate = new DateTime(2019, 8, 30),
                StartDate = new DateTime(2019, 8, 16),
                EstimatedTime = 1// hours
            };
            var task4 = new DataModels.fl_Task()
            {
                Id = 4,
                ProjectId = 2,
                Name = "Implement home page",
                Description = "Implement home page",
                Status = 0, //Inprogress
                AssigneeId = null,
                EndDate = new DateTime(2019, 8, 30),
                StartDate = new DateTime(2019, 8, 16),
                EstimatedTime = 2// hours
            };
            var task5 = new DataModels.fl_Task()
            {
                Id = 5,
                ProjectId = 2,
                Name = "Implement product page",
                Description = "Implement product page",
                Status = 0, //Inprogress
                AssigneeId = null,
                EndDate = new DateTime(2019, 8, 30),
                StartDate = new DateTime(2019, 8, 16),
                EstimatedTime = 2// hours
            };
            var task6 = new DataModels.fl_Task()
            {
                Id = 6,
                ProjectId = 2,
                Name = "Implement news page",
                Description = "Implement news page",
                Status = 0, //Inprogress
                AssigneeId = null,
                EndDate = new DateTime(2019, 8, 30),
                StartDate = new DateTime(2019, 8, 16),
                EstimatedTime = 2// hours
            };
            var task7 = new DataModels.fl_Task()
            {
                Id = 7,
                ProjectId = 3,
                Name = "Implement admin page",
                Description = "Implement admin page",
                Status = 0, //Inprogress
                AssigneeId = null,
                EndDate = new DateTime(2019, 8, 30),
                StartDate = new DateTime(2019, 8, 16),
                EstimatedTime = 2// hours
            };
            var task8 = new DataModels.fl_Task()
            {
                Id = 8,
                ProjectId = 3,
                Name = "Implement add product to card",
                Description = "Implement add product to card",
                Status = 1, //Inprogress
                AssigneeId = null,
                EndDate = new DateTime(2019, 8, 30),
                StartDate = new DateTime(2019, 8, 16),
                EstimatedTime = 2// hours
            };
            context.Task.AddOrUpdate(x => x.Id,
                task1,
                task2,
                task3,
                task4,
                task5,
                task6,
                task7,
                task8
            );

            // Project Status = 0: New | 1: Inprogress | 2: Done
            var project1 = new DataModels.fl_Project()
            {
                Id = 1,
                Name = "Freeland management",
                Description = "Freeland management",
                EndDate = new DateTime(2019, 8, 30),
                StartDate = new DateTime(2019, 8, 16),
                Status = 1, //Inprogress
                Leaders = new List<system_User>(),
                Developers = new List<system_User>(),
            };
            project1.Leaders.Add(user1);
            project1.Leaders.Add(user2);
            project1.Developers.Add(user3);
            project1.Developers.Add(user4);
            project1.Developers.Add(user5);
            var project2 = new DataModels.fl_Project()
            {
                Id = 2,
                Name = "Music Online",
                Description = "Music Online",
                EndDate = new DateTime(2019, 12, 12),
                StartDate = new DateTime(2019, 1, 1),
                Status = 1, //Inprogress
                Leaders = new List<system_User>(),
                Developers = new List<system_User>(),
            };
            project2.Leaders.Add(user2);
            project2.Developers.Add(user4);
            project2.Developers.Add(user5);
            var project3 = new DataModels.fl_Project()
            {
                Id = 3,
                Name = "Xavia",
                Description = "Xavia",
                EndDate = new DateTime(2019, 10, 1),
                StartDate = new DateTime(2019, 2, 4),
                Status = 1, //Inprogress
                Leaders = new List<system_User>(),
                Developers = new List<system_User>(),
            };
            project2.Leaders.Add(user2);
            project2.Developers.Add(user3);
            project2.Developers.Add(user4);
            context.Project.AddOrUpdate(x => x.Id,
                project1,
                project2,
                project3
            );

            // Status: 0:New | 1: Active| 2: Close
            var taskRequest1 = new DataModels.fl_TaskRequest()
            {
                Id = 1,
                Project = project1,
                Description = "Analyze requirements urgent",
                Status = 1,
                AssigneeId = null,
                Task = task1
            };
             var taskRequest2 = new DataModels.fl_TaskRequest()
             {
                 Id = 2,
                 Project = project1,
                 Description = "Design Database urgent",
                 Status = 1,
                 AssigneeId = null,
                 Task = task2
             };
            var taskRequest3 = new DataModels.fl_TaskRequest()
            {
                Id = 3,
                Project = project3,
                Description = "Implement news page urgent",
                Status = 1,
                AssigneeId = null,
                Task = task8
            };
            var taskRequest4 = new DataModels.fl_TaskRequest()
            {
                Id = 4,
                Project = project2,
                Description = "Implement add product to card",
                Status = 1,
                AssigneeId = null,
                Task = task6
            };
            context.TaskRequest.AddOrUpdate(x => x.Id,
                taskRequest1,
                taskRequest2,
                taskRequest3,
                taskRequest4
            );

            context.TaskRequestDeveloper.AddOrUpdate(x => x.Id,
                new DataModels.fl_TaskRequestDeveloper()
                {
                    Id = 1,
                    TaskRequestId = 1,
                    UserId = 1,
                    IsJoin = false,
                },
                new DataModels.fl_TaskRequestDeveloper()
                {
                    Id = 2,
                    TaskRequestId = 1,
                    UserId = 2,
                    IsJoin = true,
                },
                new DataModels.fl_TaskRequestDeveloper()
                {
                    Id = 3,
                    TaskRequestId = 2,
                    UserId = 1,
                    IsJoin = false,
                },
                new DataModels.fl_TaskRequestDeveloper()
                {
                    Id = 4,
                    TaskRequestId = 3,
                    UserId = 2,
                    IsJoin = true,
                },
                new DataModels.fl_TaskRequestDeveloper()
                {
                    Id = 5,
                    TaskRequestId = 4,
                    UserId = 1,
                    IsJoin = true,
                },
                new DataModels.fl_TaskRequestDeveloper()
                {
                    Id = 6,
                    TaskRequestId = 4,
                    UserId = 2,
                    IsJoin = true,
                }
            );
        }
    }
}
