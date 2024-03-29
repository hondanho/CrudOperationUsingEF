
using DotnetCrawler.Data.ModelDb;
using System.Threading.Tasks;
using WordPressPCL;

namespace XLeech.Core
{
    public interface IStorage
    {
        Task SyncDataBySite(SiteConfigDb siteConfig);
    }
}
