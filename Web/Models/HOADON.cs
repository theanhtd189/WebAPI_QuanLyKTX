namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HOADON")]
    public partial class HOADON
    {
        [Key]
        public int mahd { get; set; }

        public DateTime ngaytao { get; set; }

        public int maphong { get; set; }

        public double? tiendiennuoc { get; set; }

        public double? tongtien { get; set; }

        public double? tienphong { get; set; }

        [StringLength(50)]
        public string tenhs { get; set; }

        [StringLength(50)]
        public string tennv { get; set; }

        public int maphieudiennuoc { get; set; }

        public virtual PHIEU_DIENNUOC PHIEU_DIENNUOC { get; set; }

        public virtual PHONG PHONG { get; set; }
    }
}
