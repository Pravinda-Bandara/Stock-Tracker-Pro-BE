
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
    
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IMapper mapper, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDtos = _mapper.Map<List<CommentDto>>(comments);
            return Ok(commentDtos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            var commentDto = _mapper.Map<CommentDto>(comment);
            return Ok(commentDto);
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId ,CreateCommentDto commentDto) 
        {
            if (!await _stockRepository.StockExists(stockId)) 
            {
                return BadRequest("Stock doenot exist");
            }
            var commentModel=_mapper.Map<Comment>(commentDto);
            commentModel.StockId = stockId;
            var comment=await _commentRepository.CreateAsync(commentModel);

            var responseCommentmodel= _mapper.Map<CreateCommentDto>(comment);
            return Ok(responseCommentmodel);
        }
    }
}
