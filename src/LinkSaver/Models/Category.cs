
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkSaver.Models
{
   public class Category
    {

        public  int CategoryId { get; set; }
        public  string Name { get; set; }
        public  string UrlSlug { get; set; }
        public  string Description { get; set; }


        public virtual List<Link> Links
        { get; set; }

        //public List<CategoryBlogUser> CategoryBlogUsers { get; set; }
    }
}
