using Store.APIs.Helpers;
using Store.Core.Entities;

namespace Store.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification:BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecification(ProductSpecPrams productSpecPrams)
            :base(p=> (string.IsNullOrEmpty(productSpecPrams.Search)|| p.Name.ToLower().Contains(productSpecPrams.Search.ToLower()))&&
                      (!productSpecPrams.BrandId.HasValue||p.ProductBrandId== productSpecPrams.BrandId.Value) &&
                      (!productSpecPrams.TypeId.HasValue || p.ProductTypeId == productSpecPrams.TypeId.Value))
        {
            AddIncludes(p => p.ProductType);
            AddIncludes(p => p.ProductBrand);
            AddIncludes(P=>P.ProductRating);
            if (!string.IsNullOrEmpty(productSpecPrams.Sort))
            {
                switch (productSpecPrams.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p=> p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    case "rating":
                        AddOrderByDesc(p => p.ProductRating.Average(r=>r.Rating));
                        break;
                    default:
                        AddOrderBy(p => p.Id);
                        break;
                }
            }
            ApplyPagination((productSpecPrams.PageIndex-1) * productSpecPrams.PageSize, productSpecPrams.PageSize);


        }
        public ProductWithBrandAndTypeSpecification(int id):base(p=>p.Id==id)
        {
            AddIncludes(p => p.ProductType);
            AddIncludes(p => p.ProductBrand);
            AddIncludes(P => P.ProductRating);
        }
    }
}
