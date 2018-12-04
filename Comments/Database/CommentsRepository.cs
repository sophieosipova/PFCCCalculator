using CommentsService.Models;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsService.Database
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly CommentsContext db;

        public CommentsRepository(CommentsContext context)
        {
            this.db = context;
            if (!db.Comments.Any())
            {
                db.Comments.Add(new Comment { UserId = 1, CommentText = "WOW!", DishId = 1 });
                db.Comments.Add(new Comment { UserId = 2, CommentText = "Вкусно!", DishId = 1 });

                db.SaveChanges();
            }
        }


   /*     public async Task<List<Comment>> GetComments()
        {
            try
            {
                var comments = await db.Comments.ToListAsync();
                if (comments.Count == 0)
                    return null;
                return comments;
            }
            catch
            {
                return null;
            }
        }*/


        public async Task<Comment> GetCommentById(int commentId)
        {
            try
            {
                return  await db.Comments.SingleOrDefaultAsync(c => c.CommentId == commentId);
            }
            catch
            {
                 return null;
            }
        }


      /*  public async Task<List<Comment>> GetCommentsByUserId(int userId)
        {
            try
            { 
                var comments = await db.Comments.Where(c => c.UserId == userId).ToListAsync();

                if (comments.Count == 0)
                    return null;
                return comments;
            }
            catch
            {
                return null;
            }
        } */

        public async Task<Comment> CreateComment(int userId, Comment comment)
        {
            var item = new Comment
            {
                CommentText = comment.CommentText,
                UserId = comment.UserId,
                DishId = comment.DishId,
            };

            try
            {
                db.Comments.Add(item);
                db.SaveChanges();

                return await db.Comments.LastAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteComment(int userId, int commentId)
        {
            try
            {
                var comment = db.Comments.SingleOrDefault(c => c.CommentId == commentId);

                if (comment == null)
                    return false;

                db.Comments.Remove(comment);
                await db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true; ;
        }


        public async Task<Comment> UpdateComment(int userId, Comment commentToUpdate)
        {
            try
            {
                var comment = await db.Comments
                    .SingleOrDefaultAsync(c => c.CommentId == commentToUpdate.CommentId);

                if (comment == null)
                    return null;

                comment = commentToUpdate;

                db.Comments.Update(comment);
                await db.SaveChangesAsync();

                return comment;
            }
            catch
            {
                return null;
            }

        }

        public async Task<PaginatedModel<Comment>> GetCommentsByDishId(int dishId, int pageSize = 10, int pageIndex = 0)
        {
            try
            {
                var comments = await db.Comments.Where(c => c.DishId == dishId).ToListAsync();

                var totalItems = comments.Count();

                if (totalItems != 0)
                {
                    var itemsOnPage = comments
                    .OrderByDescending(c => c.CommentId)
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .ToList();

                    var model = new PaginatedModel<Comment>(pageIndex, pageSize, totalItems, itemsOnPage);

                    return model;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }


    }
}
