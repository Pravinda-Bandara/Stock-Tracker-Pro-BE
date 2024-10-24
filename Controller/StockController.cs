using api.Data;
using api.Dtos.Stock;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace api.Controller
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        public StockController (ApplicationDBContext context , IMapper mapper)
        {
          _context= context;
          _mapper=mapper;
        }


        [HttpGet]
        public IActionResult GetAll() { 
            var stoks=_context.Stocks.ToList(); 
            var stockDto=_mapper.Map<List<StockDto>>(stoks);
            return Ok(stockDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        { 
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            var stockDto= _mapper.Map<StockDto>(stock);
            return Ok(stockDto);
        }

    }
}
