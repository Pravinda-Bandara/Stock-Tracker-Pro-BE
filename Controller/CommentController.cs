
using api.Dtos.Comment;
using api.Extentions;
using api.Helpers;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;

        public CommentController(ICommentRepository commentRepository, IMapper mapper, IStockRepository stockRepository, UserManager<AppUser> userManager, IFMPService fmpService)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _stockRepository = stockRepository;
            _userManager = userManager;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject queryObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var comments = await _commentRepository.GetAllAsync(queryObject);
            var commentDtos = _mapper.Map<List<CommentDto>>(comments);
            return Ok(commentDtos);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepository.GetByIdAsync(id);
            var commentDto = _mapper.Map<CommentDto>(comment);
            return Ok(commentDto);
        }

        [HttpPost("{symbol:alpha}")]
        public async Task<IActionResult> Create([FromRoute] string symbol, CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);
            if (stock == null) 
            { 
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("Stock dose not exist");
                }
                else
                {
                    await _stockRepository.CreateAsync(stock);
                }
            }

            var useName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(useName);

            var commentModel = _mapper.Map<Comment>(commentDto);
            commentModel.AppUserId = appUser.Id;
            commentModel.StockId = stock.Id;
            var comment = await _commentRepository.CreateAsync(commentModel);

            var responseCommentmodel = _mapper.Map<CreateCommentDto>(comment);
            return Ok(responseCommentmodel);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var commentModel = _mapper.Map<Comment>(updateDto);
            var CommentDto=await _commentRepository.UpdateAsync(id, commentModel);

            if (CommentDto == null)
            {
                return NotFound("Comment not found");
            }
            var ResCommentModel=_mapper.Map<Comment>(CommentDto);
            return Ok(ResCommentModel);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var commentModel = await _commentRepository.DeleteAsync(id);
            if(commentModel != null) 
            { 
                return NotFound("comment does bit exist");
            }
            return Ok();
        }
    }
}
