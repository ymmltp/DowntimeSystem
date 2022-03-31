using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DowntimeSystem.Models
{
    public partial class ECContext : DbContext
    {
        public ECContext()
        {
        }

        public ECContext(DbContextOptions<ECContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IncidentDet> IncidentDets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=cnwuxm1medb01;Database=EC;Username=ECUser;Password=Jabil123");
            }
        }
        private string[] contains = { "eCalling", "Sparepart", "FPY", "Downtime System" }; 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasAnnotation("Relational:Collation", "English_United States.1252");
            modelBuilder.Entity<IncidentDet>(entity =>
            {
                entity.HasQueryFilter(e => e.Calcdowntime.Equals(true));
                entity.HasQueryFilter(e =>Array.AsReadOnly(contains).Contains(e.Comefrom));


                entity.ToTable("incident_det");

                entity.HasIndex(e => new { e.Comefrom, e.Occurtime }, "comefrom");

                entity.HasIndex(e => new { e.Project, e.Line, e.Occurtime }, "occurtime");

                entity.HasIndex(e => new { e.Project, e.Line, e.Station, e.Occurtime }, "station");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action)
                    .HasMaxLength(64)
                    .HasColumnName("action");

                entity.Property(e => e.Actionremark)
                    .HasMaxLength(512)
                    .HasColumnName("actionremark");

                entity.Property(e => e.Actionstatus)
                    .HasColumnName("actionstatus")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Alarmtype)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("alarmtype");

                entity.Property(e => e.Calcdowntime)
                    .IsRequired()
                    .HasColumnName("calcdowntime")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Comefrom)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("comefrom");

                entity.Property(e => e.Creator)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("creator");

                entity.Property(e => e.Ctime)
                    .HasColumnName("ctime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Department)
                    .HasMaxLength(32)
                    .HasColumnName("department");

                entity.Property(e => e.Downtime)
                    .HasColumnName("downtime")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Finishtime).HasColumnName("finishtime");

                entity.Property(e => e.Incidentstatus)
                    .HasColumnName("incidentstatus")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Issue)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("issue");

                entity.Property(e => e.Issueremark)
                    .HasMaxLength(512)
                    .HasColumnName("issueremark");

                entity.Property(e => e.Labor)
                    .HasColumnName("labor")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Line)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("line");

                entity.Property(e => e.Machine)
                    .HasMaxLength(64)
                    .HasColumnName("machine");

                entity.Property(e => e.Occurtime).HasColumnName("occurtime");

                entity.Property(e => e.Project)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("project");

                entity.Property(e => e.Repairtime)
                    .HasColumnName("repairtime")
                    .HasComment("Repair datetime");

                entity.Property(e => e.Respperson)
                    .HasMaxLength(32)
                    .HasColumnName("respperson");

                entity.Property(e => e.Rootcause)
                    .HasMaxLength(64)
                    .HasColumnName("rootcause");

                entity.Property(e => e.Rootcauseremark)
                    .HasMaxLength(512)
                    .HasColumnName("rootcauseremark");

                entity.Property(e => e.Station)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("station");

                entity.Property(e => e.Urgentlevel)
                    .HasColumnName("urgentlevel")
                    .HasDefaultValueSql("0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
