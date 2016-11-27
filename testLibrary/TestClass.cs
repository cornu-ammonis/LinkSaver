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

        [TestCase]
        public async Task LinksByCategoryToListAsyncTest()
        {
            providerClass provider = new providerClass();
            ILinkRepository repository = provider.giveRepository();
            ApplicationDbContext context = provider.giveContext();

            Category testCategory = context.Categories.FirstOrDefault();

            List<Link> testList = await repository.LinksByCategoryToListAsync(testCategory.UrlSlug);
           /* Link incorrect = context.Links.Include<Link, Category>(l => l.category)
                .First(l => l.category.Name == "new");
            testList.Add(incorrect); */ //code which adds a link of the wrong category to make sure test fails
            List<string> slugsList = new List<string>();
            
            foreach (Link link in testList)
            {
                slugsList.Add(link.category.UrlSlug);
            }

            Assert.That(slugsList, Is.All.EqualTo(testCategory.UrlSlug));
        }

        [TestCase]
        public void LinksByCategoryToListTest()
        {
            providerClass provider = new providerClass();
            ILinkRepository repository = provider.giveRepository();
            ApplicationDbContext context = provider.giveContext();

            Category testCategory = context.Categories.FirstOrDefault();

            List<Link> testList = repository.LinksByCategoryToList(testCategory.UrlSlug);
            List<string> slugsList = new List<string>();

            foreach (Link link in testList)
            {
                slugsList.Add(link.category.UrlSlug);
            }

            Assert.That(slugsList, Is.All.EqualTo(testCategory.UrlSlug));

        }

        [TestCase]
        public async Task LinksByCategoryAsyncAndSyncAreEquivalent()
        {
            providerClass provider = new providerClass();
            ILinkRepository repository = provider.giveRepository();
            ApplicationDbContext context = provider.giveContext();

            Category testCategory = context.Categories.FirstOrDefault();
            List<Link> asyncTestList = await repository.LinksByCategoryToListAsync(testCategory.UrlSlug);
            List<Link> syncTestList = repository.LinksByCategoryToList(testCategory.UrlSlug);
            Assert.That(syncTestList, Is.EquivalentTo(asyncTestList));

            
        }

        [TestCase]
        public async Task AddLinkToDatabaseAsyncTest()
        {
            providerClass provider = new providerClass();
            ILinkRepository repository = provider.giveRepository();
            ApplicationDbContext context = provider.giveContext();
            LinkCreationViewModel testCreate = new LinkCreationViewModel();
            testCreate.url = "yahoo.com";
            testCreate.category = "unittestcat";
            await repository.AddLinkToDatabaseAsync(testCreate);
            
            Assert.That(context.Links.Any(l => l.category.Name == testCreate.category && l.url == testCreate.url));
            context.Links.Remove(context.Links.Single(l => l.category.Name == testCreate.category &&
            l.url == testCreate.url));
            context.SaveChanges();
        }

       /* [TestCase]
        public async Task VerifyOnlyOneLinkCreated()
        {
            providerClass provider = new providerClass();
            ILinkRepository repository = provider.giveRepository();
            ApplicationDbContext context = provider.giveContext();
            Category testCategory = context.Categories.SingleOrDefault(c => c.Name == "unittestcat");
            List<Link> testList = await repository.LinksByCategoryToListAsync(testCategory.UrlSlug);

            Assert.That(testList.Count, Is.EqualTo(1));
        }

        [TestCase]
        public async Task VerifyCorrectUrl()
        {
            providerClass provider = new providerClass();
            ILinkRepository repository = provider.giveRepository();
            ApplicationDbContext context = provider.giveContext();
            Link linkTest = await context.Links.FirstOrDefaultAsync(l => l.category.Name == "unittestcat");
            Assert.That(linkTest.url, Is.EqualTo("yahoo.com"));
        }

        [TestCase]
        public async Task DeleteListFromDatabaseAsyncTest()
        {
            providerClass provider = new providerClass();
            ILinkRepository repository = provider.giveRepository();
            ApplicationDbContext context = provider.giveContext();
            Link linkTest = await context.Links
                .SingleAsync(l => l.category.Name == "unittestcat" && l.url == "yahoo.com");

            await repository.DeleteLinkFromDatabaseAsync(linkTest.LinkId);
            
            Assert.That(!context.Links.Any(l => l.category.Name == "unittestcat" && l.url == "yahoo.com"));
        }*/


    }
}
