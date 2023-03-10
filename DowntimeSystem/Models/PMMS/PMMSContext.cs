using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DowntimeSystem.Models.PMMS
{
    public partial class PMMSContext : DbContext
    {
        public PMMSContext()
        {
        }

        public PMMSContext(DbContextOptions<PMMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Eq> Eqs { get; set; }
        public virtual DbSet<Pmitem> Pmitems { get; set; }
        public virtual DbSet<Pmrecord> Pmrecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=cnwuxm0lsql01;Initial Catalog=PMMS;User ID=pmms_readonly;Password=Jabil123;MultipleActiveResultSets=True;Connection Timeout=120");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Eq>(entity =>
            {
                entity.HasQueryFilter(e =>EF.Functions.Like(e.Workcell,"FATP%") & e.EStatus.Equals(1));                 //只获取FATP的设备ID
                entity.HasKey(e => e.Eqid);

                entity.ToTable("EQ");

                entity.Property(e => e.Eqid)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EQID");

                entity.Property(e => e.AcceptDate).HasColumnType("datetime");

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Department)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Descriptions).HasMaxLength(100);

                entity.Property(e => e.DeviationDate).HasColumnType("datetime");

                entity.Property(e => e.EStatus).HasColumnName("E_Status");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Line)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Owners)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Sn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SN");

                entity.Property(e => e.Source).HasMaxLength(50);

                entity.Property(e => e.Workcell)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pmitem>(entity =>
            {
                entity.HasKey(e => e.Pmid)
                    .HasName("PK__PMItems__412600BA03317E3D");

                entity.ToTable("PMItems");

                entity.Property(e => e.Pmid).HasColumnName("pmid");

                entity.Property(e => e.Eqid)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EQID");

                entity.Property(e => e.LastConfirmDate).HasColumnType("datetime");

                entity.Property(e => e.LockAt)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PmitemStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PMItemStatus");

                entity.Property(e => e.Pmtype)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("PMTYPE");

                entity.Property(e => e.VerifyResult)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VerifyTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Pmrecord>(entity =>
            {
                entity.HasKey(e => e.Prid)
                    .HasName("PK__PMrecord__46638AED07F6335A");

                entity.ToTable("PMrecord");

                entity.Property(e => e.Prid).HasColumnName("prid");

                entity.Property(e => e.DocLocation).HasMaxLength(100);

                entity.Property(e => e.LabCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Operator)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Owner)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Pmdate)
                    .HasColumnType("datetime")
                    .HasColumnName("pmdate");

                entity.Property(e => e.Pmid).HasColumnName("pmid");

                entity.Property(e => e.Result).HasMaxLength(1000);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Pm)
                    .WithMany(p => p.Pmrecords)
                    .HasForeignKey(d => d.Pmid)
                    .HasConstraintName("FK__PMrecord__pmid__09DE7BCC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
