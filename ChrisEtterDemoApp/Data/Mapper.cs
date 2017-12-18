using AutoMapper;
using ChrisEtterDemoApp.Data.EF.Entities;
using ChrisEtterDemoApp.ViewModels;

namespace ChrisEtterDemoApp.Data
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Order, OrderViewModel>()
                .ForMember(x => x.OrderId, y => y.MapFrom(x => x.Id))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap();

        }
    }
}
