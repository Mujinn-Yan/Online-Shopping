using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.Models
{
    public partial class Wishlist
    {
        [Key]
        public int WishlistID { get; set; }
        public string CustomerID { get; set; }

        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
