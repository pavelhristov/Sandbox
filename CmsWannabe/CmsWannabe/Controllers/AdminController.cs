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
        public ActionResult Post()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Post(string postTitle, string postContent)
        {
            this.context.Contentents.Add(new PostContent(postTitle, postContent));

            this.context.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenderTemplate()
        {
            var postContent = new PostContentViewModel();
            postContent.Image = new ImageViewModel()
            {
                Url = "https://upload.wikimedia.org/wikipedia/en/1/17/Batman-BenAffleck.jpg"
            };

            return this.View("ImageAndTable", postContent);
        }
    }
}