using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ical;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System.IO;
using StateHolidayIcsCreator.Classes;

namespace StateHolidayIcsCreator
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("Welcome to the State Holiday .ics file creator.");
                Console.WriteLine("Please enter the year for which you would like to create the .ics file.");
                string year = Console.ReadLine();
                bool isValidYear = int.TryParse(year, out int iYear);
                while (!isValidYear || iYear > (DateTime.Now.Year + 5) || iYear < (DateTime.Now.Year - 2))
                {
                    Console.WriteLine("I'm sorry but that's an invalid year. Try again");
                    year = Console.ReadLine();
                    isValidYear = int.TryParse(year, out iYear);
                }
                Console.WriteLine("Where would you like the .ics file to be saved?");
                Console.WriteLine("1 = Desktop");
                Console.WriteLine("2 = Documents Folder");
                string saveLocation = Console.ReadLine();
                while (saveLocation != "1" && saveLocation != "2")
                {
                    Console.WriteLine("I'm sorry but that's an invalid option. Try again");
                    saveLocation = Console.ReadLine();
                }
                string filePath = "";
                if (saveLocation == "1")
                {
                    filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                else
                {
                    filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                filePath += @"\State Holidays (" + year + ").ics";
                var calendar = new Calendar();
                var events = GetAnnualHolidays.GetHolidayEvents(iYear);
                foreach (var e in events)
                {
                    calendar.Events.Add(e);
                }

                var serializer = new CalendarSerializer(new SerializationContext());
                var serializedCalendar = serializer.SerializeToString(calendar);
                File.WriteAllText(filePath, serializedCalendar);
                Console.WriteLine("File has been saved");
                Console.WriteLine("To import these holidays to your Outlook calendar, click \"File\", \"Open\", \"Import\" and then select \"Import an iCalendar (.ics) or vCalendar file (.vcs).");
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error has occurred");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
            
        }
    }
}
