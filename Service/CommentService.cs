using api.Dtos.Comment;
using api.Helpers;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace api.Service
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IFMPService _fmpService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public CommentService(
            ICommentRepository commentRepository,
            IStockRepository stockRepository,
            IFMPService fmpService,
            IMapper mapper,
            UserManager<AppUser> userManager)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _fmpService = fmpService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<CommentDto>> GetAllAsync(CommentQueryObject queryObject)
        {
            var comments = await _commentRepository.GetAllAsync(queryObject);
            return _mapper.Map<List<CommentDto>>(comments);
        }

        public async Task<CommentDto?> GetByIdAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return comment != null ? _mapper.Map<CommentDto>(comment) : null;
        }

        public async Task<CreateCommentDto?> CreateAsync(string symbol, CreateCommentDto commentDto, string username)
        {
            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null) return null;
                await _stockRepository.CreateAsync(stock);
            }

            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null) throw new Exception("User not found");

            var commentModel = _mapper.Map<Comment>(commentDto);
            commentModel.AppUserId = appUser.Id;
            commentModel.StockId = stock.Id;

            var comment = await _commentRepository.CreateAsync(commentModel);
            return _mapper.Map<CreateCommentDto>(comment);
        }

        public async Task<CommentDto?> UpdateAsync(int id, UpdateCommentRequestDto updateDto)
        {
            var existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null) return null;

            _mapper.Map(updateDto, existingComment);
            var updatedComment = await _commentRepository.UpdateAsync(id, existingComment);
            return updatedComment != null ? _mapper.Map<CommentDto>(updatedComment) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedComment = await _commentRepository.DeleteAsync(id);
            return deletedComment != null;
        }
    }
}
