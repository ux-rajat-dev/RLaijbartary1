using LibraryManagementSystem.admin.CommandModels;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.admin.Interfaces
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task<bool> CreateAsync(AuthorCommandModel model);
        Task<bool> UpdateAsync(AuthorCommandModel model);
        Task<bool> DeleteAsync(int id);
    }
}
