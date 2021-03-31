using AutoMapper;
using TaskV3.Core.Dtos;
using TaskV3.Core.Models;

namespace TaskV3.Core.Config
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CarDto, Car>();
            CreateMap<Stock, StockDto>()
                .ForPath(d => d.Make, opt => opt.MapFrom(s => s.Car.Make))
                .ForPath(d => d.Model, opt => opt.MapFrom(s => s.Car.Model))
                .ForPath(d => d.Year, opt => opt.MapFrom(s => s.Car.Year));
               
        }
    }
}
