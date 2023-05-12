using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.Models
{
    public class Product : BaseModel
    {

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Name is required.")]

        public string Name { get; set; }
        [Display(Name = "Category")]
        
        public int CategoryID { get; set; }
        public  Category Category { get; set; }
        [Display(Name = "Unit Price")]
        
        public decimal UnitPrice { get; set; }
        [Display(Name = "Previous Price")]
        [Required(ErrorMessage = "Old Price is required.")]
        public decimal OldPrice { get; set; }
        [Required(ErrorMessage = "Discount is required.")]
        public decimal Discount { get; set; }
        [Display(Name = "Stock")]
        [Required(ErrorMessage = "Stock Quantity is required.")]
        public int UnitInStock { get; set; }
        [Display(Name = "Available?")]
        public bool ProductAvailable { get; set; } = true;
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required.")]

        public string ShortDescription { get; set; }
        [Display(Name = "Picture")]
        public string? PicturePath { get; set; }
        [Required(ErrorMessage = "Note is required.")]

        public string Note { get; set; }
        [NotMapped]
        [Display(Name = "Choose the Category image")]
        [Required(ErrorMessage = "Picture is required.")]
        public IFormFile Picture { get; set; }

    }
}
