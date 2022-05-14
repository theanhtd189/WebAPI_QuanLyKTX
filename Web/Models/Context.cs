using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Web.Models
{
    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }

        public virtual DbSet<HOADON> HOADONs { get; set; }
        public virtual DbSet<HOCSINH> HOCSINHs { get; set; }
        public virtual DbSet<HOCSINH_NEW> HOCSINH_NEW { get; set; }
        public virtual DbSet<HOCSINH_OLD> HOCSINH_OLD { get; set; }
        public virtual DbSet<PHIEU_DIENNUOC> PHIEU_DIENNUOC { get; set; }
        public virtual DbSet<PHONG> PHONGs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }
        public virtual DbSet<VATTU> VATTUs { get; set; }
        public virtual DbSet<VATTU_PHONG> VATTU_PHONG { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PHIEU_DIENNUOC>()
                .HasMany(e => e.HOADONs)
                .WithRequired(e => e.PHIEU_DIENNUOC)
                .HasForeignKey(e => e.maphieudiennuoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PHONG>()
                .HasMany(e => e.HOCSINHs)
                .WithRequired(e => e.PHONG)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PHONG>()
                .HasMany(e => e.PHIEU_DIENNUOC)
                .WithRequired(e => e.PHONG)
                .WillCascadeOnDelete(false);
        }
    }
}
