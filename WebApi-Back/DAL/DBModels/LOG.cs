namespace NtripProxy.DAL.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOG")]
    public partial class LOG
    {
        public Guid ID { get; set; }

        public DateTime? Log_Time { get; set; }

        public Guid? Log_User { get; set; }

        [StringLength(255)]
        public string Log_Action { get; set; }

        [StringLength(255)]
        public string Log_Module { get; set; }

        public string Log_Message { get; set; }
    }
}
