namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PHONG")]
    public partial class PHONG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PHONG()
        {
            HOADONs = new HashSet<HOADON>();
            HOCSINHs = new HashSet<HOCSINH>();
            PHIEU_DIENNUOC = new HashSet<PHIEU_DIENNUOC>();
            VATTU_PHONG = new HashSet<VATTU_PHONG>();
        }

        [Key]
        public int maphong { get; set; }

        [Required]
        [StringLength(50)]
        public string tenphong { get; set; }

        public int tsogiuong { get; set; }

        public int? tang { get; set; }

        public bool? tinhtrang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOCSINH> HOCSINHs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEU_DIENNUOC> PHIEU_DIENNUOC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VATTU_PHONG> VATTU_PHONG { get; set; }
    }
}
