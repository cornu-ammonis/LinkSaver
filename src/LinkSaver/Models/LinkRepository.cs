using HtmlAgilityPack;
using LinkSaver.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        
        async public Task<List<Link>> AllLinksToListAsync()
        {
            return await _context.Links.Include<Link, Category>(l => l.category).ToListAsync();
        }

        async public Task DeleteLinkFromDatabaseAsync(int id)
        {
            var link = await _context.Links.SingleOrDefaultAsync(m => m.LinkId == id);
            _context.Links.Remove(link);
            await _context.SaveChangesAsync();
        } 

        async public Task AddLinkToDatabaseAsync(Link link)
        {
          
            link.title = await RetrieveTitleFromPageAsync(link.url);
            _context.Add(link);
            await _context.SaveChangesAsync();
        }

        async public Task AddLinkToDatabaseAsync(LinkCreationViewModel linkCreationModel)
        {
            Link link = new Link();
            link.url = linkCreationModel.url;
            link.category = await CreateOrRetrieveCategoryByName(linkCreationModel.category);
            link.category.Links.Add(link);
            link.title = await RetrieveTitleFromPageAsync(link.url);
            AddOrUpdateCategory(link.category);
           _context.Links.Add(link);
           
           await _context.SaveChangesAsync();
        }

        public void AddOrUpdateCategory(Category category)
        {
            if (_context.Categories.Contains(category))
            {
                _context.Categories.Update(category);
            }
            else
            {
                _context.Categories.Add(category);
            }
        }
       

        async public Task<Category> CreateOrRetrieveCategoryByName(string name)
        {
            Category category;
            if (await _context.Categories.AnyAsync(c => c.Name == name))
            {
                category = await _context.Categories.Include<Category, List<Link>>(c => c.Links).SingleAsync(c => c.Name == name);
                _context.Update(category);
            }
            else
            {
                category = new Category();
                category.Links = new List<Link>();
                category.Name = name;
                category.UrlSlug = "cat" + _context.Categories.Count().ToString();
               // _context.Add(category);
               // _context.Update(category);
            }
            return category;
            
        }

        async public Task<string> RetrieveTitleFromPageAsync(string url)
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(prependUrl(url));
            string body = await client.GetStringAsync(uri);

            var html = new HtmlDocument();
            html.LoadHtml(body);

            html.OptionFixNestedTags = true;
            var s = html.DocumentNode.Descendants("title").SingleOrDefault();
            string title = s.InnerText;
            return title;
        }

        public string prependUrl(string url)
        {
            url = url.ToLower();
            if (!url.Contains(@"http://") && !url.Contains(@"https://"))
            {
                url = "http://" + url;
            }
            return url;

        }
    }
}
