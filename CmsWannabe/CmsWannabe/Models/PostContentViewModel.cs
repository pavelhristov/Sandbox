using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsWannabe.Models
{
    public class PostContentViewModel
    {
        public ImageViewModel Image { get; set; }

        public string TemplateTop { get; set; }

        public string TemplateBottom { get; set; }

        public string Url { get; set; }
    }
}