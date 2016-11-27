using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models
{
    public class LinkCreationViewModel
    {
       public string url { get; set; }
       public string category { get; set; }
        [Display(Name = "Make public to other users?")]
        public bool IsPublic { get; set; }
        
    
    }
}
