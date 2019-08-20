namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_duration_property_for_song_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ms_Song", "Duration", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ms_Song", "Duration");
        }
    }
}
