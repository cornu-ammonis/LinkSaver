using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models
{
    public class UserLinkSave
    {
        public int LinkId { get; set; }
        public Link link { get; set; }

        public string UserName { get; set; }
        public ApplicationUser user { get; set; }
    }
}
