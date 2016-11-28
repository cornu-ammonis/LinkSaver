using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkSaver.Data;
using LinkSaver.Models;
using myExtensions;
using Microsoft.AspNetCore.Authorization;
using LinkSaver.Models.LinkViewModels;
using Microsoft.AspNetCore.Identity;

namespace LinkSaver.Controllers
{
    [Authorize]
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILinkRepository _linkRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public LinksController(ApplicationDbContext context, ILinkRepository linkRepository, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _linkRepository = linkRepository;
            _userManager = userManager;
        }

        // GET: Links
      
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);


            /*if (User.Identity.Name != "admin5@gmail.com")
            {
                return View("Blank");
            }*/
            IList<Link> viewModel = await _linkRepository.AllLinksToListAsync(user);
            if(Request.IsAjaxRequest())
            {
               
                return PartialView(viewModel);
            }
            else
            {
                return View(viewModel);
            }
            
        }

        // GET: Links by category
        public async Task<IActionResult> Category(string slug)
        {
            CategoryLinkViewModel viewModel = new UserCategoryLinkViewModel(_linkRepository, slug,
                await _userManager.FindByNameAsync(User.Identity.Name));
            if(Request.IsAjaxRequest())
            {
                return PartialView(viewModel);
            }
            else
            {
                return View(viewModel);
            }
            
        }

        public async Task<IActionResult> PublicCategory(string slug)
        {
            CategoryLinkViewModel viewModel = new PublicCategoryLinkViewModel(_linkRepository, slug,
              await _userManager.FindByNameAsync(User.Identity.Name));

            if(Request.IsAjaxRequest())
            {
                return PartialView(viewModel);
            }
            else
            {
                return View(viewModel);
            }
        }

        // GET: Links/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links.SingleOrDefaultAsync(m => m.LinkId == id);
            if (link == null)
            {
                return NotFound();
            }

            return View(link);
        }

        // GET: Links/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("category, url, IsPublic")] LinkCreationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                user = await _linkRepository.AddLinkToDatabaseAsync(viewModel, user);
                await _userManager.UpdateAsync(user);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Index", await _linkRepository.AllLinksToListAsync(user));
                }
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Links/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links.SingleOrDefaultAsync(m => m.LinkId == id);
            if (link == null)
            {
                return NotFound();
            }
            return View(link);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LinkId,category,title,url")] Link link)
        {
            if (id != link.LinkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(link);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LinkExists(link.LinkId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(link);
        }

        // GET: Links/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links.SingleOrDefaultAsync(m => m.LinkId == id);
            if (link == null)
            {
                return NotFound();
            }

            return View(link);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _linkRepository.DeleteLinkFromDatabaseAsync(id);
            return RedirectToAction("Index");
        }

        private bool LinkExists(int id)
        {
            return _context.Links.Any(e => e.LinkId == id);
        }


        public async Task<IActionResult> SaveLink(int Id)
        {
           await _linkRepository.SavePostForUserAsync(Id, await _userManager.FindByNameAsync(User.Identity.Name));
            if(Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/Components/PublicLinkButtons/UnsaveButton.cshtml",
                    await _linkRepository.RetrieveLinkByIdAsync(Id));
            }
           return RedirectToAction("Index");
        }

        public async Task<IActionResult> UnsaveLink(int Id)
        {
            await _linkRepository.UnsavePostForUserAsync(Id, 
                await _userManager.FindByNameAsync(User.Identity.Name));

            if(Request.IsAjaxRequest())
            {
                if(Request.Headers["Referer"].ToString().ToLower().Contains("category") )
                {
                    return PartialView("~/Views/Shared/Components/PublicLinkButtons/SaveButton.cshtml", 
                        await _linkRepository.RetrieveLinkByIdAsync(Id));
                }
                else
                {
                    return PartialView("Index", await _linkRepository.AllLinksToListAsync(
                      await  _userManager.FindByNameAsync(User.Identity.Name)));
                }
                
            }

            return RedirectToAction("Index");
        }
    }
}
