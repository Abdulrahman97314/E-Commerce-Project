using Store.APIs.Helpers;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications
{
    public class ProductWithFilterationForCountSpecification:BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecPrams productSpecPrams) 
            : base(p => (string.IsNullOrEmpty(productSpecPrams.Search) || p.Name.ToLower().Contains(productSpecPrams.Search.ToLower())) &&
                      (!productSpecPrams.BrandId.HasValue || p.ProductBrandId == productSpecPrams.BrandId.Value) &&
                      (!productSpecPrams.TypeId.HasValue || p.ProductTypeId == productSpecPrams.TypeId.Value))
        {
            
        }
    }
}
