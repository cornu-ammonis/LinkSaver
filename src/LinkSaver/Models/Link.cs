using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models
{
    public class Link
    {
        public int LinkId { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public Category category { get; set; }
        public ApplicationUser Author { get; set; }

        public string prependUrl()
        {
            string prependedUrl = url;
            if (!prependedUrl.ToLower().Contains(@"http://") && !prependedUrl.ToLower().Contains(@"https://"))
            {
                prependedUrl = "http://" + prependedUrl;
            }
            return prependedUrl;

        }
    }
}
