using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Shop.DAL.Models
{
    public partial class ShopCRMContext : DbContext
    {
        public ShopCRMContext()
        {
        }

        public ShopCRMContext(DbContextOptions<ShopCRMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Good> Goods { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderGood> OrderGoods { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=ShopCRM;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Good>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_Orders_UserId");

                entity.HasIndex(e => e.OrderNumber, "Uq_OrderNumber")
                    .IsUnique();

                entity.Property(e => e.OrderCreateDate).HasColumnType("datetime");

                entity.Property(e => e.OrderUpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Orders_Users");
            });

            modelBuilder.Entity<OrderGood>(entity =>
            {
                entity.HasIndex(e => e.GoodsId, "IX_OrderGoods_GoodsId");

                entity.HasIndex(e => e.OrderId, "IX_OrderGoods_OrderId");

                entity.HasOne(d => d.Goods)
                    .WithMany(p => p.OrderGoods)
                    .HasForeignKey(d => d.GoodsId)
                    .HasConstraintName("FK_OrderGoods_Goods");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderGoods)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderGoods_Orders");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
