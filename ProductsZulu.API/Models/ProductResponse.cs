using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductsZulu.API.Models
{
    public class ProductResponse
    {
        public int ProductId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsActtive { get; set; }

        public DateTime LastPurchase { get; set; }

        public string Image { get; set; } //ruta de la imagen

        public double Stock { get; set; }

        public string Remarks { get; set; }

    }
}