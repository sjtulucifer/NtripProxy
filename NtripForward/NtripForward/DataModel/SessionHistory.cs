namespace NtripForward.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SessionHistory")]
    public partial class SessionHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SessionHistory()
        {
            GGAHistories = new HashSet<GGAHistory>();
        }

        public Guid ID { get; set; }

        [StringLength(255)]
        public string AccountName { get; set; }

        [StringLength(255)]
        public string AccountType { get; set; }

        [StringLength(255)]
        public string AccountSYSName { get; set; }

        [StringLength(255)]
        public string MountPoint { get; set; }

        [StringLength(255)]
        public string Client { get; set; }

        [StringLength(255)]
        public string ClientAddress { get; set; }

        public DateTime? ConnectionStart { get; set; }

        public DateTime? ConnectionEnd { get; set; }

        public int? GGACount { get; set; }

        public int? FixedCount { get; set; }

        public string ErrorInfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GGAHistory> GGAHistories { get; set; }
    }
}
