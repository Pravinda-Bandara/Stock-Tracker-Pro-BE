using api.Dtos.Stock;
using api.Helpers;

namespace api.Interfaces
{
    public interface IStockService
    {
        Task<List<StockDto>> GetAllAsync(QueryObject query);
        Task<StockDto?> GetByIdAsync(int id);
        Task<StockDto?> GetBySymbolAsync(string symbol);
        Task<StockDto> CreateAsync(CreateStockRequestDto stockDto);
        Task<StockDto?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<bool> DeleteAsync(int id);
    }
}
