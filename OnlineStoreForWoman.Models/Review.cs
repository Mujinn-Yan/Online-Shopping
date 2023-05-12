using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.Models
{
    public partial class Review
    {
        [Key]
        public int ReviewID { get; set; }
        
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Review1 { get; set; }
        public int Rate { get; set; }
        public DateTime DateTime { get; set; }
        public bool isDelete { get; set; }

        public virtual Product Product { get; set; }
    }
}
