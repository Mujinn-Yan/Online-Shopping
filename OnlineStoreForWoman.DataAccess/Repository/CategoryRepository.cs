using OnlineStoreForWoman.DataAccess.Repository.Interface;
using OnlineStoreForWoman.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var categoryDb = _context.Category.FirstOrDefault(x => x.Id == category.Id);
            if (categoryDb!=null)
            {
                categoryDb.Name = category.Name;
                category.Description = category.Description;
            }
        }
    }
}
