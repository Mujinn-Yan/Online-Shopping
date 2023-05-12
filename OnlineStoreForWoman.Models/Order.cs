using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.Models
{
    public partial class Order : BaseModel
    {
        
        public string CustomerID { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }

        public int ShippingID { get; set; }
        public int Discount { get; set; }
        public int Taxes { get; set; }
        public int TotalAmount { get; set; }
        public bool isCompleted { get; set; }
        public DateTime OrderDate { get; set; }
        public bool DIspatched { get; set; }
        public DateTime DispatchedDate { get; set; }
        public bool Shipped { get; set; }
        public DateTime ShippingDate { get; set; }
        public bool Deliver { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Notes { get; set; }
        public bool CancelOrder { get; set; }

        //public virtual Customer Customer { get; set; }
        public virtual ShippingDetail ShippingDetail { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
