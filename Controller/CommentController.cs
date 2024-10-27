
using api.Dtos.Comment;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
    
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var comments=await _commentRepository.GetAllAsync();
            var commentDtos = _mapper.Map<List<CommentDto>>(comments);
            return Ok(commentDtos);
        }
    }

}
