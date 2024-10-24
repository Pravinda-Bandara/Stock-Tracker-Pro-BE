using api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace api.Controller
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController (ApplicationDBContext context)
        {
          _context= context;
        }


        [HttpGet]
        public IActionResult GetAll() { 
            var stoks=_context.Stocks.ToList(); 
            return Ok(stoks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        { 
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock);
        }

    }
}
