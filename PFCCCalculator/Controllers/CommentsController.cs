using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
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

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [Route("user/{userId}/")]
        public async Task<IActionResult> CreateComment(int userId, CommentModel comment)
        {
            try
            {
                var created = await commentsService.CreateComment(userId, comment);
                if (created != null)
                    return Created("", created);
                return Conflict("Не удалось создать");

            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet]
        [Route("dish/{dishId:int}")]
        [ProducesResponseType(typeof(PaginatedModel<CommentModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentsByDishId(int dishId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            if (dishId <= 0)
                return BadRequest();
            try
            {
                var model = await commentsService.GetCommentsByDishId(dishId, pageSize, pageIndex);

                if (model != null)
                    return Ok(model);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }

            return NotFound();
        }
    }
}