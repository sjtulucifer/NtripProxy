namespace NtripProxy.DAL.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COMPANY")]
    public partial class COMPANY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMPANY()
        {
            ACCOUNTs = new HashSet<ACCOUNT>();
            COMPANY1 = new HashSet<COMPANY>();
            USERs = new HashSet<USER>();
        }

        public Guid ID { get; set; }

        public Guid? Company_Chief { get; set; }

        [Required]
        [StringLength(255)]
        public string Company_Name { get; set; }

        [StringLength(255)]
        public string Company_Corporation { get; set; }

        [StringLength(255)]
        public string Company_Qualification { get; set; }

        [StringLength(255)]
        public string Company_QNo { get; set; }

        [StringLength(255)]
        public string Company_Field { get; set; }

        [StringLength(255)]
        public string Company_Contract { get; set; }

        [StringLength(255)]
        public string Company_Phone { get; set; }

        public string Company_Address { get; set; }

        public bool? isDelete { get; set; }

        public DateTime? createTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ACCOUNT> ACCOUNTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMPANY> COMPANY1 { get; set; }

        public virtual COMPANY COMPANY2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER> USERs { get; set; }
    }
}
