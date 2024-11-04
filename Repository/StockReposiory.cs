using api.Data;
using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockReposiory : IStockRepository
        
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        public StockReposiory(ApplicationDBContext context , IMapper mapper)
        {
            _context = context; 
            _mapper = mapper;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null) 
            {
                return null;
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks =  _context.Stocks
                           .Include(c => c.Comments)
                           .AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            { 
                stocks=stocks.Where(s=>s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            return await stocks.ToListAsync();
        }


        public async Task<Stock?> GetByIdAsync(int id)
        {
            return  await _context.Stocks
                                      .Include(c => c.Comments)
                                      .FirstOrDefaultAsync(i => i.Id == id);

        }

        public async Task<bool> StockExists(int id)
        {
            return await _context.Stocks.AnyAsync(s=>s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x=>x.Id == id);
            if (existingStock == null) 
            {
                return null;
            }
            _mapper.Map(stockDto, existingStock);
            await _context.SaveChangesAsync();
            return existingStock;
        }
    }
}
