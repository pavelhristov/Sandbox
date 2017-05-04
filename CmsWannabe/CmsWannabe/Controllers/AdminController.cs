using CmsWannabe.Data;
using CmsWannabe.Data.Models;
using CmsWannabe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsWannabe.Controllers
{
    public class AdminController : Controller
    {
        private ICmsWannabeDbContext context;

        public AdminController()
        {
            this.context = new ApplicationDbContext();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreatePage()
        {
            return this.View(new PageViewModel()
            {
                PostContent = new PostContentViewModel()
                {
                    Image = new ImageViewModel()
                }
            });
        }

        [HttpPost]
        public ActionResult CreatePage(PageViewModel page)
        {
            Image img = new Image() { Url = page.PostContent.Image.Url };

            PostContent postContent = new PostContent()
            {
                Image = img,
                TemplateBottom = page.PostContent.TemplateBottom,
                TemplateTop = page.PostContent.TemplateTop
            };

            Page p = new Page()
            {
                Name = page.Name,
                Url = page.Url,
                PostContent = postContent
            };

            this.context.Pages.Add(p);
            this.context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenderTemplate(string param1, string param2, string param3)
        {

            string url = String.Empty +
                (string.IsNullOrWhiteSpace(param1) ? "" : "/" + param1) +
                (string.IsNullOrWhiteSpace(param2) ? "" : "/" + param2) +
                (string.IsNullOrWhiteSpace(param3) ? "" : "/" + param3);

            var pages = this.context.Pages.Where(p => p.Url == url).ToList();
            if (pages.Count < 1)
            {
                return this.View("Error");
            }

            var page = pages.First();

            var postContent = new PostContentViewModel();
            postContent.Image = new ImageViewModel()
            {
                Url = page.PostContent.Image.Url
            };

            postContent.TemplateTop = page.PostContent.TemplateTop;
            postContent.TemplateBottom = page.PostContent.TemplateBottom;
            
            return this.View("ImageAndTable", postContent);
        }
    }
}