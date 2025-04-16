using System.ComponentModel.DataAnnotations;

namespace TimeTableGeneratorDynamic.Models
{
    public class TimeTableInputModel
    {
        public int WorkingDays { get; set; }
        public int SubjectsPerDay { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalHours => WorkingDays * SubjectsPerDay;
    }
}
