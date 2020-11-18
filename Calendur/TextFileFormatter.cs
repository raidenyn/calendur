using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Calendur
{
    public class TextFileFormatter
    {
        private readonly Calendar _calendar;

        private static readonly string[] Headers =
        {
            "понедельник-текст",
            "вторник-текст",
            "среда-текст",
            "четверг-текст",
            "пятница-текст",
            "суббота-текст",
            "воскресенье-текст",
            "понедельник-число",
            "вторник-число",
            "среда-число",
            "четверг-число",
            "пятница-число",
            "суббота-число",
            "воскресенье-число",
            "месяц"
        };

        public TextFileFormatter(Calendar calendar)
        {
            _calendar = calendar;
        }

        public void SaveToFile(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var writer = new StreamWriter(file, Encoding.Unicode))
                {
                    SaveTo(writer);
                }
            }
        }

        public void SaveTo(TextWriter writer)
        {
            writer.WriteLine(String.Join("\t", Headers));

            IReadOnlyCollection<Week> weeks = _calendar.GetByWeeks();

            foreach (var week in weeks)
            {
                var dataTexts = week.Days.Select(FormatDay);
                writer.Write(String.Join("\t", dataTexts));

                writer.Write("\t");

                var dateTexts = week.Dates.Select(FormatDate);
                writer.Write(String.Join("\t", dateTexts));

                writer.Write("\t");

                writer.Write(week.Month);

                writer.WriteLine();
            }
        }


        private string FormatDate(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return String.Empty;
            }

            return dateTime.Value.Day.ToString();
        }

        private string FormatDay(Day day)
        {
            if (day == null)
            {
                return String.Empty;
            }

            var personTexts = day.Persons.Select(FormatPerson);

            return String.Join("||", personTexts);
        }

        private string FormatPerson(Person person)
        {
            var a = person.HasAnniversary ? "!" : "";

            return $"~{a}name~{person.Name}|~{a}info~{person.Year} {person.Description}";
        }
    }
}
