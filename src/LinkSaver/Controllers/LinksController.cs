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

namespace LinkSaver.Controllers
{
    [Authorize]
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILinkRepository _linkRepository;

        public LinksController(ApplicationDbContext context, ILinkRepository linkRepository)
        {
            _context = context;
            _linkRepository = linkRepository;
        }

        // GET: Links
      
        public async Task<IActionResult> Index()
        {
            if(User.Identity.Name != "admin5@gmail.com")
            {
                return View("Blank");
            }
            return View(await _linkRepository.AllLinksToListAsync());
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
        public async Task<IActionResult> Create([Bind("LinkId,category,url")] Link link)
        {
            if (ModelState.IsValid)
            {
                await _linkRepository.AddLinkToDatabaseAsync(link);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Index", await _linkRepository.AllLinksToListAsync());
                }
                return RedirectToAction("Index");
            }
            return View(link);
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
    }
}
