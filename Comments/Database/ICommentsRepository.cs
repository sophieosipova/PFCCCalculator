using CommentsService.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsService.Database
{
    public interface ICommentsRepository
    {
        Task<List<Comment>> GetComments();
        Task<Comment> GetCommentById(int commentId);
        Task<List<Comment>> GetCommentsByUserId(int userId);
        Task<Comment> CreateComment(int userId,Comment comment);
        Task<bool> DeleteComment(int userId, int commentId);
        Task<Comment> UpdateComment(int userId, Comment commentToUpdate);
        Task<PaginatedModel<Comment>> GetCommentsByDishId(int dishId, int pageSize = 10, int pageIndex = 0);


    }
}
