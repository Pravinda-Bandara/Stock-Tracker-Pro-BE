using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context) 
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentmodel)
        {
            await _context.Comments.AddAsync(commentmodel);
            await _context.SaveChangesAsync();
            return commentmodel;
        }

        public async Task<Comment> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (commentModel != null) 
            {
                return null;
            }
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment =  await _context.Comments.FindAsync(id);
            if (comment == null) 
            {
                return null; 
            }
            return comment;
        }
    }
}
