using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "Make public to other users?")]
        public bool IsPublic { get; set; } = false;

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
