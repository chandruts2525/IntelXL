using IntelXLDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace IntelXLDataAccess.Data;

public partial class IntelxlContext : DbContext
{
    public IntelxlContext(DbContextOptions<IntelxlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnnimationMaster> AnnimationMasters { get; set; }

    public virtual DbSet<AnswerMaster> AnswerMasters { get; set; }

    public virtual DbSet<AppRole> AppRoles { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }
    public virtual DbSet<Chat> Chats { get; set; }
    public virtual DbSet<ChoiceMaster> ChoiceMasters { get; set; }

    public virtual DbSet<ClassMaster> ClassMasters { get; set; }

    public virtual DbSet<CourseMaster> CourseMasters { get; set; }
    public virtual DbSet<CouponMaster> CouponMasters { get; set; }
    public virtual DbSet<DayMaster> DayMasters { get; set; }
    public virtual DbSet<LanguageMaster> LanguageMasters { get; set; }

    public virtual DbSet<LanguageOfInstructionMaster> LanguageOfInstructionMasters { get; set; }

    public virtual DbSet<PaymentTypeMaster> PaymentTypeMasters { get; set; }

    public virtual DbSet<QuestionMaster> QuestionMasters { get; set; }

    public virtual DbSet<SubTopicMaster> SubTopicMasters { get; set; }
    public virtual DbSet<StudentTutorSchedule> StudentTutorSchedules { get; set; }
    public virtual DbSet<SubjectMaster> SubjectMasters { get; set; }

    public virtual DbSet<SubscriptionMaster> SubscriptionMasters { get; set; }
    public virtual DbSet<TimeZoneMaster> TimeZoneMasters { get; set; }
    public virtual DbSet<TimingMaster> TimingMasters { get; set; }

    public virtual DbSet<TopicMaster> TopicMasters { get; set; }
    public virtual DbSet<TutorCertification> TutorCertifications { get; set; }
    public virtual DbSet<TutorDetail> TutorDetails { get; set; }
    public virtual DbSet<TutorEducation> TutorEducations { get; set; }
    public virtual DbSet<TutorTimingConfig> TutorTimingConfigs { get; set; }
    public virtual DbSet<UnitMaster> UnitMasters { get; set; }
    public virtual DbSet<UserExam> UserExams { get; set; }
    public virtual DbSet<UserLogin> UserLogins { get; set; }
    public virtual DbSet<UserPayment> UserPayments { get; set; }

    public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnnimationMaster>(entity =>
        {
            entity.HasKey(e => e.AnnimationId);

            entity.ToTable("AnnimationMaster");

            entity.Property(e => e.AnnimationUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AnnimationMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Annimatio__Creat__607251E5");

            entity.HasOne(d => d.Question).WithMany(p => p.AnnimationMasters)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnnimationMaster_QuestionMaster");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AnnimationMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Annimatio__Updat__6166761E");
        });

        modelBuilder.Entity<AnswerMaster>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__AnswerMa__D4825004782AE201");

            entity.ToTable("AnswerMaster");
            entity.Property(e => e.Answer).HasMaxLength(500).IsUnicode(true);
            entity.Property(e => e.Description).HasMaxLength(1000).IsUnicode(true);

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AnswerMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__AnswerMas__Creat__625A9A57");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AnswerMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__AnswerMas__Updat__634EBE90");
        });

        modelBuilder.Entity<AppRole>(entity =>
        {
            entity.ToTable("AppRole");

            entity.Property(e => e.AppRoleId).HasColumnName("AppRoleID");
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AppRoleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__AppRole__Created__46B27FE2");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AppRoleUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__AppRole__Updated__47A6A41B");
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("AppUser");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.EmailId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(100).IsUnicode(false);            
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.AppRole).WithMany(p => p.AppUsers)
                .HasForeignKey(d => d.AppRoleId)
                .HasConstraintName("FK__AppUser__AppRole__619B8048");

        });
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.Property(e => e.ConversationId)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FileName).HasMaxLength(100);
            entity.Property(e => e.MediaUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SentAt).HasColumnType("datetime");

            entity.HasOne(d => d.FromUser).WithMany(p => p.ChatFroms)
                .HasForeignKey(d => d.FromId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chats_FromUser");

            entity.HasOne(d => d.ToUser).WithMany(p => p.ChatTos)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chats_ToUser");
        });

        modelBuilder.Entity<ChoiceMaster>(entity =>
        {
            entity.HasKey(e => e.ChoiceId);

            entity.ToTable("ChoiceMaster");
            entity.Property(e => e.Choice).HasMaxLength(500).IsUnicode(true);

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ChoiceMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__ChoiceMas__Creat__6442E2C9");

            entity.HasOne(d => d.Question).WithMany(p => p.ChoiceMasters)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChoiceMaster_QuestionMaster");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ChoiceMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__ChoiceMas__Updat__65370702");
        });

        modelBuilder.Entity<ClassMaster>(entity =>
        {
            entity.HasKey(e => e.ClassId);

            entity.ToTable("ClassMaster");

            entity.Property(e => e.ClassName)
            .HasMaxLength(100)
            .IsUnicode(true); 
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.ClassMasters)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassMaster_CourseMaster");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClassMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__ClassMast__Creat__662B2B3B");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ClassMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__ClassMast__Updat__671F4F74");
        });

        modelBuilder.Entity<CouponMaster>(entity =>
        {
            entity.ToTable("CouponMaster");

            entity.Property(e => e.CouponCode).HasMaxLength(50);
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CouponMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__CouponMas__Creat__681373AD");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CouponMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__CouponMas__Updat__690797E6");
        });

        modelBuilder.Entity<CourseMaster>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__CourseMa__C92D71876114E61A");

            entity.ToTable("CourseMaster");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseDuration).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CourseName)
            .HasMaxLength(50)
            .IsUnicode(true);
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.LanguageId).HasColumnName("Language_Id");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CourseMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__CourseMas__Creat__55F4C372");

            entity.HasOne(d => d.LanguageOfInstruction).WithMany(p => p.CourseMasters)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("FK__CourseMas__Langu__6FB49575");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CourseMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__CourseMas__Updat__56E8E7AB");
        });

        modelBuilder.Entity<DayMaster>(entity =>
        {
            entity.HasKey(e => e.DayId);

            entity.ToTable("DayMaster");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.DayName)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DayMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__DayMaster__Creat__69FBBC1F");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DayMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__DayMaster__Updat__6AEFE058");
        });

        modelBuilder.Entity<LanguageMaster>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__tmp_ms_x__B93855ABC58E3DB0");

            entity.ToTable("LanguageMaster");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LanguageMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__LanguageM__Creat__6BE40491");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LanguageMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__LanguageM__Updat__6CD828CA");
        });

        modelBuilder.Entity<LanguageOfInstructionMaster>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__Language__B7596FF3993145FB");

            entity.ToTable("LanguageOfInstructionMaster");

            entity.Property(e => e.LanguageId).HasColumnName("Language_Id");
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LanguageOfInstructionMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__LanguageO__Creat__6DCC4D03");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LanguageOfInstructionMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__LanguageO__Updat__6EC0713C");
        });

        modelBuilder.Entity<PaymentTypeMaster>(entity =>
        {
            entity.HasKey(e => e.PaymentId);

            entity.ToTable("PaymentTypeMaster");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PaymentTypeMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__PaymentTy__Creat__57DD0BE4");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PaymentTypeMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__PaymentTy__Updat__58D1301D");
        });

        modelBuilder.Entity<QuestionMaster>(entity =>
        {
            entity.HasKey(e => e.QuestionId);

            entity.ToTable("QuestionMaster");

            entity.Property(e => e.Question).HasMaxLength(1000).IsUnicode(true);


            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");
            entity.Property(e => e.PreviousYear)
               .HasMaxLength(100)
               .IsUnicode(false);
            entity.Property(e => e.ProbableYear)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.HasOne(d => d.Answer).WithMany(p => p.QuestionMasters)
                .HasForeignKey(d => d.AnswerId)
                .HasConstraintName("FK_QuestionMaster_AnswerMaster");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.QuestionMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__QuestionM__Creat__6FB49575");

            entity.HasOne(d => d.SubTopic).WithMany(p => p.QuestionMasters)
                .HasForeignKey(d => d.SubTopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionMaster_SubTopicMaster");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.QuestionMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__QuestionM__Updat__70A8B9AE");
        });

        modelBuilder.Entity<StudentTutorSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK_TutorSchedule");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.ScheduledDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.AppUser).WithMany(p => p.StudentTutorScheduleAppUsers)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorSchedule_Student");

            entity.HasOne(d => d.FromTime).WithMany(p => p.StudentTutorScheduleFromTimes)
                .HasForeignKey(d => d.FromTimeId)
                .HasConstraintName("FK_StudentTutorSchedules_TimingMaster");

            entity.HasOne(d => d.ToTime).WithMany(p => p.StudentTutorScheduleToTimes)
                .HasForeignKey(d => d.ToTimeId)
                .HasConstraintName("FK_StudentTutorSchedules_TimingMaster1");

            entity.HasOne(d => d.Tutor).WithMany(p => p.StudentTutorScheduleTutors)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorSchedule_Tutor");

            entity.HasOne(d => d.TutorTimingConfig).WithMany(p => p.StudentTutorSchedules)
                .HasForeignKey(d => d.TutorTimingConfigId)
                .HasConstraintName("FK_TutorSchedule_TimeConfig");
        });

        modelBuilder.Entity<SubTopicMaster>(entity =>
        {
            entity.HasKey(e => e.SubTopicId);

            entity.ToTable("SubTopicMaster");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.SubTopic)
                            .HasMaxLength(500)
                            .IsUnicode(true);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SubTopicMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__SubTopicM__Creat__73852659");

            entity.HasOne(d => d.Topic).WithMany(p => p.SubTopicMasters)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubTopicMaster_TopicMaster");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SubTopicMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__SubTopicM__Updat__74794A92");
        });

        modelBuilder.Entity<SubjectMaster>(entity =>
        {
            entity.HasKey(e => e.SubjectId);

            entity.ToTable("SubjectMaster");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.SubjectName)
            .HasMaxLength(100)
            .IsUnicode(true);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.SubjectMasters)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubjectMaster_ClassMaster");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SubjectMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__SubjectMa__Creat__719CDDE7");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SubjectMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__SubjectMa__Updat__72910220");
        });

        modelBuilder.Entity<SubscriptionMaster>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B249DAAD698D7");

            entity.ToTable("SubscriptionMaster");

            entity.Property(e => e.Coupon).HasMaxLength(25);
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.CurrencyType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SubscriptionAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SubscriptionDetails)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SubscriptionName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.Class).WithMany(p => p.SubscriptionMasters)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubscriptionMaster_ClassMaster");

            entity.HasOne(d => d.Course).WithMany(p => p.SubscriptionMasters)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubscriptionMaster_CourseMaster");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SubscriptionMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Subscript__Creat__5E8A0973");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SubscriptionMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Subscript__Updat__5F7E2DAC");
        });

        modelBuilder.Entity<TimeZoneMaster>(entity =>
        {
            entity.HasKey(e => e.TimezoneId);

            entity.ToTable("TimeZoneMaster");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.TimezoneName).IsUnicode(false);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");
            entity.Property(e => e.UtcOffSet)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TimeZoneMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__TimeZoneM__Creat__756D6ECB");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TimeZoneMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__TimeZoneM__Updat__76619304");
        });

        modelBuilder.Entity<TimingMaster>(entity =>
        {
            entity.HasKey(e => e.TimingId);

            entity.ToTable("TimingMaster");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.Timing)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TimingMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__TimingMas__Creat__7755B73D");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TimingMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__TimingMas__Updat__7849DB76");
        });

        modelBuilder.Entity<TopicMaster>(entity =>
        {
            entity.HasKey(e => e.TopicId);

            entity.ToTable("TopicMaster");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.Topic)
            .HasMaxLength(200)
            .IsUnicode(true);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TopicMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__TopicMast__Creat__793DFFAF");

            entity.HasOne(d => d.Unit).WithMany(p => p.TopicMasters)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK_TopicMaster_UnitMaster");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TopicMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__TopicMast__Updat__7A3223E8");
        });

        modelBuilder.Entity<TutorCertification>(entity =>
        {
            entity.HasKey(e => e.CertificateId);

            entity.ToTable("TutorCertification");

            entity.Property(e => e.CertificateName).HasMaxLength(100);
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.IssuedBy)
                .HasMaxLength(100)
                .HasColumnName("Issued_by");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");
            entity.Property(e => e.YearsOfStudy)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Years_of_study");

            entity.HasOne(d => d.AppUser).WithMany(p => p.TutorCertifications)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorCertification_TutorDetails");
        });

        modelBuilder.Entity<TutorDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId);

            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LanguagesSpeak)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.LanguagesSpeak).IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Pricing).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProfileUrl).IsUnicode(false);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");
            entity.Property(e => e.VideoUrl).IsUnicode(false);
            entity.HasOne(d => d.AppUser)
            .WithOne(p => p.TutorDetails)
               .HasForeignKey<TutorDetail>(d => d.TutorId);
        });

        modelBuilder.Entity<TutorEducation>(entity =>
        {
            entity.HasKey(e => e.EducationId);

            entity.ToTable("TutorEducation");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.Degree).HasMaxLength(100);
            entity.Property(e => e.Specialization).HasMaxLength(100);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");
            entity.Property(e => e.YearsOfStudy)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Years_of_study");

            entity.HasOne(d => d.AppUser).WithMany(p => p.TutorEducations)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorEducation_TutorDetails");
        });

        modelBuilder.Entity<TutorTimingConfig>(entity =>
        {
            entity.HasKey(e => e.TutorTimingId);

            entity.ToTable("TutorTimingConfig");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.Day).WithMany(p => p.TutorTimingConfigs)
                .HasForeignKey(d => d.DayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorTimingConfig_DayMaster");

            entity.HasOne(d => d.FromTime).WithMany(p => p.TutorTimingConfigFromTimes)
                .HasForeignKey(d => d.FromTimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorTimingConfig_FromTimingMaster");

            entity.HasOne(d => d.ToTime).WithMany(p => p.TutorTimingConfigToTimes)
                .HasForeignKey(d => d.ToTimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorTimingConfig_ToTimingMaster");

            entity.HasOne(d => d.Tutor).WithMany(p => p.TutorTimingConfigs)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TutorTimingConfig_Tutor");
        });

        modelBuilder.Entity<UnitMaster>(entity =>
        {
            entity.HasKey(e => e.UnitId);

            entity.ToTable("UnitMaster");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.UnitName)
            .HasMaxLength(100)
            .IsUnicode(true);
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UnitMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__UnitMaste__Creat__7B264821");

            entity.HasOne(d => d.Subject).WithMany(p => p.UnitMasters)
                .HasForeignKey(d => d.Subjectid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitMaster_SubjectMaster");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.UnitMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__UnitMaste__Updat__7C1A6C5A");
        });

        modelBuilder.Entity<UserExam>(entity =>
        {
            entity.ToTable("UserExam");
            entity.Property(e => e.PracticeType)
               .HasMaxLength(20)
               .IsUnicode(false);          
            entity.Property(e => e.YearOfQuestion)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDttm).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.UserLoginId).HasName("PK__UserLogi__107D568C27AFB3EE");

            entity.ToTable("UserLogin");

            entity.HasIndex(e => e.UserId, "UC_UserLogin_UserId").IsUnique();

            entity.Property(e => e.LoginDttm).HasColumnType("datetime");
            entity.Property(e => e.LogoutDttm).HasColumnType("datetime");

            entity.HasOne(d => d.AppUser).WithOne(p => p.UserLogin)
                .HasForeignKey<UserLogin>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLogin_AppUser");
        });

        modelBuilder.Entity<UserPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId);

            entity.ToTable("UserPayment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(18, 2)"); 
            entity.Property(e => e.InitialAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Contact).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.RecurringMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResponsePaymentId).HasMaxLength(50);

            entity.HasOne(d => d.AppUser).WithMany(p => p.UserPayments)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPayment_AppUser");
            entity.HasOne(d => d.Coupon).WithMany(p => p.UserPayments)
               .HasForeignKey(d => d.CouponId)
               .HasConstraintName("FK__UserPayme__Coupo__0EF836A4");
        });

        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.ToTable("UserSubscription");

            entity.Property(e => e.CreatedDttm).HasColumnType("datetime");
            entity.Property(e => e.ExpireDttm).HasColumnType("datetime");
            entity.Property(e => e.SubscriptionType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.AppUser).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSubsc__AppUs__29221CFB");

            entity.HasOne(d => d.Subscription).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSubsc__Subsc__2A164134");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
