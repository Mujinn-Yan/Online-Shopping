using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineStoreForWoman.DTOs;

namespace OnlineStoreForWoman.Data;

public interface IProductRepository
{
    Task<ProductModelDto> GetProduct(int productId,int userId);
}