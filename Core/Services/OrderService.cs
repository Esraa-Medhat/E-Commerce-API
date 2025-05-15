using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Order;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Specifications;
using Shared.OrdersModels;

namespace Services
{
    public class OrderService(IMapper mapper,IBasketRepository basketRepository,IUnitOfWork unitOfWork) : IOrderService
    {
        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecification(id);
            var order = await unitOfWork.GetRepository<Order, Guid>().GetAsync(spec);
            if (order is null) throw new OrderNotFoundExceptions(id);
            var result = mapper.Map<OrderResultDto>(order);
            return result;
        }

        public async Task<IEnumerable<OrderResultDto>> GetOrdersByUserEmailAsync(string userEmail)
        {
            var spec = new OrderSpecification(userEmail);
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            var result = mapper.Map<IEnumerable<OrderResultDto>>(orders);
            return result;

        }
        public async Task<OrderResultDto> CreateOrederAsync(OrderRequestDto orderRequest, string userEmail)
        {
            var address = mapper.Map<Address>(orderRequest.ShipToAddress);
            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);
            if (basket is null) throw new BasketNotFoundExceptions(orderRequest.BasketId);

            var orderItems = new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>().GetAsync(item.Id);
                if (product is null) throw new ProductNotFoundExceptions(item.Id);
                var orderItem = new OrderItem(new ProductInOrderItem(product.Id,product.Name,product.PictureUrl),item.Quantity,product.Price);
                orderItems.Add(orderItem);
            }
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequest.DeliveryMethodId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundExceptions(orderRequest.DeliveryMethodId);

            var subTotal = orderItems.Sum(i => i.Price * i.Quantity);
            var order = new Order(userEmail, address, orderItems, deliveryMethod, subTotal,"");
            await unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            var count= await unitOfWork.SaveChangesAsync();
            if (count == 0) throw new OrderCreateBadRequest();
           var result= mapper.Map<OrderResultDto>(order);
            return result;
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<DeliveryMethodDto>>(deliveryMethods);
            return result;
        }

   
    }
}
