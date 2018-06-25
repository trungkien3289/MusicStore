namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_qualify_of_song : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ms_Song", "Quality", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ms_Song", "Quality");
        }
    }
}
