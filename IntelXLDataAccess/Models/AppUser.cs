using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class AppUser
{
    [JsonPropertyName("appUserId")]
    public int AppUserId { get; set; }
    [JsonPropertyName("userName")]
    public string UserName { get; set; } = null!;
    [JsonPropertyName("password")]
    public string? Password { get; set; }
    [JsonPropertyName("appRoleId")]
    public int? AppRoleId { get; set; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = null!;
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = null!;
    [JsonPropertyName("emailId")]
    public string EmailId { get; set; } = null!;
    [JsonPropertyName("updatedDttm")]
    public DateTime? UpdatedDttm { get; set; }
    [JsonPropertyName("updatedBy")]
    public int? UpdatedBy { get; set; }
    [JsonPropertyName("status")]
    public bool Status { get; set; } = true;
    [JsonPropertyName("isVerified")]
    public bool IsVerified { get; set; } = false;
    [JsonPropertyName("appRole")]
    public virtual AppRole? AppRole { get; set; }
    [JsonPropertyName("createdDttm")]
    public DateTime? CreatedDttm { get; set; }
    [JsonPropertyName("chatFroms")]
    public virtual ICollection<Chat> ChatFroms { get; set; } = new List<Chat>(); 
    [JsonPropertyName("chatTos")]
    public virtual ICollection<Chat> ChatTos { get; set; } = new List<Chat>();

    [JsonPropertyName("studentTutorScheduleAppUsers")]
    public virtual ICollection<StudentTutorSchedule> StudentTutorScheduleAppUsers { get; set; } = new List<StudentTutorSchedule>();
    [JsonPropertyName("studentTutorScheduleTutors")]
    public virtual ICollection<StudentTutorSchedule> StudentTutorScheduleTutors { get; set; } = new List<StudentTutorSchedule>();
    [JsonPropertyName("tutorCertifications")]
    public virtual ICollection<TutorCertification> TutorCertifications { get; set; } = new List<TutorCertification>();
    [JsonPropertyName("tutorDetails")]
    public virtual TutorDetail? TutorDetails { get; set; }
    [JsonPropertyName("tutorEducations")]
    public virtual ICollection<TutorEducation> TutorEducations { get; set; } = new List<TutorEducation>();
    [JsonPropertyName("tutorTimingConfigs")]
    public virtual ICollection<TutorTimingConfig> TutorTimingConfigs { get; set; } = new List<TutorTimingConfig>();
    [JsonPropertyName("userSubscriptions")]
    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
    [JsonPropertyName("userPayments")]
    public virtual ICollection<UserPayment> UserPayments { get; set; } = new List<UserPayment>();
    [JsonPropertyName("userLogin")]
    public virtual UserLogin? UserLogin { get; set; }
    [JsonPropertyName("annimationMasterCreatedByNavigations")]
    public virtual ICollection<AnnimationMaster> AnnimationMasterCreatedByNavigations { get; set; } = new List<AnnimationMaster>();
    [JsonPropertyName("annimationMasterUpdatedByNavigations")]
    public virtual ICollection<AnnimationMaster> AnnimationMasterUpdatedByNavigations { get; set; } = new List<AnnimationMaster>();
    [JsonPropertyName("answerMasterCreatedByNavigations")]
    public virtual ICollection<AnswerMaster> AnswerMasterCreatedByNavigations { get; set; } = new List<AnswerMaster>();
    [JsonPropertyName("answerMasterUpdatedByNavigations")]
    public virtual ICollection<AnswerMaster> AnswerMasterUpdatedByNavigations { get; set; } = new List<AnswerMaster>();
    [JsonPropertyName("appRoleCreatedByNavigations")]
    public virtual ICollection<AppRole> AppRoleCreatedByNavigations { get; set; } = new List<AppRole>();
    [JsonPropertyName("appRoleUpdatedByNavigations")]
    public virtual ICollection<AppRole> AppRoleUpdatedByNavigations { get; set; } = new List<AppRole>();
    [JsonPropertyName("choiceMasterCreatedByNavigations")]
    public virtual ICollection<ChoiceMaster> ChoiceMasterCreatedByNavigations { get; set; } = new List<ChoiceMaster>();
    [JsonPropertyName("choiceMasterUpdatedByNavigations")]
    public virtual ICollection<ChoiceMaster> ChoiceMasterUpdatedByNavigations { get; set; } = new List<ChoiceMaster>();
    [JsonPropertyName("classMasterCreatedByNavigations")]
    public virtual ICollection<ClassMaster> ClassMasterCreatedByNavigations { get; set; } = new List<ClassMaster>();
    [JsonPropertyName("classMasterUpdatedByNavigations")]
    public virtual ICollection<ClassMaster> ClassMasterUpdatedByNavigations { get; set; } = new List<ClassMaster>();
    [JsonPropertyName("couponMasterCreatedByNavigations")]
    public virtual ICollection<CouponMaster> CouponMasterCreatedByNavigations { get; set; } = new List<CouponMaster>();
    [JsonPropertyName("couponMasterUpdatedByNavigations")]
    public virtual ICollection<CouponMaster> CouponMasterUpdatedByNavigations { get; set; } = new List<CouponMaster>();
    [JsonPropertyName("courseMasterCreatedByNavigations")]
    public virtual ICollection<CourseMaster> CourseMasterCreatedByNavigations { get; set; } = new List<CourseMaster>();
    [JsonPropertyName("courseMasterUpdatedByNavigations")]
    public virtual ICollection<CourseMaster> CourseMasterUpdatedByNavigations { get; set; } = new List<CourseMaster>();
    [JsonPropertyName("dayMasterCreatedByNavigations")]
    public virtual ICollection<DayMaster> DayMasterCreatedByNavigations { get; set; } = new List<DayMaster>();
    [JsonPropertyName("dayMasterUpdatedByNavigations")]
    public virtual ICollection<DayMaster> DayMasterUpdatedByNavigations { get; set; } = new List<DayMaster>();
    [JsonPropertyName("languageMasterCreatedByNavigations")]
    public virtual ICollection<LanguageMaster> LanguageMasterCreatedByNavigations { get; set; } = new List<LanguageMaster>();
    [JsonPropertyName("languageMasterUpdatedByNavigations")]
    public virtual ICollection<LanguageMaster> LanguageMasterUpdatedByNavigations { get; set; } = new List<LanguageMaster>();
    [JsonPropertyName("languageOfInstructionMasterCreatedByNavigations")]
    public virtual ICollection<LanguageOfInstructionMaster> LanguageOfInstructionMasterCreatedByNavigations { get; set; } = new List<LanguageOfInstructionMaster>();
    [JsonPropertyName("languageOfInstructionMasterUpdatedByNavigations")]
    public virtual ICollection<LanguageOfInstructionMaster> LanguageOfInstructionMasterUpdatedByNavigations { get; set; } = new List<LanguageOfInstructionMaster>();
    [JsonPropertyName("paymentTypeMasterCreatedByNavigations")]
    public virtual ICollection<PaymentTypeMaster> PaymentTypeMasterCreatedByNavigations { get; set; } = new List<PaymentTypeMaster>();
    [JsonPropertyName("paymentTypeMasterUpdatedByNavigations")]
    public virtual ICollection<PaymentTypeMaster> PaymentTypeMasterUpdatedByNavigations { get; set; } = new List<PaymentTypeMaster>();
    [JsonPropertyName("questionMasterCreatedByNavigations")]
    public virtual ICollection<QuestionMaster> QuestionMasterCreatedByNavigations { get; set; } = new List<QuestionMaster>();
    [JsonPropertyName("questionMasterUpdatedByNavigations")]
    public virtual ICollection<QuestionMaster> QuestionMasterUpdatedByNavigations { get; set; } = new List<QuestionMaster>();
    [JsonPropertyName("subTopicMasterCreatedByNavigations")]
    public virtual ICollection<SubTopicMaster> SubTopicMasterCreatedByNavigations { get; set; } = new List<SubTopicMaster>();
    [JsonPropertyName("subTopicMasterUpdatedByNavigations")]
    public virtual ICollection<SubTopicMaster> SubTopicMasterUpdatedByNavigations { get; set; } = new List<SubTopicMaster>();
    [JsonPropertyName("subjectMasterCreatedByNavigations")]
    public virtual ICollection<SubjectMaster> SubjectMasterCreatedByNavigations { get; set; } = new List<SubjectMaster>();
    [JsonPropertyName("subjectMasterUpdatedByNavigations")]
    public virtual ICollection<SubjectMaster> SubjectMasterUpdatedByNavigations { get; set; } = new List<SubjectMaster>();
    [JsonPropertyName("subscriptionMasterCreatedByNavigations")]
    public virtual ICollection<SubscriptionMaster> SubscriptionMasterCreatedByNavigations { get; set; } = new List<SubscriptionMaster>();
    [JsonPropertyName("subscriptionMasterUpdatedByNavigations")]
    public virtual ICollection<SubscriptionMaster> SubscriptionMasterUpdatedByNavigations { get; set; } = new List<SubscriptionMaster>();
    [JsonPropertyName("timeZoneMasterCreatedByNavigations")]
    public virtual ICollection<TimeZoneMaster> TimeZoneMasterCreatedByNavigations { get; set; } = new List<TimeZoneMaster>();
    [JsonPropertyName("timeZoneMasterUpdatedByNavigations")]
    public virtual ICollection<TimeZoneMaster> TimeZoneMasterUpdatedByNavigations { get; set; } = new List<TimeZoneMaster>();
    [JsonPropertyName("timingMasterCreatedByNavigations")]
    public virtual ICollection<TimingMaster> TimingMasterCreatedByNavigations { get; set; } = new List<TimingMaster>();
    [JsonPropertyName("timingMasterUpdatedByNavigations")]
    public virtual ICollection<TimingMaster> TimingMasterUpdatedByNavigations { get; set; } = new List<TimingMaster>();
    [JsonPropertyName("topicMasterCreatedByNavigations")]
    public virtual ICollection<TopicMaster> TopicMasterCreatedByNavigations { get; set; } = new List<TopicMaster>();
    [JsonPropertyName("topicMasterUpdatedByNavigations")]
    public virtual ICollection<TopicMaster> TopicMasterUpdatedByNavigations { get; set; } = new List<TopicMaster>();
    [JsonPropertyName("unitMasterCreatedByNavigations")]
    public virtual ICollection<UnitMaster> UnitMasterCreatedByNavigations { get; set; } = new List<UnitMaster>();
    [JsonPropertyName("unitMasterUpdatedByNavigations")]
    public virtual ICollection<UnitMaster> UnitMasterUpdatedByNavigations { get; set; } = new List<UnitMaster>();
   
}

public class PagedUsers
{
    public List<AppUser>? Users { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
}