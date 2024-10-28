using api.Data;
using api.Dtos.Comment;
using api.Dtos.Stock;
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

        public Task<List<Stock>> GetAllAsync()
        {
            return _context.Stocks
                           .Include(c => c.Comments)
                           .ToListAsync();
        }


        public async Task<Stock?> GetByIdAsync(int id)
        {
            return  await _context.Stocks
                                      .Include(c => c.Comments)
                                      .FirstOrDefaultAsync(i => i.Id == id);

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
