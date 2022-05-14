namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PHIEU_DIENNUOC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHIEU_DIENNUOC()
        {
            HOADONs = new HashSet<HOADON>();
        }

        [Key]
        public int maphieu { get; set; }

        public DateTime? ngaytaophieu { get; set; }

        public int maphong { get; set; }

        public int? sodien { get; set; }

        public int? sonuoc { get; set; }

        public int? giadien { get; set; }

        public int? gianuoc { get; set; }

        public double? tongtien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }

        public virtual PHONG PHONG { get; set; }
    }
}
