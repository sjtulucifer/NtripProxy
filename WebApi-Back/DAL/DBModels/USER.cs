namespace NtripProxy.DAL.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USER")]
    public partial class USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            ACCOUNTs = new HashSet<ACCOUNT>();
            ROLEs = new HashSet<ROLE>();
        }

        public Guid ID { get; set; }

        [Required]
        [StringLength(255)]
        public string User_Login { get; set; }

        [Required]
        [StringLength(255)]
        public string User_Password { get; set; }

        [StringLength(255)]
        public string User_Name { get; set; }

        [StringLength(255)]
        public string User_Phone { get; set; }

        [StringLength(255)]
        public string User_Email { get; set; }

        public Guid? User_Campany { get; set; }

        public bool? isDelete { get; set; }

        public DateTime? createTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACCOUNT> ACCOUNTs { get; set; }

        public virtual COMPANY COMPANY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ROLE> ROLEs { get; set; }
    }
}
