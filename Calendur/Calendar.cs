using System;
using System.Collections.Generic;
using System.Globalization;

namespace Calendur
{
    public class Calendar
    {
        public Calendar(int year)
        {
            Year = year;
        }

        public int Year { get; }

        public IDictionary<DateTime, Day> Days { get; protected set; } = new Dictionary<DateTime, Day>();

        public Day AddDay(DateTime date)
        {
            var day = new Day
            {
                Date = date.Date,
                Persons = new List<Person>()
            };

            Days.Add(day.Date, day);

            return day;
        }

        public IReadOnlyCollection<Week> GetByWeeks()
        {
            var weeks = new List<Week>(366 / 7 + 1);
            Week currentWeek = null;

            var currentDate = new DateTime(Year, 1, 1);

            do
            {
                if (currentDate.DayOfWeek == DayOfWeek.Monday ||
                    currentWeek == null)
                {
                    var dateForMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - currentDate.Day < 3 ? currentDate.AddMonths(1) : currentDate;
                    currentWeek = new Week
                    {
                        Month = dateForMonth.ToString("MMMM", new CultureInfo("ru"))
                    };
                    weeks.Add(currentWeek);
                }

                var dayIndex = GetDayOfWeekIndex(currentDate.DayOfWeek);
                Day day;
                if (Days.TryGetValue(currentDate, out day))
                {
                    currentWeek.Days[dayIndex] = day;
                }
                currentWeek.Dates[dayIndex] = currentDate.Date;

                currentDate = currentDate.AddDays(1);
            } while (currentDate.Year == Year);

            return weeks;
        }

        private int GetDayOfWeekIndex(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Sunday ? 6 : (int) dayOfWeek - 1;
        }
    }

    public class Week
    {
        public string Month { get; set; }

        public Day[] Days { get; } = new Day[7];

        public DateTime?[] Dates { get; } = new DateTime?[7];
    }

    public class Day
    {
        public DateTime Date { get; set; }

        public List<Person> Persons { get; set; }
    }

    public class Person
    {
        public string Name { get; set; }

        public string Year { get; set; }

        public string Description { get; set; }

        public bool HasAnniversary { get; set; }
    }
}
