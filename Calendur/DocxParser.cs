using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Novacode;

namespace Calendur
{
    public class DocxParser
    {
        private readonly ParseContext _context = new ParseContext();

        private readonly Calendar _result ;

        public DocxParser(int year)
        {
            _result = new Calendar(year);
        }


        public Calendar ParseYear(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                return ParseYear(file);
            }
        }

        public Calendar ParseYear(Stream stram)
        {
            var docx = DocX.Load(stram);

            foreach (Paragraph paragraph in docx.Paragraphs)
            {
                _context.CurrentParagraph = paragraph;
                _context.CurrentText = paragraph.Text.Trim();

                if (TryUpdateDay() || TryParsePerson())
                { }
            }

            return _result;
        }

        private bool TryUpdateDay()
        {
            var newDay = DayTitle.TryParse(_context.CurrentText);

            if (newDay == null)
            {
                return false;
            }

            _context.CurrentDay = _result.AddDay(new DateTime(_result.Year, newDay.Month, newDay.Day));

            return true;
        }

        private bool TryParsePerson()
        {
            if (_context.CurrentDay == null)
            {
                return false;
            }

            Person person = PersonParser.TryParse(_context.CurrentText, _context.CurrentParagraph.Xml);

            if (person == null)
            {
                return false;
            }

            _context.CurrentDay.Persons.Add(person);

            return true;
        }
    }

    internal class ParseContext
    {
        public Day CurrentDay { get; set; }

        public Paragraph CurrentParagraph { get; set; }

        public string CurrentText { get; set; }
    }

    internal class DayTitle
    {
        public int Month { get; private set; }

        public int Day { get; private set; }


        public static readonly List<string> MonthPatterns = new List<string>
        {
            "января",
            "февраля",
            "марта",
            "апреля",
            "мая",
            "июня",
            "июля",
            "августа",
            "сентября",
            "октября",
            "ноября",
            "декабря"
        };


        private static readonly Regex Checker = new Regex(@"^(\d+?)\W+?(" + String.Join("|", MonthPatterns.Select( p=> "(" + p  + ")")) + ")$", RegexOptions.IgnoreCase | RegexOptions.Compiled);


        public static DayTitle TryParse(string str)
        {
            var match = Checker.Match(str);

            if (!match.Success) return null;

            return new DayTitle
            {
                Day = int.Parse(match.Groups[1].Value),
                Month = MonthPatterns.IndexOf(match.Groups[2].Value) + 1 
            };
        }
    }

    internal class PersonParser
    {
        private static readonly Regex Checker = new Regex(@"^(.+)(\(\d+?\))(.+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static Person TryParse(string str, XElement xml)
        {
            var match = Checker.Match(str);

            if (!match.Success) return null;

            return new Person
            {
                Name = match.Groups[1].Value.Trim(),
                Year = match.Groups[2].Value.Trim(),
                Description = match.Groups[3].Value.Trim(),
                HasAnniversary = HasAnniversary(xml)
            };
        }

        private static bool HasAnniversary(XElement xml)
        {
            XNamespace w = XNamespace.Get("http://schemas.openxmlformats.org/wordprocessingml/2006/main") ;

            var elements = xml.Descendants(w + "color").Where(node=>node.Attribute(w + "val")?.Value != "000000");

            return elements.Any();
        }
    }
}
