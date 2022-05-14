namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HOCSINH_OLD
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int mahs { get; set; }

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
    }
}
