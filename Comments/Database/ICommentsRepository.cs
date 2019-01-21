using CommentsService.Models;
using SharedModels;
using System.Threading.Tasks;

namespace CommentsService.Database
{
    public interface ICommentsRepository 
    {
    //    Task<List<Comment>> GetComments();
        Task<Comment> GetCommentById(int commentId);
    //    Task<List<Comment>> GetCommentsByUserId(int userId);
        Task<Comment> CreateComment(string userId,Comment comment);
        Task<bool> DeleteComment(string userId, int commentId);
        Task<Comment> UpdateComment(string userId, Comment commentToUpdate);
        Task<PaginatedModel<Comment>> GetCommentsByDishId(int dishId, int pageSize = 10, int pageIndex = 0);


    }
}
