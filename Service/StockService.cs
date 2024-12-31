using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using AutoMapper;

namespace api.Service
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;

        public StockService(IStockRepository stockRepository, IMapper mapper)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;
        }

        public async Task<List<StockDto>> GetAllAsync(QueryObject query)
        {
            var stocks = await _stockRepository.GetAllAsync();

            // Apply filtering
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol)).ToList();
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                stocks = query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)
                    ? (query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol).ToList() : stocks.OrderBy(s => s.Symbol).ToList())
                    : stocks;
            }

            // Apply pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            stocks = stocks.Skip(skipNumber).Take(query.PageSize).ToList();

            return _mapper.Map<List<StockDto>>(stocks);
        }

        public async Task<StockDto?> GetByIdAsync(int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            return _mapper.Map<StockDto>(stock);
        }

        public async Task<StockDto?> GetBySymbolAsync(string symbol)
        {
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            return _mapper.Map<StockDto>(stock);
        }

        public async Task<StockDto> CreateAsync(CreateStockRequestDto stockDto)
        {
            var stockModel = _mapper.Map<Stock>(stockDto);
            var createdStock = await _stockRepository.CreateAsync(stockModel);
            return _mapper.Map<StockDto>(createdStock);
        }

        public async Task<StockDto?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _stockRepository.GetByIdAsync(id);
            if (existingStock == null)
            {
                return null;
            }

            _mapper.Map(stockDto, existingStock);
            var updatedStock = await _stockRepository.UpdateAsync(existingStock);
            return _mapper.Map<StockDto>(updatedStock);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedStock = await _stockRepository.DeleteAsync(id);
            return deletedStock != null;
        }
    }
}