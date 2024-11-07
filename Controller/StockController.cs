using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery]QueryObject query) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stoks = await _stockRepository.GetAllAsync(query); 
            var stockDtos=_mapper.Map<List<StockDto>>(stoks);
            return Ok(stockDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock =await _stockRepository.GetByIdAsync(id);

            var stockDto= _mapper.Map<StockDto>(stock);
            return Ok(stockDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateStockRequestDto stock) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel=_mapper.Map<Stock>(stock);
            await _stockRepository.CreateAsync(stockModel);

            var stockDto = _mapper.Map<StockDto>(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockDto);
        
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockDto) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = await _stockRepository.UpdateAsync(id,updateStockDto);
            var stockDto = _mapper.Map<StockDto>(stockModel);
            return Ok(stockDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = await _stockRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
