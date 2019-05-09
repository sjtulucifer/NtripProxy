namespace JXCWPlus_Back.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ACCOUNT")]
    public partial class ACCOUNT
    {
        public Guid ID { get; set; }

        public Guid Account_Company { get; set; }

        [Required]
        [StringLength(255)]
        public string Account_Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Account_Password { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Account_Register { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Account_Expire { get; set; }

        public DateTime? Account_LastLogin { get; set; }

        public DateTime? Account_PasswordOvertime { get; set; }

        public int? Account_PasswordOvercount { get; set; }

        public bool? Account_IsLocked { get; set; }

        public bool? Account_IsOnline { get; set; }

        public Guid? Account_AddUser { get; set; }

        public bool? isDelete { get; set; }

        public DateTime? createTime { get; set; }
    }
}
