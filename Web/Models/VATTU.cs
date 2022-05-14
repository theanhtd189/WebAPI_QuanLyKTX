namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VATTU")]
    public partial class VATTU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VATTU()
        {
            VATTU_PHONG = new HashSet<VATTU_PHONG>();
        }

        [Key]
        public int mavt { get; set; }

        [StringLength(100)]
        public string tenvattu { get; set; }

        public int? soluong { get; set; }

        public double? giatien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VATTU_PHONG> VATTU_PHONG { get; set; }
    }
}
