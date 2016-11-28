using LinkSaver.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.ViewComponents
{
    public class PublicLinkButtonsViewComponent : ViewComponent
    {
        ILinkRepository repository;
        public PublicLinkButtonsViewComponent(ILinkRepository _repository)
        {
            repository = _repository;
        }

        public async Task<IViewComponentResult> InvokeAsync(Link link)
        {
            if(link.Author.UserName == User.Identity.Name)
            {
                return View("AuthorButtons", link);
            }
            else
            {
                if(await repository.CheckIfSavedAsync(link.LinkId, User.Identity.Name))
                {
                    return View("UnsaveButton", link);
                }
                else
                {
                    return View("SaveButton", link);
                }
               
            }

        }
    }
}
