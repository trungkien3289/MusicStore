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
        }
    }
}
