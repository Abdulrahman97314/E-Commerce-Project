using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        [DataType("decimal(18,2)")]
        public decimal Price { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public List<ProductRating> ProductRating { get; set; }
    }
}
