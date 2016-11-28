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

        
        async public Task<List<Link>> AllLinksToListAsync(ApplicationUser user)
        {
            List<Link> l_query = await
                 (from l in _context.Links
                  where l.Author == user
                  orderby l.LinkId descending
                  select l).Include<Link, Category>(l => l.category).ToListAsync();

            return l_query;
        }

        async public Task<List<Link>> LinksByCategoryToListAsync(string categorySlug)
        {
            List<Link> categoryLinks = await
                (from l in _context.Links
                 where l.category.UrlSlug == categorySlug
                 orderby l.LinkId descending //should return most recent links first
                 select l).Include<Link, Category>(l => l.category).ToListAsync();

            return categoryLinks;
        }
        
        public List<Link> LinksByCategoryToList(string categorySlug)
        {
            List<Link> categoryLinks = 
              (from l in _context.Links
               where l.category.UrlSlug == categorySlug
               orderby l.LinkId descending //should return most recent links first
                 select l).Include<Link, Category>(l => l.category).ToList();

            return categoryLinks;
        }
        public List<Link> UserLinksByCategoryToList(string categorySlug, ApplicationUser user)
        {
            List<Link> categoryLinks =
                (from l in _context.Links
                 where l.category.UrlSlug == categorySlug
                 where l.Author == user
                 orderby l.LinkId descending
                 select l).Include<Link, Category>(l => l.category).ToList();

            return categoryLinks;
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

        async public Task<ApplicationUser> AddLinkToDatabaseAsync(LinkCreationViewModel linkCreationModel, ApplicationUser user)
        {
            Link link = new Link();
            link.url = linkCreationModel.url;
            link.Author = user;
            link.IsPublic = linkCreationModel.IsPublic;
           
            //if user doesnt enter a category name
            if (linkCreationModel.category == null)
            {
                linkCreationModel.category = "uncategorized";
            }

            link.category = await CreateOrRetrieveCategoryByName(linkCreationModel.category);
            link.category.Links.Add(link);
            link.title = await RetrieveTitleFromPageAsync(link.prependUrl());
            AddOrUpdateCategory(link.category);
            user.UserLinks.Add(link);
           _context.Links.Add(link);
           
           await _context.SaveChangesAsync();
           return user;
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

        async public Task<Category> RetrieveCategoryBySlugAsync(string slug)
        {
            if(await _context.Categories.AnyAsync(c => c.UrlSlug == slug))
            {
                return await _context.Categories.SingleAsync(c => c.UrlSlug == slug);
            }
            else
            {
                throw new InvalidOperationException("Category not found");
            }
        }

        public Category RetrieveCategoryBySlug(string slug)
        {
            if(_context.Categories.Any(c => c.UrlSlug == slug))
            {
                return _context.Categories.Single(c => c.UrlSlug == slug);
            }
            else
            {
                throw new InvalidOperationException("Category not found");
            }
        }

        async public Task<string> RetrieveTitleFromPageAsync(string prependedUrl)
        {
            string title;
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 10);
            Uri uri = new Uri(prependedUrl);
            HttpResponseMessage t;
            try
            {
                 t = await client.GetAsync(uri);
                if (t.IsSuccessStatusCode)
                {
                    string body = await t.Content.ReadAsStringAsync();
                    var html = new HtmlDocument();
                    html.LoadHtml(body);

                    html.OptionFixNestedTags = true;

                    var s = html.DocumentNode.Descendants("title").SingleOrDefault();
                    if (s == null)
                    {
                        title = "no title";
                    }
                    else
                    {
                        title = s.InnerText;
                    }


                }
                else
                {
                    title = "unable to retrieve page title";
                }
            }  
            catch(Exception e)
            {
                title = "timed out: unable to retrieve title";
            }
           
            //string body = await client.GetStringAsync(uri);
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
