namespace Library.Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Library.Model.LibraryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Library.Model.LibraryContext";
        }

        protected override void Seed(Library.Model.LibraryContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Magazines.AddOrUpdate(
              m => m.Name,
              new Magazine { Name = "Road" , Number = 1, PublishingDate = DateTime.Now},
              new Magazine { Name = "World", Number = 2, PublishingDate = DateTime.Now },
              new Magazine { Name = "Space", Number = 3, PublishingDate = DateTime.Now }
            );
            //
        }
    }
}
