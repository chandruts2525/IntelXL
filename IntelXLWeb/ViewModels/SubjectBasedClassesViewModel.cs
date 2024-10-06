using IntelXLDataAccess.Models;

namespace IntelXLWeb.ViewModel
{
    public class SubjectBasedClassesViewModel
    {
        public List<ClassMaster>? Classes { get; set; }
        public List<SubjectMaster>? Subjects { get; set; }      
        public Dictionary<ClassMaster, List<SubTopicMaster>>? SubTopics { get; set; }
        public string? SubjectName { get; set; }
    }
}
