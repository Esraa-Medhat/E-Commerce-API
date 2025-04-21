using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _context;

        public DbInitializer(AppDbContext context)
        {
            _context = context;
        }
        public async Task InitializeAsync()
        {
            try
            {
                //Create Database If It doesnot Exists & Apply To Any pending Migration

                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }
                //Data Seeding
                //Seeding ProductTypes From Json Files
                if (!_context.ProductTypes.Any())
                {
                    //1-Read All Data From Types Json File As String
                    var TypesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");

                    //2-Transform String To C# Objects[List<ProductTypes>]
                    var types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                    //3-Add List<ProductTypes> To Database
                    if (types is not null && types.Any())
                    {
                        await _context.ProductTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }

                }

                //Seeding ProductBrands From Json Files
                if (!_context.ProductBrands.Any())
                {
                    //1-Read All Data From Brands Json File As String
                    var BrandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");

                    //2-Transform String To C# Objects[List<ProductBrands>]
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                    //3-Add List<ProductBrands> To Database
                    if (brands is not null && brands.Any())
                    {
                        await _context.ProductBrands.AddRangeAsync(brands);
                        await _context.SaveChangesAsync();
                    }

                }

                //Seeding Products From Json Files
                if (!_context.Products.Any())
                {
                    //1-Read All Data From Products Json File As String
                    var ProductData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                    //2-Transform String To C# Objects[List<Products>]
                    var products = JsonSerializer.Deserialize<List<Product>>(ProductData);

                    //3-Add List<Products> To Database
                    if (products is not null && products.Any())
                    {
                        await _context.Products.AddRangeAsync(products);
                        await _context.SaveChangesAsync();
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            }


        }
    }

