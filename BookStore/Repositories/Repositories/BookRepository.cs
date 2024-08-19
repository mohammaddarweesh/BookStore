using BookStore.Data;
using BookStore.Enums;
using BookStore.Models;
using BookStore.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repositories.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> SearchAsync(string title, string author, Genre? genre)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(b => b.Title.Contains(title));

            if (!string.IsNullOrEmpty(author))
                query = query.Where(b => b.Author.Contains(author));

            if (genre.HasValue)
                query = query.Where(b => b.Genre == genre.Value);

            return await query.ToListAsync();
        }
    }
}
