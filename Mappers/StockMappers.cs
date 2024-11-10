using api.Dtos.Comment;
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
            CreateMap<Models.Stock, Dtos.Stock.UpdateStockRequestDto>().ReverseMap();
            CreateMap<Models.Stock, Dtos.Stock.FMPStock>().ReverseMap();

            //Comment
            CreateMap<Models.Comment, Dtos.Comment.CommentDto>().ReverseMap();
            CreateMap<Models.Comment, Dtos.Comment.CreateCommentDto>().ReverseMap();
            CreateMap<Models.Comment, Dtos.Comment.UpdateCommentRequestDto>().ReverseMap();

        }
    }
}
