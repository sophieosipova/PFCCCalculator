using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommentsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("user/{userId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Comment>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentsByUserId(int userId)
        {
            if (userId <= 0)
                return BadRequest();

            var comments = await commentsService.GetCommentsByUserId(userId);

            if (comments != null)
                return Ok(comments);

            return NotFound();
        }


        [HttpGet]
        [Route("dish/{dishId:int}")]
        [ProducesResponseType(typeof(PaginatedModel<Comment>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCommentsByDishId(int dishId, [FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            if (dishId <= 0)
                return BadRequest();

            var model= await commentsService.GetCommentsByDishId(dishId, pageSize, pageIndex);

            if (model != null)
                return Ok(model);

            return NotFound();
        }
    }
}