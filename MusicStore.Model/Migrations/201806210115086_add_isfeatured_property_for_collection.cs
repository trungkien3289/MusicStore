namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_isfeatured_property_for_collection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ms_Collection", "IsFeatured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ms_Collection", "IsFeatured");
        }
    }
}
