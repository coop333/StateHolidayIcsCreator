using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;

namespace StateHolidayIcsCreator.Classes
{
    public class GetAnnualHolidays
    {
        public static List<CalendarEvent> GetHolidayEvents(int year)
        {
            DateTime dtEvent;
            string summary;
            var list = new List<CalendarEvent>();

            summary = "New Year's day";
            dtEvent = new DateTime(year, 1, 1);
            AddEvent(ref list, summary, dtEvent);
            FixEventsThatFallOnWeekend(ref list, summary, dtEvent, false);            

            summary = "Martin Luther King Jr. Day";
            //Third monday in January
            dtEvent = GetFirstDayOfWeekForMonth(year, 1, DayOfWeek.Monday);
            dtEvent = dtEvent.AddDays(14);
            AddEvent(ref list, summary, dtEvent);

            summary = "President's Day";
            //Third monday in February
            dtEvent = GetFirstDayOfWeekForMonth(year, 2, DayOfWeek.Monday);
            dtEvent = dtEvent.AddDays(14);
            AddEvent(ref list, summary, dtEvent);

            summary = "Cesar Chavez Day";
            //March 31st
            dtEvent = new DateTime(year, 3, 31);
            AddEvent(ref list, summary, dtEvent);
            FixEventsThatFallOnWeekend(ref list, summary, dtEvent, false);

            summary = "Memorial Day";
            //Last Monday in May
            dtEvent = GetLastDayOfWeekForMonth(year, 5, DayOfWeek.Monday);
            AddEvent(ref list, summary, dtEvent);

            summary = "Independence Day";
            dtEvent = new DateTime(year, 7, 4);
            AddEvent(ref list, summary, dtEvent);
            FixEventsThatFallOnWeekend(ref list, summary, dtEvent, false);

            summary = "Labor Day";
            //First Monday in September
            dtEvent = GetFirstDayOfWeekForMonth(year, 9, DayOfWeek.Monday);
            AddEvent(ref list, summary, dtEvent);

            summary = "Veteran's Day";
            dtEvent = new DateTime(year, 11, 11);
            AddEvent(ref list, summary, dtEvent);
            FixEventsThatFallOnWeekend(ref list, summary, dtEvent, true); //Notice this is true

            summary = "Thanksgiving Day";
            dtEvent = GetThanksgiving(year);
            AddEvent(ref list, summary, dtEvent);

            summary = "Day After Thanksgiving";
            dtEvent = dtEvent.AddDays(1);
            AddEvent(ref list, summary, dtEvent);

            summary = "Christmas";
            dtEvent = new DateTime(year, 12, 25);
            AddEvent(ref list, summary, dtEvent);
            FixEventsThatFallOnWeekend(ref list, summary, dtEvent, false);

            return list;
        }
        private static void FixEventsThatFallOnWeekend(ref List<CalendarEvent> list, string summary, DateTime dt, bool isVeteransDay)
        {
            //http://www.calhr.ca.gov/employees/Pages/holidays.aspx
            summary += " (Observed)";
            if (dt.DayOfWeek == DayOfWeek.Saturday && isVeteransDay)
            {
                AddEvent(ref list, summary, dt.AddDays(-1));
            }
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                AddEvent(ref list, summary, dt.AddDays(1));
            }
            else
            {
                return;
            }
        }

        private static void AddEvent(ref List<CalendarEvent> e, string summary, DateTime dtEvent)
        {
            e.Add(new CalendarEvent
            {
                Summary = summary,
                DtStart = new CalDateTime(dtEvent.Date),
                DtEnd = new CalDateTime(dtEvent.Date)
            });
        }

        private static DateTime GetLastDayOfWeekForMonth(int year, int month, DayOfWeek day)
        {
            DateTime firstDayOfWeek = GetFirstDayOfWeekForMonth(year, month, day);
            DateTime result = firstDayOfWeek;
            for (int i = 0; i < 5; i++)
            {
                //loop through and get the highest date while staying within current month
                DateTime hold = result.AddDays(7);
                if(hold.Month == month)
                {
                    result = hold;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private static DateTime GetFirstDayOfWeekForMonth(int year, int month, DayOfWeek dayOfWeek)
        {            
            DateTime firstMonthDay = new DateTime(year, month, 1);
            return firstMonthDay.AddDays((dayOfWeek + 7 - firstMonthDay.DayOfWeek) % 7);
        }

        private static DateTime GetThanksgiving(int year)
        {
            DateTime firstDayOfWeek = GetFirstDayOfWeekForMonth(year, 11, DayOfWeek.Thursday);
            return firstDayOfWeek.AddDays(21);
        }
    }
}
