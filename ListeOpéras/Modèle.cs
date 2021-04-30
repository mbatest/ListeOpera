namespace ListeOpéras
{
    using System.Data.Entity;

    public partial class Modèle : DbContext
    {
        public Modèle()
            : base("name=Modèle")
        {
        }

        public virtual DbSet<Acheté> Acheté { get; set; }
        public virtual DbSet<Définition> Définition { get; set; }
        public virtual DbSet<Diriger> Diriger { get; set; }
        public virtual DbSet<Disques> Disques { get; set; }
        public virtual DbSet<Editeur> Editeur { get; set; }
        public virtual DbSet<Instrument> Instrument { get; set; }
        public virtual DbSet<Interprétation> Interprétation { get; set; }
        public virtual DbSet<LienYouTube> LienYouTube { get; set; }
        public virtual DbSet<MiseEnScene> MiseEnScene { get; set; }
        public virtual DbSet<Musicien> Musicien { get; set; }
        public virtual DbSet<Opéra> Opéra { get; set; }
        public virtual DbSet<Orchestres> Orchestres { get; set; }
        public virtual DbSet<Pays> Pays { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Salles> Salles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acheté>()
                .Property(e => e.Prix)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Définition>()
                .HasMany(e => e.Disques)
                .WithOptional(e => e.Définition1)
                .HasForeignKey(e => e.Détail_Définition);

            modelBuilder.Entity<Disques>()
                .Property(e => e.Prix)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Disques>()
                .Property(e => e.Prix_payé)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Disques>()
                .HasMany(e => e.Diriger)
                .WithRequired(e => e.Disques)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disques>()
                .HasMany(e => e.Interprétation)
                .WithRequired(e => e.Disques)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disques>()
                .HasMany(e => e.MiseEnScene)
                .WithRequired(e => e.Disques)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Musicien>()
                .HasMany(e => e.Interprétation)
                .WithRequired(e => e.Musicien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Musicien>()
                .HasMany(e => e.MiseEnScene)
                .WithRequired(e => e.Musicien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Musicien>()
                .HasMany(e => e.Opéra)
                .WithOptional(e => e.Musicien)
                .HasForeignKey(e => e.Code_Musicien);

            modelBuilder.Entity<Musicien>()
                .HasMany(e => e.Opéra1)
                .WithMany(e => e.Musicien1)
                .Map(m => m.ToTable("Livret").MapLeftKey("Code_Musicien").MapRightKey("Code_Opéra"));

            modelBuilder.Entity<Orchestres>()
                .HasMany(e => e.Diriger)
                .WithRequired(e => e.Orchestres)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Interprétation)
                .WithRequired(e => e.Roles)
                .WillCascadeOnDelete(false);
        }
    }
}
