using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace StoreLibrary.Models
{
    public partial class H60AssignmentDB_jbContext : DbContext
    {
        public H60AssignmentDB_jbContext()
        {
        }

        public H60AssignmentDB_jbContext(DbContextOptions<H60AssignmentDB_jbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Product { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategory { get; set; } = null!;


        public virtual DbSet<CartItem> CartItem { get; set; } = null!;
        public virtual DbSet<Customer> Customer { get; set; } = null!;
        public virtual DbSet<Order> Order { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItem { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; } = null!;






        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Name=ConnectionStrings:MyConnection");
                // optionsBuilder.UseSqlServer("Server=cssql.cegep-heritage.qc.ca;Database=TestingScript06;User id=JBODIKA;Password=password;");


            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("ProductCategory");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ProdCat)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.HasData(
                new ProductCategory { CategoryId = 1, ProdCat = "Skin Care" },
                new ProductCategory { CategoryId = 2, ProdCat = "Hair Care" },
                new ProductCategory { CategoryId = 3, ProdCat = "Fragrances" },
                new ProductCategory { CategoryId = 4, ProdCat = "Makeup" },
                new ProductCategory { CategoryId = 5, ProdCat = "Tools and Brushes" });
            });


            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.ProdCatId, "IX_Product_ProdCatId");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.BuyPrice).HasColumnType("numeric(8, 2)");

                entity.Property(e => e.Description)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeNotes)

                         .HasColumnName("EmployeeNotes")
                          .HasMaxLength(80)
                           .IsUnicode(false);

                entity.Property(e => e.Image)
          .HasColumnType("varbinary(max)");

                entity.Property(e => e.SellPrice).HasColumnType("numeric(8, 2)");

                entity.HasOne(d => d.ProdCat)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProdCatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductCategory");

                entity.HasData(
              new Product
              {
                  ProductId = 1,
                  ProdCatId = 1,
                  Description = "Radiant Glow Serum",
                  Manufacturer = "Lux Beauty",
                  Stock = 100,
                  BuyPrice = (decimal)10.00,
                  SellPrice = (decimal)25.00,
                  EmployeeNotes = null,
                     Image = ImageToByteArray("./wwwroot/images/fragrance.png")
              },
              new Product
              {
                  ProductId = 2,
                  ProdCatId = 1,
                  Description = "Hyaluronic Acid Moisturizer",
                  Manufacturer = "Pure Essentials",
                  Stock = 200,
                  BuyPrice = (decimal)12.00,
                  SellPrice = (decimal)28.00,
                  EmployeeNotes = null
              },
              new Product
              {
                  ProductId = 3,
                  ProdCatId = 1,
                  Description = "Charcoal Detoxifying Mask",
                  Manufacturer = "Zen Skin Solutions",
                  Stock = 130,
                  BuyPrice = (decimal)9.00,
                  SellPrice = (decimal)22.00,
                  EmployeeNotes = "Newly added product"
              },


              new Product
              {
                  ProductId = 4,
                  ProdCatId = 1,
                  Description = "Vitamin C Brightening Toner",
                  Manufacturer = "GlowBunny Cosmetics",
                  Stock = 180,
                  BuyPrice = (decimal)8.00,
                  SellPrice = (decimal)25.00,
                  EmployeeNotes = null

              },

              new Product

              {
                  ProductId = 5,
                  ProdCatId = 3,
                  Description = "Midnight Orchid Eau de Parfum",
                  Manufacturer = "Lux Beauty",
                  Stock = 75,
                  BuyPrice = (decimal)15.00,
                  SellPrice = (decimal)40.00,
                  EmployeeNotes = null
              },
               new Product
               {
                   ProductId = 6,
                   ProdCatId = 4,
                   Description = "Velvet Matte Lipstick",
                   Manufacturer = "Lux Beauty",
                   Stock = 120,
                   BuyPrice = (decimal)5.00,
                   SellPrice = (decimal)15.00,
                   EmployeeNotes = "Best Selling Product!"
               },
                new Product
                {
                    ProductId = 7,
                    ProdCatId = 5,
                    Description = "Jade Roller and Gua Sha Set",
                    Manufacturer = "Zen Skin Solutions",
                    Stock = 160,
                    BuyPrice = (decimal)25.00,
                    SellPrice = (decimal)56.00,
                    EmployeeNotes = null
                },
    new Product
    {
        ProductId = 8,
        ProdCatId = 4,
        Description = "HD Foundation Primer",
        Manufacturer = "Pure Essentials",
        Stock = 150,
        BuyPrice = (decimal)7.00,
        SellPrice = (decimal)20.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 9,
        ProdCatId = 4,
        Description = "Metallic Eyeshadow Palette",
        Manufacturer = "GlowBunny Cosmetics",
        Stock = 110,
        BuyPrice = (decimal)13.00,
        SellPrice = (decimal)32.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 10,
        ProdCatId = 3,
        Description = "Vanilla Spice Perfume Oil",
        Manufacturer = "GlowBunny Cosmetics",
        Stock = 0,
        BuyPrice = (decimal)16.00,
        SellPrice = (decimal)38.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 11,
        ProdCatId = 3,
        Description = "Ocean Breeze Body Mist",
        Manufacturer = "Zen Skin Solutions",
        Stock = 80,
        BuyPrice = (decimal)14.00,
        SellPrice = (decimal)32.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 12,
        ProdCatId = 4,
        Description = "Waterproof Mascara",
        Manufacturer = "Zen Skin Solutions",
        Stock = 140,
        BuyPrice = (decimal)6.00,
        SellPrice = (decimal)18.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 13,
        ProdCatId = 3,
        Description = "Citrus Sunshine Cologne",
        Manufacturer = "Pure Essentials",
        Stock = 92,
        BuyPrice = (decimal)18.00,
        SellPrice = (decimal)45.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 14,
        ProdCatId = 5,
        Description = "Ionic Hair Straightener",
        Manufacturer = "Pure Essentials",
        Stock = 90,
        BuyPrice = (decimal)125.00,
        SellPrice = (decimal)300.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 15,
        ProdCatId = 5,
        Description = "Professional Makeup Brush Set",
        Manufacturer = "Lux Beauty",
        Stock = 50,
        BuyPrice = (decimal)24.00,
        SellPrice = (decimal)49.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 16,
        ProdCatId = 5,
        Description = "Precision Tweezers",
        Manufacturer = "Lux Beauty",
        Stock = 75,
        BuyPrice = (decimal)7.00,
        SellPrice = (decimal)15.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 17,
        ProdCatId = 2,
        Description = "Keratin Hair Mask",
        Manufacturer = "Zen Skin Solutions",
        Stock = 160,
        BuyPrice = (decimal)9.00,
        SellPrice = (decimal)22.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 18,
        ProdCatId = 2,
        Description = "Coconut Milk Conditioner",
        Manufacturer = "Pure Essentials",
        Stock = 180,
        BuyPrice = (decimal)10.00,
        SellPrice = (decimal)24.00,
        EmployeeNotes = null
    },
    new Product
    {
        ProductId = 19,
        ProdCatId = 2,
        Description = "Argan Oil Shampoo",
        Manufacturer = "Lux Beauty",
        Stock = 150,
        BuyPrice = (decimal)8.00,
        SellPrice = (decimal)20.00,
        EmployeeNotes = null
    },
     new Product
     {
         ProductId = 20,
         ProdCatId = 4,
         Description = "Glow Bomb",
         Manufacturer = "GlowBunny Cosmetics",
         Stock = 0,
         BuyPrice = (decimal)8.00,
         SellPrice = (decimal)20.00,
         EmployeeNotes = null
     },
    new Product
    {
        ProductId = 21,
        ProdCatId = 2,
        Description = "Curl Defining Cream",
        Manufacturer = "GlowBunny Cosmetics",
        Stock = 130,
        BuyPrice = (decimal)10.00,
        SellPrice = (decimal)26.00,
        EmployeeNotes = null,

    });

            });



            modelBuilder.Entity<Customer>().HasData(
               new Customer() { CustomerId = 1, FirstName = "Joelle", LastName = "Cunningham", Email = "joelle.cunningham@gmail.com", Province = "ON", PhoneNumber = "613-478-4783", CreditCard = "4519 4383 3829 4738" },
               new Customer() { CustomerId = 2, FirstName = "Brandie", LastName = "Lucas", Email = "brandie.lucas@yahoo.com", Province = "QC", PhoneNumber = "819-478-3923", CreditCard = "4378 4367 2143 0329" },
               new Customer() { CustomerId = 3, FirstName = "Troy", LastName = "Pena", Email = "t.pena@outlook.com", Province = "AB", PhoneNumber = "825-372-1273", CreditCard = "3267 2173 4398 4388" });


            modelBuilder.Entity<Order>().HasData(
                new Order() { OrderId = 1, CustomerId = 1, DateCreated = DateTime.Now.AddDays(-10).Date, DateFufilled = DateTime.Now.AddDays(-5).Date, Total = 86, Taxes = (decimal)12.88 });


            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem() { OrderItemId = 1, ProductId = 5, OrderId = 1, Price = (decimal)40.00, Quantity = 1 },
                new OrderItem() { OrderItemId = 2, ProductId = 8, OrderId = 1, Price = (decimal)20.00, Quantity = 2 },
                new OrderItem() { OrderItemId = 3, ProductId = 4, OrderId = 1, Price = (decimal)25.00, Quantity = 1 });



            modelBuilder.Entity<ShoppingCart>(x =>
            {
                x.HasKey(x => x.CartId);
                x.HasData(
                new ShoppingCart() { CartId = 1, CustomerId = 1, DateCreated = DateTime.Now.AddDays(-10).Date },
                new ShoppingCart() { CartId = 2, CustomerId = 2, DateCreated = DateTime.Now.AddDays(-5).Date });
            });

            modelBuilder.Entity<CartItem>()
         .HasOne(c => c.Product)
         .WithMany(p => p.CartItems)
         .HasForeignKey(c => c.ProductId);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.ShoppingCart)
                .WithMany(s => s.CartItems)
                .HasForeignKey(c => c.CartId);

      




            modelBuilder.Entity<CartItem>().HasData(
                    new CartItem() { CartItemId = 1, CartId = 1, ProductId = 5, Quantity = 1, Price = (decimal)40.00 },
                    new CartItem() { CartItemId = 2, CartId = 1, ProductId = 8, Quantity = 2, Price = (decimal)20.00 },
                    new CartItem() { CartItemId = 3, CartId = 1, ProductId = 4, Quantity = 1, Price = (decimal)25.00 },
                    new CartItem() { CartItemId = 4, CartId = 2, ProductId = 15, Quantity = 2, Price = (decimal)49.00 },
                    new CartItem() { CartItemId = 5, CartId = 2, ProductId = 17, Quantity = 1, Price = (decimal)22.00 });




            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public static byte[] ImageToByteArray(string imagePath)
        {
            byte[] imageBytes = null;

            try
            {
                using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        imageBytes = reader.ReadBytes((int)stream.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur when reading the file
                Console.WriteLine("Error: " + ex.Message);
            }

            return imageBytes;
        }
    }
}




