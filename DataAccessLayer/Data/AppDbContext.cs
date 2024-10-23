using System;
using System.Collections.Generic;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccessLayer.Data;

public class AppDbContext : IdentityDbContext<User>
{

    public AppDbContext()
    {

    }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    
    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<ApplicationOrder> ApplicationOrders { get; set; }

    public virtual DbSet<ApplicationOrdersType> ApplicationOrdersTypes { get; set; }

    public virtual DbSet<ApplicationType> ApplicationTypes { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<CitiesWhereDeliveiesWork> CitiesWhereDeliveiesWorks { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentsType> PaymentsTypes { get; set; }

    public virtual DbSet<PersonAddress> PeopleAddresses { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductCategoryImage> ProductCategoryImages { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<ProductsInShoppingCart> ProductsInShoppingCarts { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<SellerProduct> SellerProducts { get; set; }

    public virtual DbSet<ShippingCost> ShippingCosts { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer("Server=.;Database=Amazon_E_Commerce_DB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC072B1F9265");

            entity.ToTable("Applications");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ApplicationTypeId)
                .HasConstraintName("FK_Application_ApplicationTypeId");

            
        });

        modelBuilder.Entity<ApplicationOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC07270C916F");

            entity.Property(e => e.PersonAddress).HasMaxLength(500);
            entity.Property(e => e.ShippingCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Application).WithMany(p => p.ApplicationOrders)
                .HasForeignKey(d => d.ApplicationId)
                .HasConstraintName("FK_ApplicationOrders_ApplicationId");

            entity.HasOne(d => d.ApplicationOrderType).WithMany(p => p.ApplicationOrders)
                .HasForeignKey(d => d.ApplicationOrderTypeId)
                .HasConstraintName("FK_ApplicationOrders_ApplicationOrderTypeId");

            entity.HasOne(d => d.Delivery).WithMany(p => p.ApplicationOrders)
                .HasForeignKey(d => d.DeliveryId)
                .HasConstraintName("FK_ApplicationOrders_DeliveryId");

            entity.HasOne(d => d.Payment).WithMany(p => p.ApplicationOrders)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_ApplicationOrders_PaymentId");

            entity.HasOne(d => d.ShoppingCart).WithMany(p => p.ApplicationOrders)
                .HasForeignKey(d => d.ShoppingCartId)
                .HasConstraintName("FK_ApplicationOrders_ShoppingCartId");
        });

        modelBuilder.Entity<ApplicationOrdersType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC0724535CF2");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<ApplicationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC0773737997");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brands__3214EC07FE6191F7");

            entity.Property(e => e.NameAr)
                .HasMaxLength(255)
                .HasColumnName("Name_Ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(255)
                .HasColumnName("Name_En");
        });

        modelBuilder.Entity<CitiesWhereDeliveiesWork>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CitiesWh__3214EC0714D1D3E2");

            entity.HasOne(d => d.City).WithMany(p => p.CitiesWhereDeliveiesWorks)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_CitiesWhereDeliveiesWorks_CityId");

            entity.HasOne(d => d.Delivery).WithMany(p => p.CitiesWhereDeliveiesWorks)
                .HasForeignKey(d => d.DeliveryId)
                .HasConstraintName("FK_CitiesWhereDeliveiesWorks_DeliveryId");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC07A2F63C83");

            entity.Property(e => e.NameAr)
                .HasMaxLength(255)
                .HasColumnName("Name_Ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(255)
                .HasColumnName("Name_En");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Deliveri__3214EC07322C23DA");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC07FFE12C42");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentTypeId)
                .HasConstraintName("FK_Payments_PaymentTypeId");
        });

        modelBuilder.Entity<PaymentsType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC073B7ADCD8");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<PersonAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PeopleAd__3214EC071308F9C3");

            entity.Property(e => e.Address).HasMaxLength(500);

            entity.HasOne(d => d.City).WithMany(p => p.PeopleAddresses)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_PeopleAddresses_CityId");

            entity.HasOne(d => d.Person).WithMany(p => p.PeopleAddresses)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK_PeopleAddresses_PersonId");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__People__3214EC0750368F63");

            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Phones__3214EC076BAE186A");

            entity.Property(e => e.PhoneNumber).HasMaxLength(50);

            entity.HasOne(d => d.Person).WithMany(p => p.Phones)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK_Phones_PersonId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07813EF3B4");

            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Height).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Length).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NameAr)
                .HasMaxLength(255)
                .HasColumnName("Name_Ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(255)
                .HasColumnName("Name_En");
            entity.Property(e => e.Size).HasMaxLength(50);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_Products_BrandId");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategoryId)
                .HasConstraintName("FK_Products_ProductCategoryId");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductC__3214EC07FFD11FC9");

            entity.HasIndex(e => e.NameEn, "UQ__ProductC__33341C8A8951CC7B").IsUnique();

            entity.HasIndex(e => e.NameAr, "UQ__ProductC__33347C132A72C059").IsUnique();

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(1000)
                .HasColumnName("Description_Ar");
            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(1000)
                .HasColumnName("Description_En");
            entity.Property(e => e.NameAr)
                .HasMaxLength(255)
                .HasColumnName("Name_Ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(255)
                .HasColumnName("Name_En");
        });

        modelBuilder.Entity<ProductCategoryImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductC__3214EC073F10B0DE");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.ProductCategoryImages)
                .HasForeignKey(d => d.ProductCategoryId)
                .HasConstraintName("FK_ProductCategoryImages_ProductCategoryId");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC07794FBA5A");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductImages_ProductId");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductR__3214EC07ADEC10FB");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Message).HasMaxLength(1000);

            entity.HasOne(d => d.SellerProduct).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.SellerProductId)
                .HasConstraintName("FK_ProductReviews_SellerProductId");

            

           
        });

        modelBuilder.Entity<ProductsInShoppingCart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC0709E6EA67");

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.SellerProduct).WithMany(p => p.ProductsInShoppingCarts)
                .HasForeignKey(d => d.SellerProductId)
                .HasConstraintName("FK_ProductsInShoppingCarts_SellerProductId");

            entity.HasOne(d => d.ShoppingCart).WithMany(p => p.ProductsInShoppingCarts)
                .HasForeignKey(d => d.ShoppingCartId)
                .HasConstraintName("FK_ProductsInShoppingCarts_ShoppingCartId");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC075E8158E9");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<SellerProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SellerPr__3214EC07E45442F5");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.SellerProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_SellerProducts_ProductId");
        });

        modelBuilder.Entity<ShippingCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shipping__3214EC075E9DA2D6");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.City).WithMany(p => p.ShippingCosts)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_ShippingCosts_CityId");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shopping__3214EC07B8EE46F4");
        });

        modelBuilder.Entity<IdentityUser>(entity =>
        {
            entity.ToTable("Users");
            entity.Ignore(p => p.PhoneNumber);
            entity.Ignore(p => p.PhoneNumberConfirmed);
        });

        modelBuilder.Ignore<IdentityUserClaim<string>>();
        modelBuilder.Ignore<IdentityRoleClaim<string>>();
        modelBuilder.Ignore<IdentityUserLogin<string>>();
        modelBuilder.Ignore<IdentityUserToken<string>>();

        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");

        
        ApplyDeleteRestrict(modelBuilder);
    }

    private void ApplyDeleteRestrict(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var foreignKeys = entity.GetForeignKeys();
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}

