namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ms_Album",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Thumbnail = c.String(),
                        ReleaseDate = c.DateTime(),
                        Status = c.Int(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ms_Artist",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Status = c.Int(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ms_Song",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlbumId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Thumbnail = c.String(),
                        MediaUrl = c.String(),
                        Lyrics = c.String(),
                        Status = c.Int(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ms_Collection",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Thumbnail = c.String(),
                        Status = c.Int(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ms_Genre",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.system_RouteData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                        UrlId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.system_Token",
                c => new
                    {
                        TokenId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AuthToken = c.String(),
                        IssuedOn = c.DateTime(nullable: false),
                        ExpiresOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TokenId);
            
            CreateTable(
                "dbo.system_User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ms_Artist_Album",
                c => new
                    {
                        ArtistId = c.Int(nullable: false),
                        AlbumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ArtistId, t.AlbumId })
                .ForeignKey("dbo.ms_Artist", t => t.ArtistId, cascadeDelete: true)
                .ForeignKey("dbo.ms_Album", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.ArtistId)
                .Index(t => t.AlbumId);
            
            CreateTable(
                "dbo.ms_Song_Album",
                c => new
                    {
                        SongId = c.Int(nullable: false),
                        AlbumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SongId, t.AlbumId })
                .ForeignKey("dbo.ms_Song", t => t.SongId, cascadeDelete: true)
                .ForeignKey("dbo.ms_Album", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.AlbumId);
            
            CreateTable(
                "dbo.ms_Song_Artist",
                c => new
                    {
                        SongId = c.Int(nullable: false),
                        ArtistId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SongId, t.ArtistId })
                .ForeignKey("dbo.ms_Song", t => t.SongId, cascadeDelete: true)
                .ForeignKey("dbo.ms_Artist", t => t.ArtistId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.ArtistId);
            
            CreateTable(
                "dbo.ms_Song_Collection",
                c => new
                    {
                        SongId = c.Int(nullable: false),
                        CollectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SongId, t.CollectionId })
                .ForeignKey("dbo.ms_Song", t => t.SongId, cascadeDelete: true)
                .ForeignKey("dbo.ms_Collection", t => t.CollectionId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.CollectionId);
            
            CreateTable(
                "dbo.ms_Song_Genre",
                c => new
                    {
                        SongId = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SongId, t.GenreId })
                .ForeignKey("dbo.ms_Song", t => t.SongId, cascadeDelete: true)
                .ForeignKey("dbo.ms_Genre", t => t.GenreId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.ms_Album_Genre",
                c => new
                    {
                        AlbumId = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AlbumId, t.GenreId })
                .ForeignKey("dbo.ms_Album", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.ms_Genre", t => t.GenreId, cascadeDelete: true)
                .Index(t => t.AlbumId)
                .Index(t => t.GenreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ms_Album_Genre", "GenreId", "dbo.ms_Genre");
            DropForeignKey("dbo.ms_Album_Genre", "AlbumId", "dbo.ms_Album");
            DropForeignKey("dbo.ms_Song_Genre", "GenreId", "dbo.ms_Genre");
            DropForeignKey("dbo.ms_Song_Genre", "SongId", "dbo.ms_Song");
            DropForeignKey("dbo.ms_Song_Collection", "CollectionId", "dbo.ms_Collection");
            DropForeignKey("dbo.ms_Song_Collection", "SongId", "dbo.ms_Song");
            DropForeignKey("dbo.ms_Song_Artist", "ArtistId", "dbo.ms_Artist");
            DropForeignKey("dbo.ms_Song_Artist", "SongId", "dbo.ms_Song");
            DropForeignKey("dbo.ms_Song_Album", "AlbumId", "dbo.ms_Album");
            DropForeignKey("dbo.ms_Song_Album", "SongId", "dbo.ms_Song");
            DropForeignKey("dbo.ms_Artist_Album", "AlbumId", "dbo.ms_Album");
            DropForeignKey("dbo.ms_Artist_Album", "ArtistId", "dbo.ms_Artist");
            DropIndex("dbo.ms_Album_Genre", new[] { "GenreId" });
            DropIndex("dbo.ms_Album_Genre", new[] { "AlbumId" });
            DropIndex("dbo.ms_Song_Genre", new[] { "GenreId" });
            DropIndex("dbo.ms_Song_Genre", new[] { "SongId" });
            DropIndex("dbo.ms_Song_Collection", new[] { "CollectionId" });
            DropIndex("dbo.ms_Song_Collection", new[] { "SongId" });
            DropIndex("dbo.ms_Song_Artist", new[] { "ArtistId" });
            DropIndex("dbo.ms_Song_Artist", new[] { "SongId" });
            DropIndex("dbo.ms_Song_Album", new[] { "AlbumId" });
            DropIndex("dbo.ms_Song_Album", new[] { "SongId" });
            DropIndex("dbo.ms_Artist_Album", new[] { "AlbumId" });
            DropIndex("dbo.ms_Artist_Album", new[] { "ArtistId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropTable("dbo.ms_Album_Genre");
            DropTable("dbo.ms_Song_Genre");
            DropTable("dbo.ms_Song_Collection");
            DropTable("dbo.ms_Song_Artist");
            DropTable("dbo.ms_Song_Album");
            DropTable("dbo.ms_Artist_Album");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.system_User");
            DropTable("dbo.system_Token");
            DropTable("dbo.system_RouteData");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ms_Genre");
            DropTable("dbo.ms_Collection");
            DropTable("dbo.ms_Song");
            DropTable("dbo.ms_Artist");
            DropTable("dbo.ms_Album");
        }
    }
}
