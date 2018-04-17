namespace MusicStore.Model.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MusicStore.Model.DataContext;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MusicStore.Model.DataContext.MainDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MusicStore.Model.DataContext.MainDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            context.SaveChanges();

            if (!context.Users.Any(u => u.UserName == "Karl"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "Karl" };

                manager.Create(user, "123123");
                manager.AddToRole(user.Id, "Admin");
            }

            context.SaveChanges();
        }
    }
}
