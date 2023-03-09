using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebPruebaSofttek.Models
{
    public partial class CoreDBContext : DbContext
    {
        private String ConnectionString;
        public CoreDBContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public CoreDBContext(DbContextOptions<CoreDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tbsaleasecom> Tbsaleasecom { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=GKNPROYECTOS08\\SQL;Database=dbPrueba;User Id=sa;Password=123.");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tbsaleasecom>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbsaleasecom");

                entity.Property(e => e.Cnt).HasColumnName("cnt");

                entity.Property(e => e.Datefull)
                    .HasColumnName("datefull")
                    .HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Pricefull)
                    .HasColumnName("pricefull")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Priceunit)
                    .HasColumnName("priceunit")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Productname)
                    .HasColumnName("productname")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
