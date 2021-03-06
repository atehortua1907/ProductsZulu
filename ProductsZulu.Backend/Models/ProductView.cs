﻿namespace ProductsZulu.Backend.Models
{
    using ProductsZuluDomain;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    [NotMapped]
    public class ProductView : Product
    {   
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}