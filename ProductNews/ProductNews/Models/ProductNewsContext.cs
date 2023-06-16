using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProductNews.Models
{
    public partial class ProductNewsContext : DbContext
    {
        public ProductNewsContext()
        {
        }

        public ProductNewsContext(DbContextOptions<ProductNewsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<NewsGroup> NewsGroups { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=localhost;database=ProductNews;uid=sa;password=123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.ModifiedHistory).HasColumnType("ntext");

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.NewId)
                    .HasName("PK__News__7CC3769EB12962E2");

                entity.Property(e => e.NewId).HasColumnName("NewID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedHistory).HasColumnType("ntext");

                entity.Property(e => e.NewsContent).HasColumnType("ntext");

                entity.Property(e => e.NewsGroupId).HasColumnName("NewsGroupID");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.NewsCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__News__CreatedBy__3D5E1FD2");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.NewsModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__News__ModifiedBy__3E52440B");

                entity.HasOne(d => d.NewsGroup)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.NewsGroupId)
                    .HasConstraintName("FK__News__NewsGroupI__3C69FB99");
            });

            modelBuilder.Entity<NewsGroup>(entity =>
            {
                entity.ToTable("NewsGroup");

                entity.Property(e => e.NewsGroupId).HasColumnName("NewsGroupID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedHistory).HasColumnType("ntext");

                entity.Property(e => e.NewsGroupName).HasMaxLength(100);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.NewsGroupCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__NewsGroup__Creat__38996AB5");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.NewsGroupModifiedByNavigations)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK__NewsGroup__Modif__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
