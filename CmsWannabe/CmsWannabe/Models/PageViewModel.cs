using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsWannabe.Models
{
    public class PageViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public virtual PostContentViewModel PostContent { get; set; }
    }
}