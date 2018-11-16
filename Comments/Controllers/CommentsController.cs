using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Comments.Models;
using System.Net;

using Microsoft.EntityFrameworkCore;



namespace CommentsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        private readonly CommentsContext db;

        public CommentsController(CommentsContext context)
        {
            this.db = context;
            if (!db.Comments.Any())
            {
                db.Comments.Add(new Comment{ UserId = 1, CommentText = "WOW!",DishId = 1 });
                db.Comments.Add(new Comment { UserId = 2, CommentText = "Вкусно!", DishId = 1 });
               
                db.SaveChanges();
            }
        }

       
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Comment>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetComments()
        {
            return Ok(await db.Comments.ToListAsync());
        }


        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Comment), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            if (commentId <= 0)
            {
                return BadRequest();
            }

            var product = await db.Comments.SingleOrDefaultAsync(c => c.CommentId == commentId);


            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("user/{UserId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Comment>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentsByUserId(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest();
            }

            var comments = await db.Comments.Where(c => c.UserId == userId).ToListAsync();


            if (comments != null)
            {
                return Ok(comments);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("dish/{dishId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Comment>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentsByDishId(int dishId)
        {
            if (dishId <= 0)
            {
                return BadRequest();
            }

            var comments = await db.Comments.Where(c => c.DishId == dishId).ToListAsync();


            if (comments.Count != 0)
            {
                return Ok(comments);
            }

            return NotFound();
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateComment ([FromBody]Comment comment)
        {
            var item = new Comment
            {
                CommentId = comment.CommentId,
                CommentText = comment.CommentText,
                UserId = comment.UserId,
                DishId = comment.DishId,
            };
            db.Comments.Add(item);

            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommentById), new { commentId = item.CommentId }, null);
        }

        /
        [Route("{id:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = db.Comments.SingleOrDefault(c => c.CommentId == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comment);

            await db.SaveChangesAsync();

            return NoContent();
        }

        [Route("")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateComment([FromBody]Comment commentToUpdate)
        {
            var comment = await db.Comments
                .SingleOrDefaultAsync(c => c.CommentId== commentToUpdate.CommentId);

            if (comment == null)
            {
                return NotFound(new { Message = $"Product with id {commentToUpdate.CommentId} not found." });
            }


            comment = commentToUpdate;
            db.Comments.Update(comment);

            await db.SaveChangesAsync();


            return CreatedAtAction(nameof(GetCommentById), new { commentId = commentToUpdate.CommentId }, null);
        }

    }
}


   

       