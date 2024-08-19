using AutoMapper;
using BookStore.Dtos;
using BookStore.Enums;
using BookStore.Models;
using BookStore.Repositories.IRepositories;
using BookStore.Services.IServices;

namespace BookStore.Services.Services
{
    public class BookService : BaseService<Book, BookDto>, IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository, IMapper mapper)
            : base(bookRepository, mapper)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<BookDto>> SearchAsync(string title, string author, Genre? genre)
        {
            var books = await _bookRepository.SearchAsync(title, author, genre);
            return books.Select(book => _mapper.Map<BookDto>(book));
        }
    }
}
