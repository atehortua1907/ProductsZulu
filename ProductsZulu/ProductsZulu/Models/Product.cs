﻿namespace ProductsZulu
{
    using System;

    public class Product
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public bool IsActtive { get; set; }
        public DateTime LastPurchase { get; set; }
        public double Stock { get; set; }
        public string Remarks { get; set; }

        public string  ImageFullPath
        {
            get
            {
                return string.Format("http://productszulu.azurewebsites.net/{0}",
                    Image.Substring(1)); // retorna la imagen concatenando la url y eliminando la ~
            }
        }

    }
}
