using LinkSaver.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models
{
    public class LinkRepository : ILinkRepository
    {
        private ApplicationDbContext _context;

        public LinkRepository(ApplicationDbContext db)
        {
            _context = db;
        }

        async public Task DeleteLinkFromDatabaseAsync(int id)
        {
            var link = await _context.Links.SingleOrDefaultAsync(m => m.LinkId == id);
            _context.Links.Remove(link);
            await _context.SaveChangesAsync();
        } 

        async public Task AddLinkToDatabaseAsync(Link link)
        {
            link.title = "placeholder";

            _context.Add(link);
            await _context.SaveChangesAsync();
        }
    }
}
