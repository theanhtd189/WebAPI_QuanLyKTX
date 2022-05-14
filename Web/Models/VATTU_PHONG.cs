namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VATTU_PHONG
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int maphong { get; set; }

        public int mavt { get; set; }

        public int? soluong { get; set; }

        public virtual PHONG PHONG { get; set; }

        public virtual VATTU VATTU { get; set; }
    }
}
