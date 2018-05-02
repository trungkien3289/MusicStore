namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNLogforloginformationintodatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.system_Logging",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CallSite = c.String(),
                        Date = c.String(),
                        Exception = c.String(),
                        Level = c.String(),
                        Logger = c.String(),
                        MachineName = c.String(),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Thread = c.String(),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.system_Logging");
        }
    }
}
