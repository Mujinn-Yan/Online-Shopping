using OnlineStoreForWoman.DataAccess.Repository.Interface;
using OnlineStoreForWoman.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private  ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context; 
        }

        public void Update(Product product)
        {
            var productDb = _context.Product.FirstOrDefault(x => x.Id == product.Id);
            if (productDb!=null)
            {
                productDb.Name = product.Name;
                productDb.ShortDescription = product.ShortDescription;
                productDb.UnitPrice = product.UnitPrice;
                //productDb.CategoryId = product.CategoryId;
                if (product.PicturePath!=null)
                {
                    productDb.PicturePath = product.PicturePath;
                }
            }
        }
    }
}
