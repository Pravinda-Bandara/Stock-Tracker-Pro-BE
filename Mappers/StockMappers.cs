using AutoMapper;

namespace api.Mappers
{
    public class StockMappers : Profile
    {
        public StockMappers()
        {
            // Map Stock to StockDto and vice versa
            CreateMap<Models.Stock, Dtos.Stock.StockDto>().ReverseMap();
            CreateMap<Models.Stock, Dtos.Stock.CreateStockRequestDto>().ReverseMap();
        }
    }
}
