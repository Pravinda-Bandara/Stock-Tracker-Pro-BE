using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Controller
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IStockRepository _stockRepository;
        public StockController (ApplicationDBContext context , IMapper mapper ,IStockRepository stockRepository)
        {
          _context= context;
          _mapper=mapper;
          _stockRepository=stockRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var stoks = await _stockRepository.GetAllAsync(); 
            var stockDtos=_mapper.Map<List<StockDto>>(stoks);
            return Ok(stockDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        { 
            var stock =await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            var stockDto= _mapper.Map<StockDto>(stock);
            return Ok(stockDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateStockRequestDto stock) { 
            var stockModel=_mapper.Map<Stock>(stock);
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            var stockDto = _mapper.Map<StockDto>(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockDto);
        
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockDto) 
        { 
            var stockModel =await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (stockModel == null) 
            {
                return NotFound();
            }
            _mapper.Map(updateStockDto, stockModel);
            await _context.SaveChangesAsync();
            var stockDto = _mapper.Map<StockDto>(stockModel);
            return Ok(stockDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) 
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (stockModel == null)
            {
                return NotFound(nameof(GetById));
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
