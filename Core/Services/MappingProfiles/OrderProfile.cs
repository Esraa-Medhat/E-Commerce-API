using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Order;
using Shared.OrdersModels;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderResultDto>()
            .ForMember(d => d.orderPaymentStatus, o => o.MapFrom(s => s.orderPaymentStatus.ToString()))
            .ForMember(d => d.deliveryMethod, o => o.MapFrom(s => s.deliveryMethod.ShortName))
            .ForMember(d => d.Total, o => o.MapFrom(s => s.SubTotal + s.deliveryMethod.Cost));

            CreateMap<DeliveryMethod, DeliveryMethodDto>();

            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.productInOrderItem.ProductId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.productInOrderItem.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.productInOrderItem.PictureUrl));

            CreateMap<Address, AddressDto>().ReverseMap();
                
        }

    }
}

