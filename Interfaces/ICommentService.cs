using api.Dtos.Comment;
using api.Helpers;

namespace api.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetAllAsync(CommentQueryObject queryObject);
        Task<CommentDto?> GetByIdAsync(int id);
        Task<CreateCommentDto?> CreateAsync(string symbol, CreateCommentDto commentDto, string username);
        Task<CommentDto?> UpdateAsync(int id, UpdateCommentRequestDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
