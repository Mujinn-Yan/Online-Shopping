using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineStoreForWoman.DTOs;

namespace OnlineStoreForWoman.Data
{
    public interface ISearchRepository
    {
        Task<Response<ProductDto, SearchContextDto>> Search(Dictionary<string, string> queryParams);
        Task<HomePageDto> GetHomePage();
    }
}
