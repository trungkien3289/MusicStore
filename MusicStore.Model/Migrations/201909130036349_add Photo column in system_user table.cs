namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPhotocolumninsystem_usertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.system_User", "Photo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.system_User", "Photo");
        }
    }
}
