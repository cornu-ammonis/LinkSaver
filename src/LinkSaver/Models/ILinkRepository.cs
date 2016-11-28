using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models
{
    public interface ILinkRepository
    {
        Task AddLinkToDatabaseAsync(Link link);
        Task<ApplicationUser> AddLinkToDatabaseAsync(LinkCreationViewModel linkCreationModel, ApplicationUser user);
        Task DeleteLinkFromDatabaseAsync(int id);

        Task<Category> CreateOrRetrieveCategoryByName(string name);
        Task<Category> RetrieveCategoryBySlugAsync(string slug);
        Category RetrieveCategoryBySlug(string slug);
        Task<string> RetrieveTitleFromPageAsync(string url);

        void AddOrUpdateCategory(Category category);

        string prependUrl(string unprocessed_url);

        Task<List<Link>> AllLinksToListAsync(ApplicationUser user);
        Task<List<Link>> LinksByCategoryToListAsync(string categorySlug);
        List<Link> LinksByCategoryToList(string categorySlug);
        List<Link> UserLinksByCategoryToList(string categorySlug, ApplicationUser user);
        List<Link> PublicLinksByCategoryToList(string categorySlug, ApplicationUser user);

        Task<ApplicationUser> SavePostForUserAsync(int linkId, ApplicationUser user);
        Task<ApplicationUser> UnsavePostForUserAsync(int linkId, ApplicationUser user);
        Task<bool> CheckIfSavedAsync(int linkId, string userName);

        Task<Link> RetrieveLinkByIdAsync(int linkId);

    }
}
