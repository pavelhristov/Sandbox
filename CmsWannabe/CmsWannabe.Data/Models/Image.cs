using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsWannabe.Data.Models
{
    public class Image
    {
        private ICollection<PostContent> postContents;

        public Image()
        {
            this.postContents = new HashSet<PostContent>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Url { get; set; }

        public virtual ICollection<PostContent> PostContents { get => this.postContents; set => this.postContents = value; }
    }
}
