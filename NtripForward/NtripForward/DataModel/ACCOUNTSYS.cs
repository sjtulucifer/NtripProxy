namespace NtripForward.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ACCOUNTSYS")]
    public partial class ACCOUNTSYS
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(255)]
        public string AccountSYS_Name { get; set; }

        [Required]
        [StringLength(255)]
        public string AccountSYS_Password { get; set; }

        [Column(TypeName = "date")]
        public DateTime AccountSYS_Register { get; set; }

        [Column(TypeName = "date")]
        public DateTime AccountSYS_Expire { get; set; }

        public DateTime? AccountSYS_LastLogin { get; set; }

        public int? AccountSYS_Age { get; set; }

        public bool? AccountSYS_IsLocked { get; set; }

        public bool? AccountSYS_IsOnline { get; set; }

        public bool? isDelete { get; set; }

        public DateTime? createTime { get; set; }
    }
}
