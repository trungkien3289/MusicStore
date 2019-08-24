namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_thumnail_property_for_artist_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ms_Artist", "Thumbnail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ms_Artist", "Thumbnail");
        }
    }
}
