using MusicStore.Model.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;

namespace MusicStore.Model.DataContext
{
    public class ApplicationUser : IdentityUser
    {
    }
    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<MainDbContext>
    {
        protected override void Seed(MainDbContext context)
        {

            context.ms_Album.Add(new ms_Album()
            {
                Title = "Top Euro - American",
                ReleaseDate = DateTime.Now
            });

            base.Seed(context);
        }

    }
    public class MainDbContext : IdentityDbContext<ApplicationUser>
    {
        public MainDbContext()
            : base("MusicStoreMVCEntities")
        {

        }
        static MainDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<MainDbContext>(new ApplicationDbInitializer());
        }

        public static MainDbContext Create()
        {
            return new MainDbContext();
        }
        public DbSet<ms_Album> ms_Album { get; set; }
        public DbSet<ms_Artist> ms_Artist { get; set; }
        public DbSet<ms_Genre> ms_Genre { get; set; }
        public DbSet<ms_Song> ms_Song { get; set; }
        public DbSet<ms_Collection> ms_Collection { get; set; }
        public DbSet<system_RouteData> system_RouteData { get; set; }
        public DbSet<system_User> User { get; set; }
        public DbSet<system_Token> Token { get; set; }
        public DbSet<system_Logging> Logging { get; set; }
        public DbSet<ms_Application> ms_application { get; set; }

        public DbSet<system_UserRole> UserRole { get; set; }
        public DbSet<fl_Project> Project { get; set; }
        public DbSet<fl_RequestComment> RequestComments { get; set; }
        public DbSet<fl_Task> Task { get; set; }
        public DbSet<fl_TaskRequest> TaskRequest { get; set; }
        public DbSet<fl_TaskRequestDeveloper> TaskRequestDeveloper { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ms_Song>()
                .HasMany(c => c.Genres).WithMany(i => i.Songs)
                .Map(t => t.MapLeftKey("SongId")
                    .MapRightKey("GenreId")
                    .ToTable("ms_Song_Genre"));
            modelBuilder.Entity<ms_Song>()
                .HasMany(c => c.Collections).WithMany(i => i.Songs)
                .Map(t => t.MapLeftKey("SongId")
                    .MapRightKey("CollectionId")
                    .ToTable("ms_Song_Collection"));
            modelBuilder.Entity<ms_Song>()
                .HasMany(c => c.Artists).WithMany(i => i.Songs)
                .Map(t => t.MapLeftKey("SongId")
                    .MapRightKey("ArtistId")
                    .ToTable("ms_Song_Artist"));
            modelBuilder.Entity<ms_Song>()
                .HasMany(c => c.Albums).WithMany(i => i.Songs)
                .Map(t => t.MapLeftKey("SongId")
                    .MapRightKey("AlbumId")
                    .ToTable("ms_Song_Album"));
            modelBuilder.Entity<ms_Artist>()
                .HasMany(c => c.Albums).WithMany(i => i.Artists)
                .Map(t => t.MapLeftKey("ArtistId")
                    .MapRightKey("AlbumId")
                    .ToTable("ms_Artist_Album"));

            modelBuilder.Entity<ms_Album>()
                .HasMany(c => c.Genres).WithMany(i => i.Albums)
                .Map(t => t.MapLeftKey("AlbumId")
                    .MapRightKey("GenreId")
                    .ToTable("ms_Album_Genre"));

            modelBuilder.Entity<ms_Artist>()
                .HasMany(c => c.Genres).WithMany(i => i.Artists)
                .Map(t => t.MapLeftKey("ArtistId")
                    .MapRightKey("GenreId")
                    .ToTable("ms_Artist_Genre"));

            modelBuilder.Entity<fl_Project>()
                .HasMany(c => c.Tasks).WithRequired(t => t.Project)
                .HasForeignKey<int>(t => t.ProjectId);

            modelBuilder.Entity<fl_Project>()
                .HasMany(c => c.Leaders).WithMany(t => t.LeaderProjects)
                .Map(t => t.MapLeftKey("Id")
                    .MapRightKey("UserId")
                    .ToTable("fl_Project_LeaderUser"));

            modelBuilder.Entity<fl_Project>()
                .HasMany(c => c.Developers).WithMany(t => t.DeveloperProjects)
                .Map(t => t.MapLeftKey("Id")
                    .MapRightKey("UserId")
                    .ToTable("fl_Project_DeveloperUser"));

            modelBuilder.Entity<fl_Task>()
                .HasOptional(c => c.Assignee).WithMany(t => t.Tasks)
                .HasForeignKey<Nullable<int>>(t => t.AssigneeId);

            //modelBuilder.Entity<fl_TaskRequest>()
            //   .HasMany(c => c.Developers).WithMany(t => t.DeveloperTaskRequests)
            //    .Map(t => t.MapLeftKey("Id")
            //        .MapRightKey("UserId")
            //        .ToTable("fl_TaskRequest_DeveloperUser"));

            modelBuilder.Entity<fl_TaskRequest>()
               .HasOptional(tr => tr.Assignee).WithMany(u => u.AssigneeTaskRequests)
                .HasForeignKey<Nullable<int>>(tr => tr.AssigneeId);

            modelBuilder.Entity<fl_TaskRequest>()
                .HasRequired(t => t.Task).WithOptional(tr => tr.TaskRequest).Map(x => x.MapKey("TaskId"));

            modelBuilder.Entity<fl_RequestComment>()
             .HasRequired(rc => rc.User).WithMany(u => u.RequestComments)
              .HasForeignKey<int>(tr => tr.UserId);

            modelBuilder.Entity<fl_RequestComment>()
            .HasRequired(rc => rc.TaskRequest).WithMany(tr => tr.RequestComments)
             .HasForeignKey<int>(tr => tr.TaskRequestId);

            modelBuilder.Entity<system_User>()
            .HasRequired(u => u.Role).WithMany(r => r.Users)
             .HasForeignKey<int>(u => u.RoleId);

            modelBuilder.Entity<fl_TaskRequestDeveloper>()
           .HasRequired(trd => trd.TaskRequest).WithMany(tr => tr.Developers)
            .HasForeignKey<int>(trd => trd.TaskRequestId);

        }
    }
}
