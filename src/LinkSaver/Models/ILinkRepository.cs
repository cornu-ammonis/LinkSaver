using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkSaver.Models
{
    public interface ILinkRepository
    {
        Task AddLinkToDatabaseAsync(Link link);
        Task DeleteLinkFromDatabaseAsync(int id);

        Task<string> RetrieveTitleFromPageAsync(string url);

        string prependUrl(string unprocessed_url);

        Task<List<Link>> AllLinksToListAsync();
    }
}
