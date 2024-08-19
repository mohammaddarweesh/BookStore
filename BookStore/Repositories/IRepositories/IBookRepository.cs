using BookStore.Enums;
using BookStore.Models;

namespace BookStore.Repositories.IRepositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<IEnumerable<Book>> SearchAsync(string title, string author, Genre? genre);
    }
}
