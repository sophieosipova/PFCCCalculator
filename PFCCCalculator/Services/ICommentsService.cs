using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using PFCCCalculatorService.Models;
using SharedModels;

namespace PFCCCalculatorService.Services
{
    public interface ICommentsService
    {


        Task<List<Comment>> GetCommentsByUserId(int userId);

        //Task<IActionResult> CreateComment(Comment comment);

          Task<bool> DeleteComment(int userId,int commentId);

        // Task<IActionResult> UpdateComment(Comment commentToUpdate);

        Task<PaginatedModel<Comment>> GetCommentsByDishId(int dishId, int pageSize = 10, int pageIndex = 0);
      
    }
}
