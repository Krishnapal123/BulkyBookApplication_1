using BulkyBookApp.Models;
using BulkyBookWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookApp.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Define the relationship between Userclass and Role
        modelBuilder.Entity<Userclass>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(u => u.ToTable("UserRole"));

     
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Userclass> User {get; set; }
    public DbSet<Product> Products { get; set; }

    // Add DbSet for Cart and CartItem
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Role> Roles { get; set; }



}
