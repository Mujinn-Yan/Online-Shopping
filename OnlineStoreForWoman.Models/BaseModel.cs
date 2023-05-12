using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public String? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public String? UpdatedBy { get; set; }
    }
}
