using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models
{
    public interface ILinkRepository
    {
        Task AddLinkToDatabaseAsync(Link link);
        Task AddLinkToDatabaseAsync(LinkCreationViewModel linkCreationModel);
        Task DeleteLinkFromDatabaseAsync(int id);

        Task<Category> CreateOrRetrieveCategoryByName(string name);
        Task<Category> RetrieveCategoryBySlugAsync(string slug);
        Category RetrieveCategoryBySlug(string slug);
        Task<string> RetrieveTitleFromPageAsync(string url);

        void AddOrUpdateCategory(Category category);

        string prependUrl(string unprocessed_url);

        Task<List<Link>> AllLinksToListAsync();
        Task<List<Link>> LinksByCategoryToListAsync(string categorySlug);
        List<Link> LinksByCategoryToList(string categorySlug);
    }
}
