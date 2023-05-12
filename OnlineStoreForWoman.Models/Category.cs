using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.Models
{
    public partial class Category:BaseModel
    { 
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string PicturePath { get; set; }
        [NotMapped]
        [Display(Name = "Choose the Category image")]
        [Required]
        public IFormFile Picture { get; set; }

        
    }
}
