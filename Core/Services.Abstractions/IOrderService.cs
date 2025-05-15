using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.OrdersModels;

namespace Services.Abstractions
{
    public interface IOrderService
    {
       Task<OrderResultDto> GetOrderByIdAsync(Guid id);
       Task<IEnumerable<OrderResultDto>> GetOrdersByUserEmailAsync(string userEmail);
        Task<OrderResultDto> CreateOrederAsync(OrderRequestDto orderRequestDto, string userEmail);
        Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods();
    }
}
