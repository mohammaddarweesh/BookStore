using BookStore.Dtos;
using BookStore.Enums;

namespace BookStore.Services.IServices
{
    public interface IBookService : IBaseService<BookDto>
    {
        Task<IEnumerable<BookDto>> SearchAsync(string title, string author, Genre? genre);
    }
}
