using CmsWannabe.Data.Models;
using System.Data.Entity;

namespace CmsWannabe.Data
{
    public interface ICmsWannabeDbContext: IBaseDbContext
    {
        IDbSet<PostContent> Contentents { get; }
    }
}
