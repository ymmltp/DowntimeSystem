using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DowntimeSystem.Models.HR
{
    public partial class iccojabilContext : DbContext
    {
        public iccojabilContext()
        {
        }

        public iccojabilContext(DbContextOptions<iccojabilContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EmpViewForTe> EmpViewForTes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=CNWUXM0HR02\\INST1;Initial Catalog=icco-jabil;User ID=TE_conn;Password=Jabil@TE123;MultipleActiveResultSets=True;Connection Timeout=120;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Jabil@TE123")
                .HasAnnotation("Relational:Collation", "Chinese_PRC_CI_AS");

            modelBuilder.Entity<EmpViewForTe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Emp_View_ForTE", "dbo");

                entity.Property(e => e.CardId)
                    .HasMaxLength(16)
                    .HasColumnName("card_id");

                entity.Property(e => e.ChineseName).HasMaxLength(50);

                entity.Property(e => e.Dept)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Empid)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("EMPID");

                entity.Property(e => e.Workcell).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
