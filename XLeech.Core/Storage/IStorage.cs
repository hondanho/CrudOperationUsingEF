

using XLeech.Core.Model;

namespace XLeech.Core
{
    public interface IStorage
    {
        Task<bool> SavePost(PostModel post);
        Task<bool> SaveCategory(CategoryModel category);
    }
}
