using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using LinkSaver;
using LinkSaver.Models;
using Microsoft.Extensions.DependencyInjection;
using LinkSaver.Data;
using Microsoft.EntityFrameworkCore;

namespace testLibrary
{
    [TestFixture]
    public class TestClass
    {
        /*ILinkRepository testRepository;
        public TestClass(ILinkRepository _testRepository )
        {
            testRepository = _testRepository;
        } */

        [TestCase]
        public async Task AllLinksRepositoryTest()
        {

            providerClass provider = new providerClass();
            ILinkRepository repository = provider.giveRepository();
            ApplicationDbContext context = provider.giveContext();

            List<Link> repositoryList = await repository.AllLinksToListAsync();
            List<Link> contextList = await context.Links.Include<Link, Category>(l => l.category).ToListAsync();
             
            Assert.That(repositoryList, Is.EqualTo(contextList));
            /*
            foreach (Link link in repositoryList)
            {
                Link contextLink = contextList.Single(l => l.LinkId == link.LinkId);
                contextLink.category = new Category();
                contextLink.category.UrlSlug = "notthis";
                Assert.AreEqual(link, contextLink);
                Assert.AreEqual(link.category.UrlSlug, contextLink.category.UrlSlug);
            } */
        }

        [TestCase]
        public async Task AllToListCorrectCategoriesRepositoryTest()
        {
            providerClass provider = new providerClass();
            ILinkRepository repository = provider.giveRepository();
            ApplicationDbContext context = provider.giveContext();

            List<Link> repositoryList = await repository.AllLinksToListAsync();
            List<Link> contextList = await context.Links.Include<Link, Category>(l => l.category).ToListAsync();

            List<Category> repositoryCategories = new List<Category>();
            List<Category> contextCategories = new List<Category>();
          
            foreach(Link link in repositoryList)
            {
                repositoryCategories.Add(link.category);
            }
            

            foreach(Link link in contextList)
            {
                contextCategories.Add(link.category);
            }

            Assert.That(repositoryCategories, Is.EquivalentTo(contextCategories));
        }
    }
}
