using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsWannabe.Data
{
    public interface IBaseDbContext
    {
        int SaveChanges();
    }
}
