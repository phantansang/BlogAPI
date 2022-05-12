using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogAPI.Repository.Models
{
    public partial class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {
        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblPost> TblPost { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-TRLJ554\\SQLEXPRESS;Initial Catalog=BlogDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblPost>(entity =>
            {
                entity.HasKey(e => e.PostId);

                entity.ToTable("tblPOST");

                entity.Property(e => e.PostId)
                    .HasColumnName("_PostID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Author)
                    .HasColumnName("_Author")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Category)
                    .HasColumnName("_Category")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Content).HasColumnName("_Content");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("_CreateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("_Description")
                    .HasMaxLength(1000);

                entity.Property(e => e.Imagepath)
                    .HasColumnName("_Imagepath")
                    .HasMaxLength(200);

                entity.Property(e => e.LastUpdate)
                    .HasColumnName("_LastUpdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Log).HasColumnName("_Log");

                entity.Property(e => e.MetaTitle)
                    .HasColumnName("_MetaTitle")
                    .HasMaxLength(200);

                entity.Property(e => e.Status).HasColumnName("_Status");

                entity.Property(e => e.Title)
                    .HasColumnName("_Title")
                    .HasMaxLength(200);

                entity.Property(e => e.Views).HasColumnName("_Views");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
