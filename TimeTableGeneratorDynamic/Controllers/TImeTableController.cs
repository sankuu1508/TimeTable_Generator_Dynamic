using Microsoft.AspNetCore.Mvc;
using TimeTableGeneratorDynamic.Models;

namespace TimeTableGeneratorDynamic.Controllers
{
    public class TImeTableController : Controller
    {
        public IActionResult Index()
        {
            return View(new TimeTableInputModel());
        }

        [HttpPost]
        public IActionResult SubmitInput(TimeTableInputModel model)
        {
            TempData["TotalHours"] = model.TotalHours;
            TempData["SubjectCount"] = model.TotalSubjects;
            TempData["WorkingDays"] = model.WorkingDays;
            TempData["SubjectsPerDay"] = model.SubjectsPerDay;
            return RedirectToAction("SubjectHours");
        }

        public IActionResult SubjectHours()
        {
            int subjectCount = Convert.ToInt32(TempData["SubjectCount"]);
            TempData.Keep();
            var list = new List<SubjectHourModel>();
            for (int i = 0; i < subjectCount; i++)
                list.Add(new SubjectHourModel());
            return View(list);
        }

        [HttpPost]
        public IActionResult Generate(List<SubjectHourModel> subjects)
        {
            int totalHours = Convert.ToInt32(TempData["TotalHours"]);
            int sumHours = subjects.Sum(s => s.Hours);

            if (sumHours != totalHours)
            {
                ModelState.AddModelError("", $"Total hours must be exactly {totalHours}");
                return View("SubjectHours", subjects);
            }

            int workingDays = Convert.ToInt32(TempData["WorkingDays"]);
            int subjectsPerDay = Convert.ToInt32(TempData["SubjectsPerDay"]);
            TempData.Clear();

            var timetable = GenerateTimeTable(subjects, workingDays, subjectsPerDay);
            return View("Result", timetable);
        }

        private List<List<string>> GenerateTimeTable(List<SubjectHourModel> subjects, int days, int perDay)
        {
            var timetable = new List<List<string>>();
            var subjectList = new List<string>();

            foreach (var subject in subjects)
                subjectList.AddRange(Enumerable.Repeat(subject.SubjectName, subject.Hours));

            var rnd = new Random();
            subjectList = subjectList.OrderBy(x => rnd.Next()).ToList();

            for (int i = 0; i < perDay; i++)
            {
                var row = new List<string>();
                for (int j = 0; j < days; j++)
                {
                    if (subjectList.Any())
                    {
                        row.Add(subjectList[0]);
                        subjectList.RemoveAt(0);
                    }
                    else
                    {
                        row.Add("");
                    }
                }
                timetable.Add(row);
            }
            return timetable;
        }
    }

}
