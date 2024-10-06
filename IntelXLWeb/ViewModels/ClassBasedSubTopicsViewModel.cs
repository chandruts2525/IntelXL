using IntelXLDataAccess.Models;

namespace IntelXLWeb.ViewModels
{
    public class ClassBasedSubTopicsViewModel
    {
        public List<TopicMaster>? Topics { get; set; }
        public List<SubjectMaster>? Subjects { get; set; }
        public string? ClassName { get; set; }
        public string? SubjectName { get; set; }
        public int ClassId { get; set; }
        public bool IsEnabled { get; set; } = false;
        public List<int> ClassIds { get; set; }=new List<int>();
        public int AllowIndex { get; set; }
        public bool HasSubscription { get; set; } = false;
    }
}
