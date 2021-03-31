using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskV3.Core.Dtos;
using TaskV3.Core.Interfaces.Business;
using TaskV3.Core.Models;

namespace TaskV3.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IDealerContext _dealerContext;
        private readonly IMapper _mapper;

        public CarController(ICarService carService, IDealerContext dealerContext, IMapper mapper)
        {
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _dealerContext = dealerContext ?? throw new ArgumentNullException(nameof(dealerContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Add new car into the DB.
        /// </summary>
        /// <returns>Car info</returns>
        /// <param name="carDto">Car info</param>
        [HttpPost("Add")]
        public async Task<IActionResult> AddCarAsync(CarDto carDto)
        {
            try
            {
                var car = _mapper.Map<Car>(carDto);
                var carId = await _carService.AddCarAsync(car);
                if (carId == 0)
                    return BadRequest($"Car with the combination(Make: {carDto.Make}, Model: {carDto.Model}, Year: {carDto.Year}) already exists in DB.");
                return Ok(car);
            }
            catch (Exception ex)
            {
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex);
                return result;
            }
        }

        /// <summary>
        /// Remove existing car stock from the DB.
        /// </summary>
        /// <returns>Ok.</returns>
        /// <param name="carId">Car id</param>
        [HttpDelete("Remove")]
        public async Task<IActionResult> RemoveCarAsync(int carId)
        {
            try
            {
                await _carService.RemoveCarAsync(carId, _dealerContext.DealerId);
                return Ok();
            }
            catch (Exception ex)
            {
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex);
                return result;
            }
        }

        /// <summary>
        /// Update car stock in the DB.
        /// </summary>
        /// <returns>Ok.</returns>
        /// <param name="carId">Car id</param>
        /// <param name="amount">Car stock amount</param>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateStockAsync(int carId, int amount)
        {
            try
            {
                await _carService.UpdateCarStockAsync(carId, amount, _dealerContext.DealerId);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return BadRequest($"Car with Id {carId} doesn't not exist in DB.");
            }
            catch (Exception ex)
            {
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex);
                return result;
            }

        }

        /// <summary>
        /// List car stock in the DB.
        /// </summary>
        /// <returns>Ok.</returns>
        /// <param name="carId">Car id</param>
        /// <param name="amount">Car stock amount</param>
        [HttpPost("List")]
        public async Task<IActionResult> ListCarsAsync(int carId, int amount)
        {
            try
            {
                await _carService.ListCarsAsync(carId, amount, _dealerContext.DealerId);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return BadRequest($"Car with Id {carId} doesn't not exist in DB.");
            }
            catch (Exception ex)
            {
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex);
                return result;
            }
        }

        /// <summary>
        /// Search car stock in the DB.
        /// </summary>
        /// <returns>List of cars with stocks</returns>
        /// <param name="make">Car make</param>
        /// <param name="model">Car model</param>
        [HttpGet("Search")]
        public async Task<IActionResult> SearchAsync(string make, string model)
        {
            try
            {
                var stocks = await _carService.SearchCarStocksAsync(make, model, _dealerContext.DealerId);
                return Ok(_mapper.Map<IEnumerable<StockDto>>(stocks));
            }
            catch (Exception ex)
            {
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex);
                return result;
            }
        }
    }
}
