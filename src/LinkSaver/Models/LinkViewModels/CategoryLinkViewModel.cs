using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models.LinkViewModels
{
    public class CategoryLinkViewModel : LinkViewModel
    {
        public CategoryLinkViewModel(ILinkRepository linkRepository, string catSlug)
        {
            category = linkRepository.RetrieveCategoryBySlug(catSlug);
            PopulateLinks(linkRepository.LinksByCategoryToList(catSlug));
        }

        public Category category { get; private set; }
    }
}
