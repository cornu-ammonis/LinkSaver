using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models.LinkViewModels
{
    public class LinkViewModel
    {

        protected void PopulateLinks(List<Link> links)
        {
            Links = links;
        }

        public List<Link> Links { get; private set; }

    }
}
