namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_relationship_between_artist_genre : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ms_Artist_Genre",
                c => new
                    {
                        ArtistId = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ArtistId, t.GenreId })
                .ForeignKey("dbo.ms_Artist", t => t.ArtistId, cascadeDelete: true)
                .ForeignKey("dbo.ms_Genre", t => t.GenreId, cascadeDelete: true)
                .Index(t => t.ArtistId)
                .Index(t => t.GenreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ms_Artist_Genre", "GenreId", "dbo.ms_Genre");
            DropForeignKey("dbo.ms_Artist_Genre", "ArtistId", "dbo.ms_Artist");
            DropIndex("dbo.ms_Artist_Genre", new[] { "GenreId" });
            DropIndex("dbo.ms_Artist_Genre", new[] { "ArtistId" });
            DropTable("dbo.ms_Artist_Genre");
        }
    }
}
