using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dishes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFCCCalculatorService.Services;

namespace PFCCCalculatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {

        private readonly IDishesService dishesService;

        public DishesController(IDishesService dishesService)
        {
            this.dishesService = dishesService;
        }


        // GET api/products
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Dish>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishes()
        {         
            return Ok(await dishesService.GetDishes());
        }

        [HttpGet]
        [Route("{dishId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Dish), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishById(int dishId)
        {
            if (dishId <= 0)
                return BadRequest();

            var dish = await dishesService.GetDishById(dishId);

            if (dish != null)
                return Ok(dish);

            return NotFound();
        }

    }
}