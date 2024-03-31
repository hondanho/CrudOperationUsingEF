
using XLeech.Core.Model;

namespace XLeech.Core
{
    public interface IStorage
    {
        Task<bool> IsExistCategory(CategoryModel post);
        Task<bool> IsExistPost(CategoryModel post);
        Task<bool> SavePost(PostModel post);
        Task<bool> SaveCategory(CategoryModel category);
    }
}
