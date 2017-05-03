using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsWannabe.Data.Models
{
    public class PostContent
    {
        public PostContent()
        {

        }

        public PostContent(string title, string content)
        {
            this.Title = title;
            this.Content = content;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }
    }
}
