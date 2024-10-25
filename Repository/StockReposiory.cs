using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockReposiory : IStockRepository
        
    {
        private readonly ApplicationDBContext _context;
        public StockReposiory(ApplicationDBContext context)
        {
            _context = context; 
        }
        public Task<List<Stock>> GetAllAsync()
        { 
           return _context.Stocks.ToListAsync();
        }
    }
}
