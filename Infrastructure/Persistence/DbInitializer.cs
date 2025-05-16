using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(AppDbContext context,AppIdentityDbContext identityDbContext,UserManager<AppUser> userManager 
          ,RoleManager<IdentityRole> roleManager )
        {
            _context = context;
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
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
                if (!_context.DeliveryMethods.Any())
                {
                    //1-Read All Data From delivery Json File As String
                    var deliveryData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\delivery.json");

                    //2-Transform String To C# Objects[List<DeliveryMethod>]
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                    //3-Add List<DeliveryMethod> To Database
                    if (deliveryMethods is not null && deliveryMethods.Any())
                    {
                        await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                        await _context.SaveChangesAsync();
                    }

                }
            
    
        }
            catch (Exception)
            {
                throw;
            }
            }

        public async Task InitializeIdentityAsync()
        {
            if (_identityDbContext.Database.GetPendingMigrations().Any())
            {
              await _identityDbContext.Database.MigrateAsync();
            }

            if (!_roleManager.Roles.Any())
            {

                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin"
                });
                 await _roleManager.CreateAsync(new IdentityRole()
                 {
                     Name = "Admin"
                 }
        
                );
            }
            if (!_userManager.Users.Any())
            {
                var superAdminUser = new AppUser()
                {
                    DisplayName = "Super Admin",
                    Email="SuperAdmin@gmail.com",
                    UserName="SuperAdmin",
                    PhoneNumber="0123456789"

                };
                var adminUser = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "0123456789"

                };
              await  _userManager.CreateAsync(superAdminUser, "P@ssW0rd");
                await _userManager.CreateAsync(adminUser, "P@ssW0rd");
              await  _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
              await  _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
    }

