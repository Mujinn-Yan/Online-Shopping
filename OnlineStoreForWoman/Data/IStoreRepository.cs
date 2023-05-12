using System.Threading.Tasks;
using OnlineStoreForWoman.DTOs;
using OnlineStoreForWoman.Helpers;

namespace OnlineStoreForWoman.Data;

public interface IStoreRepository
{
    Task<Response<StoreOrderDto, OrderParams>> GetOrders(OrderParams orderParams, int userId);
    Task<StoreOrderDto> GetOrder(int userId, int orderId);
    Task<StoreOrderDto> StartDispatchingOrder(int userId, int orderId);
}