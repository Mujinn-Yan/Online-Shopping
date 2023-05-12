using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreForWoman.Models
{
    public class CartModelView
    {
        public List<Cart> CartData { get; set; }
        public int WishList { get; set; }
        public double TotalAmount { get; set; }
        [NotMapped]
        public string CustomerId { get; set; }
    }
}
