using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommentsService.Models;
using System.Net;
using SharedModels;
using CommentsService.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CommentsService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        private readonly ICommentsRepository commentsRepository;

        public CommentsController(ICommentsRepository commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

       
      /*  [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Comment>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetComments()
        {
            var comments = await commentsRepository.GetComments();

            if (comments == null)
                return NotFound();

            return Ok(comments);
        } */


        [HttpGet, AllowAnonymous]
        [Route("{commentId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Comment), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            if (commentId <= 0)
                return BadRequest();

            var comment = await commentsRepository.GetCommentById(commentId);

            if (comment != null)
                return Ok(comment);

            return NotFound();
        }

  /*      [HttpGet]
        [Route("user/{userId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Comment>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentsByUserId(int userId)
        {
            if (userId <= 0)
                return BadRequest();

            var comments = await commentsRepository.GetCommentsByUserId(userId);

            if (comments != null)
                return Ok(comments);

            return NotFound();
        }*/



        [Route("user/{userId}")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateComment (string userId, Comment comment)
        {
            var c = await commentsRepository.CreateComment(userId, comment);

            if (c == null)
                return Conflict();

            return Created("", c);
        }

        
        [Route("user/{userId}/{commentId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteComment(string userId,int commentId)
        {
            if (userId == "")
                return BadRequest();

            if (await commentsRepository.DeleteComment(userId, commentId))
                return NoContent();

            return NotFound();
        }

        [Route("user/{userId}")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateComment(string userId, [FromBody]Comment commentToUpdate)
        {
            var comment = await commentsRepository.UpdateComment(userId, commentToUpdate);

            if (comment == null)
                return NotFound();

            return Created("", comment);
        }


        /*[HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedModel<Comment>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Comment>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Items([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var totalItems = await db.Comments
                .LongCountAsync();

            var itemsOnPage = await  db.Comments
                .OrderBy(c => c.CommentId)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedModel<Comment>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        } */


        [HttpGet, AllowAnonymous]
        [Route("dish/{dishId:int}")]
        [ProducesResponseType(typeof(PaginatedModel<Comment>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentsByDishId(int dishId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            if (dishId <= 0)
                return BadRequest();

            var comments = await commentsRepository.GetCommentsByDishId(dishId, pageSize, pageIndex);
              
            if (comments != null)
                return Ok(comments);

            return NotFound();
        }



    }
}


   

       