using IntelXLDataAccess.Models;

using System.Text.Json.Serialization;

namespace IntelXLWeb.Models
{
    public class TimeConfigViewModel
    {
        public int TutorId { get; set; }
        public List<DayConfigViewModel>? TimeConfigs { get; set; }
    }

    public class DayConfigViewModel
    {
        public int DayId { get; set; }
        public List<TimeSlotViewModel>? TimeSlots { get; set; }
    }

    public class TimeSlotViewModel
    {
        public int FromTimeId { get; set; }
        public int ToTimeId { get; set; }
    }
}
