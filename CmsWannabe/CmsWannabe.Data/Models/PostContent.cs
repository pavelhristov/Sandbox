using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsWannabe.Data.Models
{
    public class PostContent
    {
        private ICollection<Page> pages;

        public PostContent()
        {
            this.pages = new HashSet<Page>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public virtual Image Image { get; set; }

        public string TemplateTop { get; set; }

        public string TemplateBottom { get; set; }

        public virtual ICollection<Page> Pages { get => this.pages; set => this.pages = value; }
    }
}
