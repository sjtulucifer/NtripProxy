namespace NtripProxy.DAL.DBModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NtripProxyDB : DbContext
    {
        public NtripProxyDB()
            : base("name=NtripProxyDB")
        {
        }

        public virtual DbSet<ACCOUNT> ACCOUNTs { get; set; }
        public virtual DbSet<ACCOUNTSYS> ACCOUNTSYS { get; set; }
        public virtual DbSet<COMPANY> COMPANies { get; set; }
        public virtual DbSet<GGAHistory> GGAHistories { get; set; }
        public virtual DbSet<LOG> LOGs { get; set; }
        public virtual DbSet<MENU> MENUs { get; set; }
        public virtual DbSet<PERMISSION> PERMISSIONs { get; set; }
        public virtual DbSet<ROLE> ROLEs { get; set; }
        public virtual DbSet<SessionHistory> SessionHistories { get; set; }
        public virtual DbSet<USER> USERs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<COMPANY>()
                .HasMany(e => e.ACCOUNTs)
                .WithRequired(e => e.COMPANY)
                .HasForeignKey(e => e.Account_Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<COMPANY>()
                .HasMany(e => e.COMPANY1)
                .WithOptional(e => e.COMPANY2)
                .HasForeignKey(e => e.Company_Chief);

            modelBuilder.Entity<COMPANY>()
                .HasMany(e => e.USERs)
                .WithOptional(e => e.COMPANY)
                .HasForeignKey(e => e.User_Campany);

            modelBuilder.Entity<GGAHistory>()
                .Property(e => e.Lng)
                .HasPrecision(10, 6);

            modelBuilder.Entity<GGAHistory>()
                .Property(e => e.Lat)
                .HasPrecision(10, 6);

            modelBuilder.Entity<MENU>()
                .HasMany(e => e.PERMISSIONs)
                .WithMany(e => e.MENUs)
                .Map(m => m.ToTable("PEMISSION_MENU").MapLeftKey("Menu_ID").MapRightKey("Permission_ID"));

            modelBuilder.Entity<PERMISSION>()
                .HasMany(e => e.ROLEs)
                .WithMany(e => e.PERMISSIONs)
                .Map(m => m.ToTable("ROLE_PERMISSION"));

            modelBuilder.Entity<ROLE>()
                .HasMany(e => e.USERs)
                .WithMany(e => e.ROLEs)
                .Map(m => m.ToTable("USER_ROLE"));

            modelBuilder.Entity<SessionHistory>()
                .HasMany(e => e.GGAHistories)
                .WithOptional(e => e.SessionHistory)
                .HasForeignKey(e => e.SessionID);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.ACCOUNTs)
                .WithOptional(e => e.USER)
                .HasForeignKey(e => e.Account_AddUser);
        }
    }
}
