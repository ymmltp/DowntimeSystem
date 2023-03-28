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

        public virtual DbSet<EscalationNameList> EscalationNameLists { get; set; }
        public virtual DbSet<EscalationRule> EscalationRules { get; set; }
        public virtual DbSet<IncidentDet> IncidentDets { get; set; }
        public virtual DbSet<IssueSummary> IssueSummaries { get; set; }
        public virtual DbSet<WeeklyAlarmNameList> WeeklyAlarmNameLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { 
                //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=cnwuxm1medb01;Database=EC;Username=ECUser;Password=Jabil123");
            }
        }
        private string[] contains = { "eCalling", "FPY", "Downtime System" }; //"Sparepart", 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "English_United States.1252");

            modelBuilder.Entity<EscalationNameList>(entity =>
            {
                entity.HasKey(e => new { e.Department, e.Project, e.Email })
                    .HasName("escalation_name_list_pkey");

                entity.ToTable("escalation_name_list");

                entity.Property(e => e.Department)
                    .HasMaxLength(32)
                    .HasColumnName("department");

                entity.Property(e => e.Project)
                    .HasMaxLength(32)
                    .HasColumnName("project");

                entity.Property(e => e.Email)
                    .HasMaxLength(64)
                    .HasColumnName("email");

                entity.Property(e => e.Chinesename)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("chinesename");

                entity.Property(e => e.Contacttype)
                    .HasMaxLength(64)
                    .HasColumnName("contacttype");

                entity.Property(e => e.Englishname)
                    .HasMaxLength(32)
                    .HasColumnName("englishname");

                entity.Property(e => e.Jobtitle)
                    .HasMaxLength(32)
                    .HasColumnName("jobtitle");

                entity.Property(e => e.Level).HasColumnName("level");
            });

            modelBuilder.Entity<EscalationRule>(entity =>
            {
                entity.HasKey(e => new { e.Department, e.Project, e.Level })
                    .HasName("escalation_rule_pkey");

                entity.ToTable("escalation_rule");

                entity.Property(e => e.Department)
                    .HasMaxLength(32)
                    .HasColumnName("department");

                entity.Property(e => e.Project)
                    .HasMaxLength(32)
                    .HasColumnName("project");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Timespan).HasColumnName("timespan");
            });

            modelBuilder.Entity<IncidentDet>(entity =>
            {
                entity.HasQueryFilter(e => e.Calcdowntime.Equals(true)); //只获取downtime事件
                entity.HasQueryFilter(e => Array.AsReadOnly(contains).Contains(e.Comefrom));  //只获取需要的系统传来的数据
                entity.ToTable("incident_det");

                entity.HasIndex(e => new { e.Comefrom, e.Occurtime }, "comefrom");

                entity.HasIndex(e => new { e.Machine, e.Incidentstatus, e.Occurtime }, "machine_incidentstatus");

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

            modelBuilder.Entity<IssueSummary>(entity =>
            {
                entity.ToTable("issue_summary");

                entity.HasIndex(e => new { e.Department, e.Project, e.Line, e.Station, e.Issue, e.Rootcause }, "issue_summary_department_project_line_station_issue_rootcau_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action)
                    .HasMaxLength(512)
                    .HasColumnName("action");

                entity.Property(e => e.Correctiveaction)
                    .HasMaxLength(512)
                    .HasColumnName("correctiveaction");

                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("department");

                entity.Property(e => e.Editor)
                    .HasMaxLength(32)
                    .HasColumnName("editor");

                entity.Property(e => e.Issue)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("issue");

                entity.Property(e => e.Lastupdatedate)
                    .HasColumnName("lastupdatedate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Line)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("line");

                entity.Property(e => e.Preventiveaction)
                    .HasMaxLength(512)
                    .HasColumnName("preventiveaction");

                entity.Property(e => e.Project)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("project");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Rootcause)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("rootcause");

                entity.Property(e => e.Station)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("station");

                entity.Property(e => e.Totaldowntime).HasColumnName("totaldowntime");

                entity.Property(e => e.Week)
                    .HasMaxLength(10)
                    .HasColumnName("week");
            });

            modelBuilder.Entity<WeeklyAlarmNameList>(entity =>
            {
                entity.HasKey(e => new { e.Project, e.Department, e.Email })
                    .HasName("weekly_alarm_name_list_pkey");

                entity.ToTable("weekly_alarm_name_list");

                entity.Property(e => e.Project)
                    .HasMaxLength(32)
                    .HasColumnName("project");

                entity.Property(e => e.Department)
                    .HasMaxLength(32)
                    .HasColumnName("department");

                entity.Property(e => e.Email)
                    .HasMaxLength(64)
                    .HasColumnName("email");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
