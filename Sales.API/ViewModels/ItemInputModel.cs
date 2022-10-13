using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sales.API.ViewModels
{
    public class ItemInputModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Range(0.1, 7922800000000000000, ErrorMessage = "Price cannot be less than zero")]
        public decimal Price { get; set; }
    }
}