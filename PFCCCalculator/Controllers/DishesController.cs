using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PFCCCalculatorService.Models;
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
 /*       [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<DishModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishes()
        {
            var dishes = await dishesService.GetDishes();

            if (dishes == null)
                return NotFound();

            return Ok(dishes);
        } */

/*        [HttpGet]
        [Route("{dishId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DishModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDishById(int dishId)
        {
            if (dishId <= 0)
                return BadRequest();

            try
            {

                var dish = await dishesService.GetDishById(dishId);

                if (dish != null)
                    return Ok(JsonConvert.SerializeObject(dish));
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }

            return NotFound();
        } */
      
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [Route("user/{userId}/")]
        public async Task<IActionResult> CreateDish(int UserId, DishModel dish)
        {
            try
            {
                if (dish == null)
                {
                    ModelState.AddModelError("", "Рецепт не задан");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var created = await dishesService.CreateDish(UserId, dish);
                if (created != null)
                    return Created("", created);
                return Conflict("Не удалось создать");

            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }


        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [Route("user/{userId}/")]
        public async Task<IActionResult> UpdateDish(int UserId, DishModel dish)
        {
            try
            {
                var created = await dishesService.UpdateDish(UserId, dish);
                if (created != null)
                    return Created("", created);
                return Conflict("Не удалось обновить");

            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        } 

    }
}