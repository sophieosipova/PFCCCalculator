
using System.Collections.Generic;
using System.Threading.Tasks;

using PFCCCalculatorService.Models;
using SharedModels;

namespace PFCCCalculatorService.Services
{
    public interface ICommentsService
    {


        Task<List<CommentModel>> GetCommentsByUserId(int userId);

        //Task<IActionResult> CreateComment(Comment comment);

          Task<bool> DeleteComment(int userId,int commentId);

        // Task<IActionResult> UpdateComment(Comment commentToUpdate);

        Task<PaginatedModel<CommentModel>> GetCommentsByDishId(int dishId, int pageSize = 10, int pageIndex = 0);
      
    }
}
