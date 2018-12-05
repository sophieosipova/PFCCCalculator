﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dishes.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using DishesService.Database;

namespace DishesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IDishesRepository dishesRepository;

        public DishesController(IDishesRepository dishesRepository)
        {
            this.dishesRepository = dishesRepository;
        }

        // GET api/products
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Dish>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishes()
        {
            var dishes = await dishesRepository.GetDishes();

            if (dishes == null)
                return NotFound();

            return Ok(dishes);
        }
        

        [HttpGet]
        [Route("withproduct/{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<Dish>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishesByProduct(int productId)
        {
            if (productId < 0)
                return BadRequest();

            List<Dish> dishes = await dishesRepository.GetDishesByProduct(productId);

            if (dishes == null)
                return NotFound();

            return Ok(dishes);
        }

        [HttpGet]
        [Route("{dishId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Dish), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishById(int dishId)
        {
            if (dishId <= 0)
                return BadRequest();

            var dish = await dishesRepository.GetDishById(dishId);

            if (dish != null)
                return Ok(dish);

            return NotFound();
        }



        [Route("user/{userId:int}")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateDish(int userId, [FromBody]Dish dish)
        {
            var createdDish = await dishesRepository.CreateDish(userId, dish);

            if (createdDish == null)
                return Conflict();
            
            return Created("", createdDish);
        }


        //DELETE api/v1/[controller]/id
        [Route("user/{userId:int}/{dishId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteDish(int userId, int dishId)
        {
            if (dishId < 0)
                return BadRequest();

            if (!await dishesRepository.DeleteDish(userId, dishId))
                return NotFound();

            return NoContent();
        }

        [Route("user/{userId:int}")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateDish(int userId,[FromBody]Dish dishToUpdate)
        {
            var upd = await dishesRepository.UpdateDish(userId, dishToUpdate);

            if (upd == null)
                return NotFound();

            return Created("",upd);
        }
    }

    
}