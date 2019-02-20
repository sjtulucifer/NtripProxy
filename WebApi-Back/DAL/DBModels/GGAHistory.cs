namespace NtripProxy.DAL.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GGAHistory")]
    public partial class GGAHistory
    {
        public Guid ID { get; set; }

        [StringLength(255)]
        public string Account { get; set; }

        [StringLength(255)]
        public string AccountType { get; set; }

        [StringLength(255)]
        public string AccountSYS { get; set; }

        public DateTime? FixedTime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Lng { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Lat { get; set; }

        public int? Status { get; set; }

        public string GGAInfo { get; set; }

        public Guid? SessionID { get; set; }

        public virtual SessionHistory SessionHistory { get; set; }
    }
}
