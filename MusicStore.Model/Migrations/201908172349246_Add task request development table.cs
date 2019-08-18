namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addtaskrequestdevelopmenttable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.fl_TaskRequest_DeveloperUser", "Id", "dbo.fl_TaskRequest");
            DropForeignKey("dbo.fl_TaskRequest_DeveloperUser", "UserId", "dbo.system_User");
            DropIndex("dbo.fl_TaskRequest_DeveloperUser", new[] { "Id" });
            DropIndex("dbo.fl_TaskRequest_DeveloperUser", new[] { "UserId" });
            CreateTable(
                "dbo.fl_TaskRequestDeveloper",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskRequestId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsJoin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.fl_TaskRequest", t => t.TaskRequestId, cascadeDelete: true)
                .ForeignKey("dbo.system_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TaskRequestId)
                .Index(t => t.UserId);
            
            CreateIndex("dbo.system_User", "RoleId");
            AddForeignKey("dbo.system_User", "RoleId", "dbo.system_UserRole", "Id", cascadeDelete: true);
            DropTable("dbo.fl_TaskRequest_DeveloperUser");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.fl_TaskRequest_DeveloperUser",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.UserId });
            
            DropForeignKey("dbo.system_User", "RoleId", "dbo.system_UserRole");
            DropForeignKey("dbo.fl_TaskRequestDeveloper", "UserId", "dbo.system_User");
            DropForeignKey("dbo.fl_TaskRequestDeveloper", "TaskRequestId", "dbo.fl_TaskRequest");
            DropIndex("dbo.fl_TaskRequestDeveloper", new[] { "UserId" });
            DropIndex("dbo.fl_TaskRequestDeveloper", new[] { "TaskRequestId" });
            DropIndex("dbo.system_User", new[] { "RoleId" });
            DropTable("dbo.fl_TaskRequestDeveloper");
            CreateIndex("dbo.fl_TaskRequest_DeveloperUser", "UserId");
            CreateIndex("dbo.fl_TaskRequest_DeveloperUser", "Id");
            AddForeignKey("dbo.fl_TaskRequest_DeveloperUser", "UserId", "dbo.system_User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.fl_TaskRequest_DeveloperUser", "Id", "dbo.fl_TaskRequest", "Id", cascadeDelete: true);
        }
    }
}
