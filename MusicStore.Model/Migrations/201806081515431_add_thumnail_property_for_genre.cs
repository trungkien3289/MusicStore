namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_thumnail_property_for_genre : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ms_Genre", "Thumbnail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ms_Genre", "Thumbnail");
        }
    }
}
