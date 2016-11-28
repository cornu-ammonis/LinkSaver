﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models.LinkViewModels
{
    public class PublicCategoryLinkViewModel : CategoryLinkViewModel
    {
        public PublicCategoryLinkViewModel(ILinkRepository linkRepository, string catSlug, ApplicationUser _user)
        {
            user = _user;
            SetCategory(linkRepository.RetrieveCategoryBySlug(catSlug));
            PopulateLinks(linkRepository.PublicLinksByCategoryToList(catSlug, user));

        }

        public ApplicationUser user { get; private set; }
    }
}
