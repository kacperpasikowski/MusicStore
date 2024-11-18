using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Helpers
{
	public static class SeedData
	{
		public static async Task InitializeAsync(IServiceProvider serviceProvider)
		{
			using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
			
			using var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
			using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			
			if (!await context.Users.AnyAsync())
			{
				await SeedRolesAsync(roleManager);
				await SeedUsersAsync(userManager);
			}
			
			if(await context.Products.AnyAsync())
			{
				return;
			}
			
			await SeedCategoriesAsync(context);
			await SeedProductTypesAsync(context);
			await SeedBrandsAsync(context);
			await SeedProductsAsync(context);
		}

		private static async Task SeedCategoriesAsync(ApplicationDbContext context)
		{
			var categories = new[]
			{
				new Category {Name = "Guitars", PhotoUrl="../assets/Categories/Category-Guitars.jpg"},
				new Category {Name = "Drums", PhotoUrl="../assets/Categories/Category-Drums.jpg"},
				new Category {Name = "Pianos And Keyboards", PhotoUrl="../assets/Categories/Category-Pianos-And-Keyboards.jpg"},
				new Category {Name = "Bass Guitars", PhotoUrl="../assets/Categories/Category-Bass-Guitars.jpg"}
			};

			context.Categories.AddRange(categories);
			await context.SaveChangesAsync();
		}



		private static async Task SeedProductTypesAsync(ApplicationDbContext context)
		{
			var categories = await context.Categories.ToListAsync();

			var productTypes = new[]
			{
				new ProductType {Name="Acoustic Guitars", CategoryId = categories.First(c =>c.Name == "Guitars").Id, PhotoUrl="../assets/ProductTypes/Type-Acoustic-Guitar.jpg"},
				new ProductType {Name="Electric Guitars", CategoryId = categories.First(c =>c.Name == "Guitars").Id, PhotoUrl="../assets/ProductTypes/Type-Electric-Guitar.jpg"},
				new ProductType {Name="Acoustic Bass", CategoryId = categories.First(c =>c.Name == "Bass Guitars").Id, PhotoUrl="../assets/ProductTypes/Type-Acoustic-Bass.jpg"},
				new ProductType {Name="Electric Bass", CategoryId = categories.First(c =>c.Name == "Bass Guitars").Id, PhotoUrl="../assets/ProductTypes/Type-Electric-Bass.jpg"},
				new ProductType {Name="Acoustic Drums", CategoryId = categories.First(c =>c.Name == "Drums").Id, PhotoUrl="../assets/ProductTypes/Type-Acoustic-Drums.jpg"},
				new ProductType {Name="Electric Drums", CategoryId = categories.First(c =>c.Name == "Drums").Id, PhotoUrl="../assets/ProductTypes/Type-Electric-Drums.jpg"},
				new ProductType {Name="Grand Pianos", CategoryId = categories.First(c =>c.Name == "Pianos And Keyboards").Id, PhotoUrl="../assets/ProductTypes/Type-Grand-Piano.jpg"},
				new ProductType {Name="Keyboards", CategoryId = categories.First(c =>c.Name == "Pianos And Keyboards").Id, PhotoUrl="../assets/ProductTypes/Type-Keyboard-Piano.jpg"},

			};

			context.ProductTypes.AddRange(productTypes);
			await context.SaveChangesAsync();
		}

		private static async Task SeedBrandsAsync(ApplicationDbContext context)
		{
			var brands = new[]
			{
				new Brand { Name = "Yamaha", PhotoUrl="../assets/Brands/Brand-Yamaha.jpg" },
				new Brand { Name = "Fender", PhotoUrl="../assets/Brands/Brand-Fender.jpg" },
				new Brand { Name = "Gibson", PhotoUrl="../assets/Brands/Brand-Gibson.jpg"},
				new Brand { Name = "Jackson", PhotoUrl="../assets/Brands/Brand-Jackson.jpg" },
				new Brand { Name = "Tama", PhotoUrl="../assets/Brands/Brand-Tama.jpg" },
				new Brand { Name = "Pearl", PhotoUrl="../assets/Brands/Brand-Pearl.jpg" },
			};

			context.Brands.AddRange(brands);
			await context.SaveChangesAsync();
		}
		
		private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			string[] roleNames = {"User", "Admin", "Moderator"};
			
			foreach(var role in roleNames)
			{
				if(!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}
		}
		
		private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
		{
			
			
			var users = new[]
			{
				new  {UserName = "Admin", FirstName = "Admin", LastName = "Admin", Email= "admin@123", Password = "Admin@123", Role = "Admin"},
				new  {UserName = "Pudzian", FirstName = "Marcin", LastName = "Pudzianowski", Email= "pudzian@123", Password = "Pudzian@123", Role = "User"},
				new  {UserName = "Malysz", FirstName = "Adam", LastName = "Malysz", Email= "malysz@123", Password = "Malysz@123" , Role = "User" },
				new  {UserName = "Kubica", FirstName = "Robert", LastName = "Kubica", Email= "kubica@123", Password = "Kubica@123" , Role = "Moderator"},
			};
			
			
			foreach (var userInfo in users)
			{
				if(await userManager.FindByEmailAsync(userInfo.Email) == null)
				{
					var user = new AppUser {UserName = userInfo.UserName, FirstName = userInfo.FirstName, LastName = userInfo.LastName, Email = userInfo.Email};
					var result = await userManager.CreateAsync(user, userInfo.Password);
					
					if(result.Succeeded)
					{
						await userManager.AddToRoleAsync(user, userInfo.Role);
					}
				}
			}
		}

		private static async Task SeedProductsAsync(ApplicationDbContext context)
		{
			var brands = await context.Brands.ToListAsync();
			var productTypes = await context.ProductTypes.ToListAsync();

			var products = new[]
			{
		// **Acoustic Guitars**
		new Product
		{
			Name = "Fender FA-25",
			Description = "Solid-top acoustic guitar with authentic sound.",
			Price = 199.99M,
			QuantityInStoct = 10,
			ProductTypeId = productTypes.First(pt => pt.Name == "Acoustic Guitars").Id,
			BrandId = brands.First(b => b.Name == "Fender").Id,
			PhotoUrl = "../assets/Products/Acoustic-Guitar-FenderFA25.jpg"
		},
		new Product
		{
			Name = "Gibson CD-60S",
			Description = "Affordable dreadnought acoustic guitar with great tone.",
			Price = 229.99M,
			QuantityInStoct = 8,
			ProductTypeId = productTypes.First(pt => pt.Name == "Acoustic Guitars").Id,
			BrandId = brands.First(b => b.Name == "Gibson").Id,
			PhotoUrl = "../assets/Products/Acoustic-Guitar-GibsonJ45.jpg"
		},
		// **Electric Guitars**
		new Product
		{
			Name = "Fender Stratocaster",
			Description = "Classic electric guitar known for its bright sound.",
			Price = 699.99M,
			QuantityInStoct = 5,
			ProductTypeId = productTypes.First(pt => pt.Name == "Electric Guitars").Id,
			BrandId = brands.First(b => b.Name == "Fender").Id,
			PhotoUrl = "../assets/Products/Electric-Guitar-FenderStratocaster.jpg"
		},
		new Product
		{
			Name = "Gibson Les Paul Standard",
			Description = "Iconic electric guitar with rich, warm tones.",
			Price = 2499.99M,
			QuantityInStoct = 3,
			ProductTypeId = productTypes.First(pt => pt.Name == "Electric Guitars").Id,
			BrandId = brands.First(b => b.Name == "Gibson").Id,
			PhotoUrl = "../assets/Products/Electric-Guitar-GibsonLP.jpg"
		},
		new Product
		{
			Name = "Jackson JS22 Dinky",
			Description = "Affordable electric guitar with fast neck and high-output pickups.",
			Price = 199.99M,
			QuantityInStoct = 7,
			ProductTypeId = productTypes.First(pt => pt.Name == "Electric Guitars").Id,
			BrandId = brands.First(b => b.Name == "Jackson").Id,
			PhotoUrl = "../assets/Products/Electric-Guitar-JacksonPDX2.jpg"
		},
		new Product
		{
			Name = "Yamaha Standard Plus",
			Description = "Affordable electric guitar with fast neck and high-output pickups.",
			Price = 599.99M,
			QuantityInStoct = 10,
			ProductTypeId = productTypes.First(pt => pt.Name == "Electric Guitars").Id,
			BrandId = brands.First(b => b.Name == "Yamaha").Id,
			PhotoUrl = "../assets/Products/Electric-Guitar-Yamaha-1.jpg"
		},
		// **Acoustic Bass**
		new Product
		{
			Name = "Yamaha Bass",
			Description = "Versatile and affordable acoustic bass guitar.",
			Price = 249.99M,
			QuantityInStoct = 6,
			ProductTypeId = productTypes.First(pt => pt.Name == "Acoustic Bass").Id,
			BrandId = brands.First(b => b.Name == "Yamaha").Id,
			PhotoUrl = "../assets/Products/Acoustic-Bass-Yamaha1.jpg"
		},
		// **Electric Bass**
		new Product
		{
			Name = "Yamaha Precision Bass",
			Description = "Industry standard for bass guitars.",
			Price = 799.99M,
			QuantityInStoct = 5,
			ProductTypeId = productTypes.First(pt => pt.Name == "Electric Bass").Id,
			BrandId = brands.First(b => b.Name == "Yamaha").Id,
			PhotoUrl = "../assets/Products/Electric-Bass-Yamaha1.jpg"
		},
		new Product
		{
			Name = "Gibson Bass Super",
			Description = "Versatile electric bass with punchy tone.",
			Price = 249.99M,
			QuantityInStoct = 7,
			ProductTypeId = productTypes.First(pt => pt.Name == "Electric Bass").Id,
			BrandId = brands.First(b => b.Name == "Gibson").Id,
			PhotoUrl = "../assets/Products/Electric-Bass-Gibson1.jpg"
		},
		// **Acoustic Drums**
		new Product
		{
			Name = "Pearl Export EXX Drum Set",
			Description = "Complete 5-piece drum set with cymbals.",
			Price = 699.99M,
			QuantityInStoct = 3,
			ProductTypeId = productTypes.First(pt => pt.Name == "Acoustic Drums").Id,
			BrandId = brands.First(b => b.Name == "Pearl").Id,
			PhotoUrl = "../assets/Products/Acoustic-Drums-Pearl1.jpg"
		},
		new Product
		{
			Name = "Tama Imperialstar Drum Set",
			Description = "High-quality acoustic drum set suitable for beginners and pros.",
			Price = 799.99M,
			QuantityInStoct = 2,
			ProductTypeId = productTypes.First(pt => pt.Name == "Acoustic Drums").Id,
			BrandId = brands.First(b => b.Name == "Tama").Id,
			PhotoUrl = "../assets/Products/Acoustic-Drums-TamaStar1.jpg"
		},
		// **Electric Drums**
		new Product
		{
			Name = "Pearl DTX402K Electronic Drum Kit",
			Description = "Compact electronic drum kit for home practice.",
			Price = 499.99M,
			QuantityInStoct = 4,
			ProductTypeId = productTypes.First(pt => pt.Name == "Electric Drums").Id,
			BrandId = brands.First(b => b.Name == "Pearl").Id,
			PhotoUrl = "../assets/Products/Electric-Drums-Pearl1.jpg"
		},
		new Product
		{
			Name = "Pearl e/Merge Electronic Drum Set",
			Description = "Advanced electronic drum kit with realistic feel.",
			Price = 3499.99M,
			QuantityInStoct = 1,
			ProductTypeId = productTypes.First(pt => pt.Name == "Electric Drums").Id,
			BrandId = brands.First(b => b.Name == "Pearl").Id,
			PhotoUrl = "../assets/Products/Electric-Drums-Pearl2.jpg"
		},
		// **Grand Pianos**
		new Product
		{
			Name = "Yamaha CFX Concert Grand Piano",
			Description = "Premium concert grand piano with exceptional sound.",
			Price = 179999.99M,
			QuantityInStoct = 1,
			ProductTypeId = productTypes.First(pt => pt.Name == "Grand Pianos").Id,
			BrandId = brands.First(b => b.Name == "Yamaha").Id,
			PhotoUrl = "../assets/Products/Grand-Piano-Yamaha1.jpg"
		},
		new Product
		{
			Name = "Yamaha CFX Concert Grand Piano",
			Description = "Premium concert grand piano with exceptional sound.",
			Price = 200000.99M,
			QuantityInStoct = 1,
			ProductTypeId = productTypes.First(pt => pt.Name == "Grand Pianos").Id,
			BrandId = brands.First(b => b.Name == "Yamaha").Id,
			PhotoUrl = "../assets/Products/Grand-Piano-Yamaha2.jpg"
		},
		// **Keyboards**
		new Product
		{
			Name = "Yamaha PSR-E373 Keyboard",
			Description = "Portable keyboard with wide range of features.",
			Price = 199.99M,
			QuantityInStoct = 10,
			ProductTypeId = productTypes.First(pt => pt.Name == "Keyboards").Id,
			BrandId = brands.First(b => b.Name == "Yamaha").Id,
			PhotoUrl = "../assets/Products/Keyboard-Yamaha1.jpg"
		},
		new Product
		{
			Name = "Yamaha DGX-660 Portable Grand",
			Description = "88-key portable grand piano with authentic piano feel.",
			Price = 799.99M,
			QuantityInStoct = 5,
			ProductTypeId = productTypes.First(pt => pt.Name == "Keyboards").Id,
			BrandId = brands.First(b => b.Name == "Yamaha").Id,
			PhotoUrl = "../assets/Products/Keyboard-Yamaha2.jpg"
		}
	};

			context.Products.AddRange(products);
			await context.SaveChangesAsync();
		}
	}
}