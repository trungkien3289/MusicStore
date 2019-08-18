namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetaskrequestdevelopertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.fl_TaskRequest", "ProjectId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.fl_TaskRequest", "ProjectId");
        }
    }
}
