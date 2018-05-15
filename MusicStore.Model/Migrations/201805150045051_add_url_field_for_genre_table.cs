namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_url_field_for_genre_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ms_Genre", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ms_Genre", "Url");
        }
    }
}
