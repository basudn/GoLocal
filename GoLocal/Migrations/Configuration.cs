namespace GoLocal.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;

    internal sealed class Configuration : DbMigrationsConfiguration<GoLocal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GoLocal.Models.ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(x => x.Name,
            new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin" });
            ApplicationUserManager mgr = new ApplicationUserManager(
            new Microsoft.AspNet.Identity.EntityFramework.UserStore<Models.ApplicationUser>(context));
            Models.ApplicationUser existingUser = context.Users.FirstOrDefault(x => x.UserName == "admin@abc.com");
            if (existingUser != null) Microsoft.AspNet.Identity.UserManagerExtensions.Delete(mgr, existingUser);
            Models.ApplicationUser au = new Models.ApplicationUser { Email = "admin@abc.com", UserName = "admin@abc.com" };
            var idResult = mgr.CreateAsync(au, "Welcome@1").Result;
            Microsoft.AspNet.Identity.UserManagerExtensions.AddToRoles(mgr, au.Id, "Admin");
        }
    }
}
