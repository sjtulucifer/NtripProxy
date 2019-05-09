namespace NtripForward.DataModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NtripForwardDB : DbContext
    {
        public NtripForwardDB()
            : base("name=NtripForwardDB")
        {
        }

        public virtual DbSet<ACCOUNT> ACCOUNTs { get; set; }
        public virtual DbSet<ACCOUNTSYS> ACCOUNTSYS { get; set; }
        public virtual DbSet<GGAHistory> GGAHistories { get; set; }
        public virtual DbSet<SessionHistory> SessionHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GGAHistory>()
                .Property(e => e.Lng)
                .HasPrecision(10, 6);

            modelBuilder.Entity<GGAHistory>()
                .Property(e => e.Lat)
                .HasPrecision(10, 6);

            modelBuilder.Entity<SessionHistory>()
                .HasMany(e => e.GGAHistories)
                .WithOptional(e => e.SessionHistory)
                .HasForeignKey(e => e.SessionID);
        }
    }
}
