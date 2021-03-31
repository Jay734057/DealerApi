using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskV3.Business;
using TaskV3.Core.Interfaces.Business;
using TaskV3.Core.Interfaces.Repositories;
using TaskV3.Core.Models;
using Xunit;

namespace TaskV3.Test
{
    public class CarServiceTests
    {
        private readonly ICarService _carService;

        public CarServiceTests()
        {
            var carRepository = Substitute.For<ICarRepository>();
            carRepository.GetCarByMakeModelAndYearAsync("Audi", "A4", 2020).Returns(Task.FromResult(new Car()));
            carRepository.GetCarByMakeModelAndYearAsync("Audi", "Q7", 2019).Returns(Task.FromResult(new Car()));
            carRepository.CreateCarAsync(new Car()).ReturnsForAnyArgs(Task.FromResult(1));
            carRepository.GetCarByIdAsync(1).Returns(Task.FromResult(new Car()));
            carRepository.GetCarByIdAsync(2).Returns(Task.FromResult(new Car()));


            var stockRepository = Substitute.For<IStockRepository>();
            stockRepository.CreateStockAsync(new Stock()).ReturnsForAnyArgs(Task.FromResult(true));
            stockRepository.UpdateStockAsync(new Stock()).ReturnsForAnyArgs(Task.FromResult(true));
            stockRepository.DeleteStockAsync(new Stock()).ReturnsForAnyArgs(Task.FromResult(true));
            stockRepository.GetStockAsync(1, 1).ReturnsForAnyArgs(Task.FromResult(new Stock()));
            stockRepository.GetStockAsync(3, 2).ReturnsForAnyArgs(Task.FromResult(new Stock()));
            stockRepository.GetStocksByMakeAndModelAsync("Audi", "A4", 1).Returns(Task.FromResult((IEnumerable<Stock>)new List<Stock> { new Stock { Amount = 3 } }));
            stockRepository.GetStocksByMakeAndModelAsync("Audi", "Q7", 1).Returns(Task.FromResult((IEnumerable<Stock>)new List<Stock> { new Stock { Amount = 3 }, new Stock { Amount = 2 } }));

            _carService = new CarService(carRepository, stockRepository);
        }


        [Theory]
        [InlineData("Audi", "A4", 2020, 0)]
        [InlineData("Audi", "A4", 2021, 1)]
        [InlineData("Audi", "Q7", 2019, 0)]
        [InlineData("Audi", "Q7", 2018, 1)]
        public async Task AddCarTestAsync(string make, string model, short year, int expected)
        {
            var car = new Car { Make = make, Model = model, Year = year };
            var carId = await _carService.AddCarAsync(car);
            Assert.Equal(expected, carId);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(2, 1, true)]
        [InlineData(3, 1, true)]
        [InlineData(2, 2, true)]
        public async Task RemoveCarTestAsync(int carId, int dealerId, bool expected)
        {
            var result = await _carService.RemoveCarAsync(carId, dealerId);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 3, 1, false, true)]
        [InlineData(2, 5, 1, false, true)]
        [InlineData(3, 10, 1, true, true)]
        [InlineData(3, 20, 2, true, true)]
        public async Task ListCarsTestAsync(int carId, int amount, int dealerId, bool expectedException, bool expected)
        {
            if (expectedException)
            {
                await Assert.ThrowsAsync<KeyNotFoundException>(
                                () => _carService.ListCarsAsync(carId, amount, dealerId)
                            );
            }
            else
            {
                var result = await _carService.ListCarsAsync(carId, amount, dealerId);
                Assert.Equal(expected, result);
            }
        }

        [Theory]
        [InlineData(1, 3, 1, false, true)]
        [InlineData(2, 5, 1, false, true)]
        [InlineData(3, 10, 1, true, true)]
        [InlineData(3, 20, 2, true, true)]
        public async Task UpdateCarStockTestAsync(int carId, int amount, int dealerId, bool expectedException, bool expected)
        {
            if (expectedException)
            {
                await Assert.ThrowsAsync<KeyNotFoundException>(
                                () => _carService.ListCarsAsync(carId, amount, dealerId)
                            );
            }
            else
            {
                var result = await _carService.UpdateCarStockAsync(carId, amount, dealerId);
                Assert.Equal(expected, result);
            }
        }

        [Theory]
        [InlineData("Audi", "A4", 1, 3)]
        [InlineData("Audi", "Q7", 1, 5)]
        public async Task SearchCarsTestAsync(string make, string model, int dealerId, int expected)
        {
            var result = await _carService.SearchCarStocksAsync(make, model, dealerId);
            Assert.Equal(expected, result.Sum(r => r.Amount));
        }
    }
}
