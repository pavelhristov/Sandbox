namespace CmsWannabe.Migrations
{
    using CmsWannabe.Data.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CmsWannabe.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "CmsWannabe.Models.ApplicationDbContext";
        }

        protected override void Seed(CmsWannabe.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //if (!context.Pages.Any())
            //{
            //    var image = new Image()
            //    {
            //        Url = "https://upload.wikimedia.org/wikipedia/en/1/17/Batman-BenAffleck.jpg"
            //    };
            //    context.Images.Add(image);
            //    context.SaveChanges();


            //    var postContent = new PostContent();
            //    postContent.TemplateTop = "Image";
            //    postContent.TemplateBottom = "Table";
            //    postContent.Image = image;

            //    context.PostContentents.Add(postContent);
            //    context.SaveChanges();



            //    var page = new Page();
            //    page.Name = "ImageAndTable";
            //    page.Url = "/superheroes/batman";
            //    page.PostContent = postContent;

            //    context.Pages.Add(page);
            //    context.SaveChanges();
            //}

            //var image = new Image()
            //{
            //    Url = "https://www.sideshowtoy.com/photo.php?sku=902622"
            //};
            //context.Images.Add(image);
            //context.SaveChanges();


            //var postContent = new PostContent();
            //postContent.TemplateTop = "Image";
            //postContent.TemplateBottom = "UnorderedList";
            //postContent.Image = image;

            //context.PostContentents.Add(postContent);
            //context.SaveChanges();



            //var page = new Page();
            //page.Name = "ImageAndUnorderedList";
            //page.Url = "/superheroes/ironman";
            //page.PostContent = postContent;

            //context.Pages.Add(page);
            //context.SaveChanges();
        }
    }
}
