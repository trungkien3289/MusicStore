namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_quality_data_type : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ms_Song", "Quality", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ms_Song", "Quality", c => c.String());
        }
    }
}
