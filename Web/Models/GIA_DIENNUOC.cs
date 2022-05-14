namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GIA_DIENNUOC
    {
        public int id { get; set; }

        public double giadien { get; set; }

        public double gianuoc { get; set; }
    }
}
