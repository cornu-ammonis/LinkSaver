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
        public string category { get; set; }
    }
}
