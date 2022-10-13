using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.API.ViewModels
{
    public class CustomerInputModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }
        [Required(ErrorMessage = "Identity is required")]
        public string Identity { get; set; }
    }
}