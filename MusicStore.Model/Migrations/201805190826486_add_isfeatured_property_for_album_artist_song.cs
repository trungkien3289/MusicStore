namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_isfeatured_property_for_album_artist_song : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ms_Album", "IsFeatured", c => c.Boolean(nullable: false));
            AddColumn("dbo.ms_Artist", "IsFeatured", c => c.Boolean(nullable: false));
            AddColumn("dbo.ms_Song", "IsFeatured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ms_Song", "IsFeatured");
            DropColumn("dbo.ms_Artist", "IsFeatured");
            DropColumn("dbo.ms_Album", "IsFeatured");
        }
    }
}
