using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskV3.Core.Interfaces.Repositories;
using TaskV3.Core.Models;

namespace TaskV3.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly ApiContext _context;

        public CarRepository(ApiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> CreateCarAsync(Car car)
        {
            _context.Add(car);
            await _context.SaveChangesAsync();

            return car.Id;
        }

        public async Task<bool> CreateCarsAsync(IEnumerable<Car> cars)
        {
            _context.AddRange(cars);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            var car = await _context.Cars.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
            return car;
        }

        public async Task<Car> GetCarByMakeModelAndYearAsync(string make, string model, int year)
        {
            return await _context.Cars.AsNoTracking().SingleOrDefaultAsync(e => e.Make == make && e.Model == model && e.Year == year);
        }
    }
}
