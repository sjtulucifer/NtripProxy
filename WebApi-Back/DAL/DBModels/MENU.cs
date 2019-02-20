namespace NtripProxy.DAL.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MENU")]
    public partial class MENU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MENU()
        {
            PERMISSIONs = new HashSet<PERMISSION>();
        }

        public Guid ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Menu_Catagory { get; set; }

        [Required]
        [StringLength(255)]
        public string Menu_Name { get; set; }

        [Required]
        public string Menu_URL { get; set; }

        public string Menu_Description { get; set; }

        public bool? isDelete { get; set; }

        public DateTime? createTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PERMISSION> PERMISSIONs { get; set; }
    }
}
