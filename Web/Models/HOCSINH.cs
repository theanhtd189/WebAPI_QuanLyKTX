namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HOCSINH")]
    public partial class HOCSINH
    {
        [Key]
        public int mahs { get; set; }

        public int maphong { get; set; }

        [Required]
        [StringLength(50)]
        public string hoten { get; set; }

        public DateTime? ngaysinh { get; set; }

        [StringLength(100)]
        public string quequan { get; set; }

        public bool? gioitinh { get; set; }

        [StringLength(150)]
        public string ttphuhuynh { get; set; }

        [StringLength(30)]
        public string lop { get; set; }

        public virtual PHONG PHONG { get; set; }
    }
}
