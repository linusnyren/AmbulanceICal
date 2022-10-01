using System;
using AmbulanceICal.Models;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace AmbulanceICal.Services
{
    public interface IICsService
    {
        string GenerateIcs(List<SchemaModel> schemaModels);
    }
    public class IcsService : IICsService
    {
        private readonly string authorText = "En tjänst av Linus Nyrén";
        private readonly string Url = "https://docs.google.com/spreadsheets/d/1MatU5bzu_DOBP2OX0kURe-hOgAa8SsFLAcZ5DCUr240/edit#gid=0";

        public string GenerateIcs(List<SchemaModel> schemaModels)
        {
            var calendar = new Calendar();

            foreach (var model in schemaModels)
            {
                CalendarEvent e = GetCalendarEvent(model);
                e.Description = GetDescription();
                calendar.Events.Add(e);
            }

            var serializer = new CalendarSerializer();
            return serializer.SerializeToString(calendar);
        }

        private string GetDescription()
        {
            var currentTimeInSweden = TimeZoneInfo.ConvertTime(
                DateTimeOffset.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("Europe/Stockholm")
                ).ToString("d/MM HH:mm");

            var description = $"{authorText} \nSenast uppdaterad {currentTimeInSweden} \nLänk till Excel schemat: {Url}";

            return description;
        }

        private static CalendarEvent GetCalendarEvent(SchemaModel model)
        {
            if (string.IsNullOrEmpty(model.WorkHours))
                return GetDayEvent(model);

            return GetEvent(model);
        }

        private static CalendarEvent GetEvent(SchemaModel model)
        {
            var startHour = model.WorkHours.Substring(0, 5);
            var endHour = model.WorkHours.Substring(6);

            DateTime start = GetDate(model, startHour);
            DateTime end = GetDate(model, endHour);

            if (start > end) //This indicates it's a night shift
                end = end.AddDays(1);

            return new CalendarEvent
            {
                Start = new CalDateTime(start),
                End = new CalDateTime(end),
                Summary = GetSummary(model)
            };
        }

        private static CalendarEvent GetDayEvent(SchemaModel model)
        {
            var description = GetSummary(model);
            return new CalendarEvent
            {
                Summary = $"{description} - CHECK EXCEL",
                Start = new CalDateTime(GetDate(model)),
                End = new CalDateTime(GetDate(model))
            };
        }

        private static string GetSummary(SchemaModel model)
        {
            return $"{model.Team} - {model.Shift.Value}";
        }

        private static DateTime GetDate(SchemaModel model, string hours = "", bool addOneDay = false)
        {
            var week = model.Week;
            var year = DateTime.Now.ToString("yyyy");
            var currentWeek = DateTime.Now.DayOfYear / 7;
            if (week < (currentWeek - 20))
                year = (int.Parse(year) + 1).ToString();

            var month = DateTime.ParseExact(
                $"01/01-{year}",
                "dd/MM-yyyy",
                System.Globalization.CultureInfo.InvariantCulture)
                .AddDays(week * 7)
                .Month;
            try
            {
                var date = GetAsDateTime(model, hours, year, month);
                if (addOneDay)
                    date = date.AddDays(1);
                return date;
            }
            catch (Exception e)
            {
                Console.WriteLine();
                DateTime date = GetAsDateTime(model, hours, year, month-1);
                if (addOneDay)
                    date = date.AddDays(1);
                return date;
            }
        }

        private static DateTime GetAsDateTime(SchemaModel model, string hours, string year, int month)
        {
            return DateTime.Parse($"{year}-{month}-{model.Day} {hours}");
        }
    }
}
