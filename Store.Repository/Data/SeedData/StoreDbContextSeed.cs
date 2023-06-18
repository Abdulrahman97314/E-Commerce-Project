using Store.Core.Entities;
using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Data.SeedData
{
    public static class StoreDbContextSeed
    {
        public async static Task SeedAsync(StoreDbContext storeDbContext)
        {
				if (!storeDbContext.ProductTypes.Any())
				{
					var productTypesJson = await File.ReadAllTextAsync("../Store.Repository/Data/SeedData/JsonDataSeed/types.json");
					var productTypesData = JsonSerializer.Deserialize<List<ProductType>>(productTypesJson);
					if (productTypesData?.Count>0)
					{
                        await storeDbContext.ProductTypes.AddRangeAsync(productTypesData);
                        await storeDbContext.SaveChangesAsync();
                    }
				}
                
				if (!storeDbContext.DeliveryMethods.Any())
				{
					var delivertMethodsJson = await File.ReadAllTextAsync("../Store.Repository/Data/SeedData/JsonDataSeed/delivery.json");
					var delivertMethodsData = JsonSerializer.Deserialize<List<DeliveryMethod>>(delivertMethodsJson);
					if (delivertMethodsData?.Count>0)
					{
                        await storeDbContext.DeliveryMethods.AddRangeAsync(delivertMethodsData);
                        await storeDbContext.SaveChangesAsync();
                    }
				}
                if (!storeDbContext.ProductBrands.Any())
                {
                    var ProductBrandsJson = await File.ReadAllTextAsync("../Store.Repository/Data/SeedData/JsonDataSeed/brands.json");
                    var productBrandsData = JsonSerializer.Deserialize<List<ProductBrand>>(ProductBrandsJson);
                    if (productBrandsData?.Count > 0)
                    {
                        await storeDbContext.ProductBrands.AddRangeAsync(productBrandsData);
                        await storeDbContext.SaveChangesAsync();
                    }
                }
                if (!storeDbContext.Products.Any())
                {
                    var ProductProductsJson = await File.ReadAllTextAsync("../Store.Repository/Data/SeedData/JsonDataSeed/products.json");
                    var productProductsData = JsonSerializer.Deserialize<List<Product>>(ProductProductsJson);
                    if (productProductsData?.Count > 0)
                    {
                        await storeDbContext.Products.AddRangeAsync(productProductsData);
                        await storeDbContext.SaveChangesAsync();
                    }
                }
        }
    }
}
