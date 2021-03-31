using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskV3.Core.Interfaces.Business;
using TaskV3.Core.Interfaces.Repositories;
using TaskV3.Core.Models;

namespace TaskV3.Business
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IStockRepository _stockRepository;

        public CarService(ICarRepository carRepository, IStockRepository stockRepository)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _stockRepository = stockRepository ?? throw new ArgumentNullException(nameof(stockRepository));
        }

        public async Task<int> AddCarAsync(Car car)
        {
            //check if car exists.
            var existingCar = await _carRepository.GetCarByMakeModelAndYearAsync(car.Make, car.Model, car.Year);
            if (existingCar == null)
            {
                var carId = await _carRepository.CreateCarAsync(car);
                return carId;
            }
            return 0;
        }

        public async Task<bool> RemoveCarAsync(int carId, int dealerId)
        {
            //check if car exists.
            var car = await _carRepository.GetCarByIdAsync(carId);
            if(car == null)
            {
                return true;
            }

            //delete the stock from the dealer if car exists
            var stock = await _stockRepository.GetStockAsync(carId, dealerId);
            if(stock == null)
                return true;
            return await _stockRepository.DeleteStockAsync(stock);
        }

        public async Task<bool> ListCarsAsync(int carId, int amount, int dealerId)
        {
            //check if car exists
            var car = await _carRepository.GetCarByIdAsync(carId);
            if(car == null)
            {
                throw new KeyNotFoundException();
            }

            //get stock for the dealer and update accordingly
            var stock = await _stockRepository.GetStockAsync(car.Id, dealerId);
            if (stock == null)
            {
                stock = new Stock
                {
                    CarId = carId,
                    Amount = amount,
                    DealerId = dealerId
                };
                return await _stockRepository.CreateStockAsync(stock);
            }
            else
            {
                stock.Amount += amount;
                return await _stockRepository.UpdateStockAsync(stock);
            }
        }

        public async Task<IEnumerable<Stock>> SearchCarStocksAsync(string make, string model, int dealerId)
        {
            return await _stockRepository.GetStocksByMakeAndModelAsync(make, model, dealerId);
        }

        public async Task<bool> UpdateCarStockAsync(int carId, int amount, int dealerId)
        {
            //check if car exists.
            var car = await _carRepository.GetCarByIdAsync(carId);
            if(car == null)
            {
                throw new KeyNotFoundException();
            }

            //get stock for the dealer and update accordingly
            var stock = await _stockRepository.GetStockAsync(car.Id, dealerId);
            if(stock == null)
            {
                stock = new Stock
                {
                    CarId = carId,
                    DealerId = dealerId,
                    Amount = amount
                };
            }
            else
            {
                stock.Amount = amount;
            }

            return await _stockRepository.UpdateStockAsync(stock);
        }
    }
}
