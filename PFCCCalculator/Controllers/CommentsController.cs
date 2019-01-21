using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PFCCCalculatorService.Models;
using PFCCCalculatorService.Services;
using SharedModels;

namespace PFCCCalculatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService commentsService;
        private readonly ILogger<ICommentsService> logger;
        private readonly IAutorizationService autorizationService;
        public CommentsController(ICommentsService commentsService,   IAutorizationService autorizationService, ILogger<ICommentsService> logger)
        {
            this.commentsService = commentsService;
            this.autorizationService = autorizationService;
            this.logger = logger;
        }

        /*   [HttpGet]
           [Route("user/{userId:int}")]
           [ProducesResponseType((int)HttpStatusCode.NotFound)]
           [ProducesResponseType(typeof(List<CommentModel>), (int)HttpStatusCode.OK)]
           public async Task<IActionResult> GetCommentsByUserId(int userId)
           {
               if (userId <= 0)
                   return BadRequest();

               var comments = await commentsService.GetCommentsByUserId(userId);

               if (comments != null)
                   return Ok(comments);

               return NotFound();
           }*/
        [Route("user/{userId:int}/{commentId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteComment(string userId, int commentId)
        {
            if (userId == "")
            {
                logger.LogInformation("DELETE --- fail");
                return BadRequest();
            }

            try
            {
                if (!await autorizationService.ValidateToken(Request.Headers["Authorization"].ToString()))
                    return Unauthorized();

                if (await commentsService.DeleteComment(userId, commentId))
                {
                    logger.LogInformation("DELETE --- success");
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                logger.LogInformation("DELETE ---", e.Message);
                return Conflict(e.Message);
            }

            logger.LogInformation("DELETE --- fail");
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [Route("user/{userId}")]
        public async Task<IActionResult> CreateComment(string userId, CommentModel comment)
        {
            try
            {
                if (comment == null)
                {
                    ModelState.AddModelError("", "Комментарий не задан");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await autorizationService.ValidateToken(Request.Headers["Authorization"].ToString()))
                    return Unauthorized();

                var created = await commentsService.CreateComment(userId, comment);
                if (created == null)
                {
                    logger.LogInformation("CREATE  --- fail");
                    return Conflict("Ошибка");
                }
                logger.LogInformation("CREATE  --- success");
                return Created("", created);

            }
            catch (Exception e)
            {
                logger.LogInformation("CREATE  --- ", e.Message);
                return Conflict(e.Message);
            }
        }

        [HttpGet]
        [Route("dish/{dishId:int}")]
        [ProducesResponseType(typeof(PaginatedModel<CommentModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentsByDishId(int dishId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            if (dishId <= 0)
            {
                logger.LogInformation("GET --- fai");
                return BadRequest();
            }
            try
            {
                var model = await commentsService.GetCommentsByDishId(dishId, pageSize, pageIndex);
                
                if (model != null)
                {
                    logger.LogInformation("GET ---", model.ToString());
                    return Ok(model);
                }
                logger.LogInformation("GET --- Not Found");
            }
            catch (Exception e)
            {
                logger.LogInformation("GET ---", e.Message);
                return Conflict(e.Message);
            }

            return NotFound();
        }
    }
}