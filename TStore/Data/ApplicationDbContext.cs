using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TStore.Common;
using TStore.Models;

namespace TStore.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();

            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }

        modelBuilder.Entity<OrderDetail>()
                    .HasKey(e => new { e.OrderId, e.Id });


        modelBuilder.Entity<Product>()
                    .Property(e => e.Price)
                    .HasDefaultValue(0);
        modelBuilder.Entity<Product>()
                   .Property(e => e.PromotionPrice)
                   .HasDefaultValue(0);
        modelBuilder.Entity<Product>()
                   .Property(e => e.Rating)
                   .HasDefaultValue(0);
        modelBuilder.Entity<Product>()
                   .Property(e => e.Quantity)
                   .HasDefaultValue(0);
        modelBuilder.Entity<Product>()
                   .Property(e => e.IsHot)
                   .HasDefaultValue(false);
        modelBuilder.Entity<Product>()
                   .Property(e => e.ViewCount)
                   .HasDefaultValue(0);
        modelBuilder.Entity<Product>()
                   .HasIndex(e => e.SeoName)
                   .IsUnique();

        modelBuilder.Entity<ProductCategory>()
                   .Property(e => e.Status)
                   .HasDefaultValue(true);
        modelBuilder.Entity<ProductCategory>()
                   .HasIndex(e => e.SeoName)
                   .IsUnique();



        // Add Relationship
        modelBuilder.Entity<ProductCategory>()
                    .HasMany(e => e.Products)
                    .WithOne(e => e.ProductCategory)
                    .HasForeignKey(e => e.ProductCategoryId)
                    .IsRequired();

        modelBuilder.Entity<Product>()
                    .HasOne(e => e.ProductCategory)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.ProductCategoryId)
                    .IsRequired();

        modelBuilder.Entity<Brand>()
                    .HasMany(e => e.Products)
                    .WithOne(e => e.Brand)
                    .HasForeignKey(e => e.BrandId)
                    .IsRequired();

        modelBuilder.Entity<Product>()
                    .HasOne(e => e.Brand)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.BrandId)
                    .IsRequired();

        modelBuilder.Entity<Supplier>()
                    .HasMany(e => e.Products)
                    .WithOne(e => e.Supplier)
                    .HasForeignKey(e => e.SupplierId)
                    .IsRequired();

        modelBuilder.Entity<Product>()
                    .HasOne(e => e.Supplier)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.SupplierId)
                    .IsRequired();

        modelBuilder.Entity<Orders>()
                    .HasMany(e => e.OrderDetails)
                    .WithOne(e => e.Orders)
                    .HasForeignKey(e => e.OrderId)
                    .IsRequired();

        modelBuilder.Entity<OrderDetail>()
                    .HasOne(e => e.Orders)
                    .WithMany(e => e.OrderDetails)
                    .HasForeignKey(e => e.OrderId)
                    .IsRequired();



        modelBuilder.Entity<Menu>()
                    .HasData(
                        new Menu { Id = 1, Title = "Trang chủ", Alias = "/", Description = "trang chủ", Position = 1 },
                        new Menu { Id = 2, Title = "Danh mục sản phẩm", Alias = "danh-muc-san-pham", Description = "danh mục sản phẩm", Position = 2 },
                        new Menu { Id = 4, Title = "Sản phẩm", Alias = "san-pham", Description = "san phẩm", Position = 3 },
                        new Menu { Id = 3, Title = "Giới thiệu", Alias = "gioi-thieu", Description = "giới thiệu", Position = 4 }
                    );

        modelBuilder.Entity<About>()
                    .HasData(
                        new About { Id = 1, Detail = "<h1 style=\"text-align: center;\"><strong>Giới thiệu TStore</strong></h1>\r\n<p><strong>Tổng gi&aacute;m đốc: <span style=\"color: rgb(53, 152, 219);\">L&ecirc; Trần Tấn T&agrave;i</span></strong></p>\r\n<p><span style=\"color: rgb(0, 0, 0);\"><strong>TStore</strong> l&agrave; một ứng dụng thương mại điện tử to&agrave;n cầu chuy&ecirc;n cung cấp h&agrave;ng h&oacute;a điện tử như l&agrave; \"Điện thoại\", \"Laptop\", ... Với sự chuy&ecirc;n nghiệp của nh&acirc;n vi&ecirc;n cũng như sự tận t&acirc;m trong từng sản phẩm của c&ocirc;ng ty ch&uacute;ng t&ocirc;i cam kết h&agrave;ng h&oacute;a ph&iacute;a <strong>TStore</strong> sẽ đồng h&agrave;nh c&ugrave;ng bạn một thời gian d&agrave;i với chăm ng&ocirc;n \"Bạn của bạn v&agrave; <strong>TStore</strong>, <strong>TStore</strong> l&agrave; bạn của bạn\" \U0001f923.</span></p>\r\n<p><strong><span style=\"color: rgb(0, 0, 0);\">Th&ocirc;ng tin li&ecirc;n hệ: </span></strong></p>\r\n<ul>\r\n<li><span style=\"color: rgb(0, 0, 0);\">Số điện thoại: <strong>0888888888</strong></span></li>\r\n<li><span style=\"color: rgb(0, 0, 0);\">Email: <strong>contact.devt.110103@gmail.com</strong></span></li>\r\n</ul>" }
                    );

        modelBuilder.Entity<ProductCategory>()
                   .HasData(
                       new ProductCategory { Id = 1, Name = "Điện thoại", SeoName = "dien-thoai", Status = true },
                       new ProductCategory { Id = 2, Name = "Laptop", SeoName = "laptop", Status = true },
                       new ProductCategory { Id = 3, Name = "Phụ kiện", SeoName = "phu-kien", Status = true }
                   );

    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseModel && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                ((BaseModel)entityEntry.Entity).CreatedAt = DateTime.Now;
            }
            ((BaseModel)entityEntry.Entity).UpdatedAt = DateTime.Now;
        }

        return base.SaveChanges();
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<About> Abouts { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<Slide> Slides { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
}