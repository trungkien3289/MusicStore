namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inittableandrelationshipforfreelandmanagementfeature : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.fl_Project",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        EndDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.fl_TaskRequest",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TaskId = c.Int(nullable: false),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        AssigneeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_User", t => t.AssigneeId)
                .ForeignKey("dbo.fl_Task", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.AssigneeId);
            
            CreateTable(
                "dbo.fl_RequestComment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TaskRequestId = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.fl_TaskRequest", t => t.TaskRequestId, cascadeDelete: true)
                .ForeignKey("dbo.system_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TaskRequestId);
            
            CreateTable(
                "dbo.fl_Task",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        AssigneeId = c.Int(),
                        EndDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EstimatedTime = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_User", t => t.AssigneeId)
                .ForeignKey("dbo.fl_Project", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.AssigneeId);
            
            CreateTable(
                "dbo.system_UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.fl_TaskRequest_DeveloperUser",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.UserId })
                .ForeignKey("dbo.fl_TaskRequest", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.system_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.fl_Project_DeveloperUser",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.UserId })
                .ForeignKey("dbo.fl_Project", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.system_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.fl_Project_LeaderUser",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.UserId })
                .ForeignKey("dbo.fl_Project", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.system_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.UserId);
            
            AddColumn("dbo.system_User", "Email", c => c.String());
            AddColumn("dbo.system_User", "RoleId", c => c.Int(nullable: false));
            AddColumn("dbo.system_User", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.fl_Task", "ProjectId", "dbo.fl_Project");
            DropForeignKey("dbo.fl_Project_LeaderUser", "UserId", "dbo.system_User");
            DropForeignKey("dbo.fl_Project_LeaderUser", "Id", "dbo.fl_Project");
            DropForeignKey("dbo.fl_Project_DeveloperUser", "UserId", "dbo.system_User");
            DropForeignKey("dbo.fl_Project_DeveloperUser", "Id", "dbo.fl_Project");
            DropForeignKey("dbo.fl_TaskRequest", "Id", "dbo.fl_Task");
            DropForeignKey("dbo.fl_Task", "AssigneeId", "dbo.system_User");
            DropForeignKey("dbo.fl_RequestComment", "UserId", "dbo.system_User");
            DropForeignKey("dbo.fl_RequestComment", "TaskRequestId", "dbo.fl_TaskRequest");
            DropForeignKey("dbo.fl_TaskRequest_DeveloperUser", "UserId", "dbo.system_User");
            DropForeignKey("dbo.fl_TaskRequest_DeveloperUser", "Id", "dbo.fl_TaskRequest");
            DropForeignKey("dbo.fl_TaskRequest", "AssigneeId", "dbo.system_User");
            DropIndex("dbo.fl_Project_LeaderUser", new[] { "UserId" });
            DropIndex("dbo.fl_Project_LeaderUser", new[] { "Id" });
            DropIndex("dbo.fl_Project_DeveloperUser", new[] { "UserId" });
            DropIndex("dbo.fl_Project_DeveloperUser", new[] { "Id" });
            DropIndex("dbo.fl_TaskRequest_DeveloperUser", new[] { "UserId" });
            DropIndex("dbo.fl_TaskRequest_DeveloperUser", new[] { "Id" });
            DropIndex("dbo.fl_Task", new[] { "AssigneeId" });
            DropIndex("dbo.fl_Task", new[] { "ProjectId" });
            DropIndex("dbo.fl_RequestComment", new[] { "TaskRequestId" });
            DropIndex("dbo.fl_RequestComment", new[] { "UserId" });
            DropIndex("dbo.fl_TaskRequest", new[] { "AssigneeId" });
            DropIndex("dbo.fl_TaskRequest", new[] { "Id" });
            DropColumn("dbo.system_User", "IsActive");
            DropColumn("dbo.system_User", "RoleId");
            DropColumn("dbo.system_User", "Email");
            DropTable("dbo.fl_Project_LeaderUser");
            DropTable("dbo.fl_Project_DeveloperUser");
            DropTable("dbo.fl_TaskRequest_DeveloperUser");
            DropTable("dbo.system_UserRole");
            DropTable("dbo.fl_Task");
            DropTable("dbo.fl_RequestComment");
            DropTable("dbo.fl_TaskRequest");
            DropTable("dbo.fl_Project");
        }
    }
}
