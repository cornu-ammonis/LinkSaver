using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models.LinkViewModels
{
    public class CategoryLinkViewModel : LinkViewModel
    {
       

        public Category category { get; private set; }

        public void SetCategory(Category _category)
        {
            category = _category;
        }
    }
}
