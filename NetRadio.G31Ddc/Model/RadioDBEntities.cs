using NetRadio.G31Ddc.Model;
using System.Data.Entity;

namespace NetRadio.G31Ddc
{
    public class RadioDBEntities : DbContext
    {
        public RadioDBEntities()
            : base("RadioContext")
        {
            // Database.SetInitializer(new CustomInitializer());
            // Database.Initialize(true); // force the initialization

        }

        public virtual DbSet<Section> SectionEntity { get; set; }
        public virtual DbSet<Memory> MemoryEntity { get; set; }
        public virtual DbSet<Search> SearchEntity { get; set; }
        public virtual DbSet<SearchResult> SearchResultEntity { get; set; }
        public virtual DbSet<Scan> ScanEntity { get; set; }
        public virtual DbSet<ScanResult> ScanResultEntity { get; set; }
        public virtual DbSet<Skip> SkipEntity { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Section>()
                .HasMany(e => e.Children)
                .WithOptional(e => e.Parent)
                .HasForeignKey(e => e.ParentID);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.MemoryCollection)
                .WithRequired(e => e.Parent)
                .HasForeignKey(e => e.SectionID);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.SearchCollection)
                .WithRequired(e => e.Parent)
                .HasForeignKey(e => e.SectionID);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.SearchResultCollection)
                .WithRequired(e => e.Parent)
                .HasForeignKey(e => e.SectionID);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.ScanCollection)
                .WithRequired(e => e.Parent)
                .HasForeignKey(e => e.SectionID);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.ScanResultCollection)
                .WithRequired(e => e.Parent)
                .HasForeignKey(e => e.SectionID);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.SkipCollection)
                .WithRequired(e => e.Parent)
                .HasForeignKey(e => e.SectionID);
        }
    }
}
