using MidiDomotica.TimedEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.Core.Handlers
{
    public static class TimedEventsHandler
    {
        public static async Task Init(Type defaultExecutionType)
        {
            await TimedEventsManager.Init(defaultExecutionType);
        }

        public static async Task<Dictionary<string, IEnumerable<DateTimeOffset>>> GetUpcomingEvents()
        {
            return await TimedEventsManager.GetUpcomingEvents();
        }

        public static async Task<bool> RemoveTimedEvent(string group, string name)
        {
            return await TimedEventsManager.RemoveEvent(group, name);
        }

        public static async Task<string> AddOnceTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year)
        {
            return await TimedEventsManager.AddJob(TimedEventType.Once, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year);
        }

        public static async Task<string> AddWeekScheduleTimedEvent(string actionGroup, string methodStr, int minute, int hour, int dayOfMonth, int month, int year, DaysOfWeek daysOfWeek)
        {
            return await TimedEventsManager.AddJob(TimedEventType.WeekSchedule, actionGroup, methodStr, 0, minute, hour, dayOfMonth, month, year, daysOfWeek: daysOfWeek);
        }

        public static async Task<string> AddSecondlyTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatSecondlyForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year);
        }

        public static async Task<string> AddSecondlyTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatSecondlyForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, amount: amount);
        }

        public static async Task<string> AddEveryXSecondsTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXSecondsForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X);
        }
        public static async Task<string> AddEveryXSecondsTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X, int amount)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXSecondsForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X, amount: amount);
        }

        public static async Task<string> AddMinutelyTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatMinutelyForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year);
        }

        public static async Task<string> AddMinutelyTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatMinutelyForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, amount: amount);
        }

        public static async Task<string> AddEveryXMinutesTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXMinutesForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X);
        }
        public static async Task<string> AddEveryXMinutesTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X, int amount)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXMinutesForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X, amount: amount);
        }

        public static async Task<string> AddHourlyTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatHourlyForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year);
        }

        public static async Task<string> AddHourlyTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatHourlyForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, amount: amount);
        }

        public static async Task<string> AddEveryXHoursTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXHoursForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X);
        }
        public static async Task<string> AddEveryXHoursTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X, int amount)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXHoursForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X, amount: amount);
        }

        public static async Task<string> AddDailyTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatDailyForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year);
        }

        public static async Task<string> AddDailyTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatDailyForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, amount: amount);
        }

        public static async Task<string> AddEveryXDaysTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXDaysForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X);
        }
        public static async Task<string> AddEveryXDaysTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXDaysForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X, amount: amount);
        }

        public static async Task<string> AddWeeklyTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatWeeklyForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year);
        }

        public static async Task<string> AddWeeklyTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatWeeklyForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, amount: amount);
        }

        public static async Task<string> AddEveryXWeeksTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXWeeksForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X);
        }
        public static async Task<string> AddEveryXWeeksTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXWeeksForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X, amount: amount);
        }

        public static async Task<string> AddMonthlyTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatMonthlyForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year);
        }

        public static async Task<string> AddMonthlyTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatMonthlyForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, amount: amount);
        }

        public static async Task<string> AddEveryXMonthsTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXMonthsForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X);
        }
        public static async Task<string> AddEveryXMonthsTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXMonthsForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X, amount: amount);
        }

        public static async Task<string> AddYearlyTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatYearlyForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year);
        }

        public static async Task<string> AddYearlyTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatYearlyForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, amount: amount);
        }

        public static async Task<string> AddEveryXYearsTimedEvent(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXYearsForever, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X);
        }
        public static async Task<string> AddEveryXYearsTimedEventAtAmount(string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int amount, int X)
        {
            return await TimedEventsManager.AddJob(TimedEventType.RepeatEveryXYearsForAmount, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X: X, amount: amount);
        }

        public static async Task<string> AddTimedEvent(string typeStr, string actionGroup, string methodStr, int second, int minute, int hour, int dayOfMonth, int month, int year, int X, int amount, DaysOfWeek daysOfWeek)
        {
            TimedEventType type;
            if (Enum.TryParse(typeStr, out type))
            {
                return await TimedEventsManager.AddJob(type, actionGroup, methodStr, second, minute, hour, dayOfMonth, month, year, X, amount, daysOfWeek);
            }

            return $"Invalid Timed Event Type: {typeStr}.";
        }
    }
}
