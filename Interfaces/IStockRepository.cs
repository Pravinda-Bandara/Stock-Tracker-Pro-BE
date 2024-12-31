using api.Dtos.Stock;
using api.Helpers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> DeleteAsync(int id);
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<bool> StockExists(int id);
        Task<Stock?> UpdateAsync(Stock stockModel);
    }
}
