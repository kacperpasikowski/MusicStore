using System.Net.Security;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Config;
using Project.Models;

namespace Project.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}
	
	public DbSet<Product> Products { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<ProductType> ProductTypes { get; set; }
	public DbSet<ShoppingCart> ShoppingCarts { get; set; }
	public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
	public DbSet<Brand> Brands { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
		
		base.OnModelCreating(builder);
		
		//Category ProductType 1 - N 
		builder.Entity<ProductType>()
			.HasOne(pt => pt.Category)
			.WithMany(c => c.ProductTypes)
			.HasForeignKey(pt => pt.CategoryId)
			.OnDelete(DeleteBehavior.Cascade);
			
		builder.Entity<Product>()
			.HasOne(p => p.ProductType)
			.WithMany(pt=> pt.Products)
			.HasForeignKey(p => p.ProductTypeId)
			.OnDelete(DeleteBehavior.Cascade);
			
		builder.Entity<Product>()
			.HasOne(p => p.Brand)
			.WithMany(b => b.Products)
			.HasForeignKey(p => p.BrandId)
			.OnDelete(DeleteBehavior.Cascade);
			
		builder.Entity<AppUser>()
			.HasOne(u=> u.ShoppingCart)
			.WithOne(sc => sc.AppUser)
			.HasForeignKey<ShoppingCart>(sc => sc.AppUserId)
			.OnDelete(DeleteBehavior.Cascade);
			
		builder.Entity<ShoppingCartItem>()
			.HasOne(sci => sci.ShoppingCart)
			.WithMany(sc => sc.ShoppingCartItems)
			.HasForeignKey(sci => sci.ShoppingCartId)
			.OnDelete(DeleteBehavior.Cascade);
			
		builder.Entity<ShoppingCartItem>()
			.HasOne(sci => sci.Product)
			.WithMany()
			.HasForeignKey(sci => sci.ProductId)
			.OnDelete(DeleteBehavior.Cascade);

	}
}
