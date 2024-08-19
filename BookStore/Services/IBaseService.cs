namespace BookStore.Services
{
    public interface IBaseService<TDto> where TDto : class
    {
        Task<TDto> GetByIdAsync(int id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task AddAsync(TDto dto);
        Task UpdateAsync(TDto dto);
        Task DeleteAsync(int id);
    }
}
