namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_application_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ms_Application",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppId = c.String(),
                        WDGId = c.String(),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        Type = c.String(),
                        Generic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ms_Application");
        }
    }
}
