﻿
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            var commentDto = _mapper.Map<CommentDto>(comment);
            return Ok(commentDto);
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            if (!await _stockRepository.StockExists(stockId))
            {
                return BadRequest("Stock doenot exist");
            }
            var commentModel = _mapper.Map<Comment>(commentDto);
            commentModel.StockId = stockId;
            var comment = await _commentRepository.CreateAsync(commentModel);

            var responseCommentmodel = _mapper.Map<CreateCommentDto>(comment);
            return Ok(responseCommentmodel);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
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
            var commentModel = await _commentRepository.DeleteAsync(id);
            if(commentModel != null) 
            { 
                return NotFound("comment does bit exist");
            }
            return Ok();
        }
    }
}
