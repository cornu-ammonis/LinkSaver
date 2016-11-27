using LinkSaver.Data;
using LinkSaver.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestLibrary
{
    public class providerClass
    {

        ApplicationDbContext _context;
        ILinkRepository _repository;
        public providerClass()
        {
            IServiceCollection collection = new ServiceCollection();
            collection.AddScoped<ILinkRepository, LinkRepository>();
            collection.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-LinkSaver-15b46fb3-af8e-414e-82f5-c7bb6e61402a;Trusted_Connection=True;MultipleActiveResultSets=true"));
            IServiceProvider provider = collection.BuildServiceProvider();
            _repository = provider.GetService<ILinkRepository>();
            _context = provider.GetService<ApplicationDbContext>();
        }

        public ApplicationDbContext giveContext()
        {
            return _context;
        }

        public ILinkRepository giveRepository()
        {
            return _repository;
        }

    }
}
