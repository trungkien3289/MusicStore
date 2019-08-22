namespace MusicStore.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initdatabase : DbMigration
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
                        IsFeatured = c.Boolean(nullable: false),
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
                        Thumbnail = c.String(),
                        IsFeatured = c.Boolean(nullable: false),
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
                        Url = c.String(),
                        Thumbnail = c.String(),
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
                        Duration = c.Double(),
                        Quality = c.Double(),
                        IsFeatured = c.Boolean(nullable: false),
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
                        IsFeatured = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ms_Application",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppId = c.String(),
                        WDGId = c.String(),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        Type = c.String(),
                        Generic = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.fl_Project",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        EndDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.system_User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        RoleId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.system_UserRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.fl_TaskRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Task = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        AssigneeId = c.Int(),
                        TaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_User", t => t.AssigneeId)
                .ForeignKey("dbo.fl_Project", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.fl_Task", t => t.TaskId)
                .Index(t => t.ProjectId)
                .Index(t => t.AssigneeId)
                .Index(t => t.TaskId);
            
            CreateTable(
                "dbo.fl_TaskRequestDeveloper",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskRequestId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsJoin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.fl_TaskRequest", t => t.TaskRequestId, cascadeDelete: true)
                .ForeignKey("dbo.system_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TaskRequestId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.fl_RequestComment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TaskRequestId = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.fl_TaskRequest", t => t.TaskRequestId, cascadeDelete: true)
                .ForeignKey("dbo.system_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TaskRequestId);
            
            CreateTable(
                "dbo.fl_Task",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        AssigneeId = c.Int(),
                        EndDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EstimatedTime = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_User", t => t.AssigneeId)
                .ForeignKey("dbo.fl_Project", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.AssigneeId);
            
            CreateTable(
                "dbo.system_UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
            
            CreateTable(
                "dbo.fl_Project_DeveloperUser",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.UserId })
                .ForeignKey("dbo.fl_Project", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.system_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.fl_Project_LeaderUser",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.UserId })
                .ForeignKey("dbo.fl_Project", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.system_User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.fl_Task", "ProjectId", "dbo.fl_Project");
            DropForeignKey("dbo.fl_Project_LeaderUser", "UserId", "dbo.system_User");
            DropForeignKey("dbo.fl_Project_LeaderUser", "Id", "dbo.fl_Project");
            DropForeignKey("dbo.fl_Project_DeveloperUser", "UserId", "dbo.system_User");
            DropForeignKey("dbo.fl_Project_DeveloperUser", "Id", "dbo.fl_Project");
            DropForeignKey("dbo.system_User", "RoleId", "dbo.system_UserRole");
            DropForeignKey("dbo.fl_TaskRequest", "TaskId", "dbo.fl_Task");
            DropForeignKey("dbo.fl_Task", "AssigneeId", "dbo.system_User");
            DropForeignKey("dbo.fl_RequestComment", "UserId", "dbo.system_User");
            DropForeignKey("dbo.fl_RequestComment", "TaskRequestId", "dbo.fl_TaskRequest");
            DropForeignKey("dbo.fl_TaskRequest", "ProjectId", "dbo.fl_Project");
            DropForeignKey("dbo.fl_TaskRequestDeveloper", "UserId", "dbo.system_User");
            DropForeignKey("dbo.fl_TaskRequestDeveloper", "TaskRequestId", "dbo.fl_TaskRequest");
            DropForeignKey("dbo.fl_TaskRequest", "AssigneeId", "dbo.system_User");
            DropForeignKey("dbo.ms_Album_Genre", "GenreId", "dbo.ms_Genre");
            DropForeignKey("dbo.ms_Album_Genre", "AlbumId", "dbo.ms_Album");
            DropForeignKey("dbo.ms_Artist_Genre", "GenreId", "dbo.ms_Genre");
            DropForeignKey("dbo.ms_Artist_Genre", "ArtistId", "dbo.ms_Artist");
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
            DropIndex("dbo.fl_Project_LeaderUser", new[] { "UserId" });
            DropIndex("dbo.fl_Project_LeaderUser", new[] { "Id" });
            DropIndex("dbo.fl_Project_DeveloperUser", new[] { "UserId" });
            DropIndex("dbo.fl_Project_DeveloperUser", new[] { "Id" });
            DropIndex("dbo.ms_Album_Genre", new[] { "GenreId" });
            DropIndex("dbo.ms_Album_Genre", new[] { "AlbumId" });
            DropIndex("dbo.ms_Artist_Genre", new[] { "GenreId" });
            DropIndex("dbo.ms_Artist_Genre", new[] { "ArtistId" });
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
            DropIndex("dbo.fl_Task", new[] { "AssigneeId" });
            DropIndex("dbo.fl_Task", new[] { "ProjectId" });
            DropIndex("dbo.fl_RequestComment", new[] { "TaskRequestId" });
            DropIndex("dbo.fl_RequestComment", new[] { "UserId" });
            DropIndex("dbo.fl_TaskRequestDeveloper", new[] { "UserId" });
            DropIndex("dbo.fl_TaskRequestDeveloper", new[] { "TaskRequestId" });
            DropIndex("dbo.fl_TaskRequest", new[] { "TaskId" });
            DropIndex("dbo.fl_TaskRequest", new[] { "AssigneeId" });
            DropIndex("dbo.fl_TaskRequest", new[] { "ProjectId" });
            DropIndex("dbo.system_User", new[] { "RoleId" });
            DropTable("dbo.fl_Project_LeaderUser");
            DropTable("dbo.fl_Project_DeveloperUser");
            DropTable("dbo.ms_Album_Genre");
            DropTable("dbo.ms_Artist_Genre");
            DropTable("dbo.ms_Song_Genre");
            DropTable("dbo.ms_Song_Collection");
            DropTable("dbo.ms_Song_Artist");
            DropTable("dbo.ms_Song_Album");
            DropTable("dbo.ms_Artist_Album");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.system_Token");
            DropTable("dbo.system_RouteData");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.system_UserRole");
            DropTable("dbo.fl_Task");
            DropTable("dbo.fl_RequestComment");
            DropTable("dbo.fl_TaskRequestDeveloper");
            DropTable("dbo.fl_TaskRequest");
            DropTable("dbo.system_User");
            DropTable("dbo.fl_Project");
            DropTable("dbo.ms_Application");
            DropTable("dbo.ms_Collection");
            DropTable("dbo.ms_Song");
            DropTable("dbo.ms_Genre");
            DropTable("dbo.ms_Artist");
            DropTable("dbo.ms_Album");
            DropTable("dbo.system_Logging");
        }
    }
}
