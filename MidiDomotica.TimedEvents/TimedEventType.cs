using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.TimedEvents
{
    public enum TimedEventType
    {
        Once,
        WeekSchedule,
        RepeatSecondlyForAmount,
        RepeatEveryXSecondsForAmount,
        RepeatMinutelyForAmount,
        RepeatEveryXMinutesForAmount,
        RepeatHourlyForAmount,
        RepeatEveryXHoursForAmount,
        RepeatDailyForAmount,
        RepeatEveryXDaysForAmount,
        RepeatWeeklyForAmount,
        RepeatEveryXWeeksForAmount,
        RepeatMonthlyForAmount,
        RepeatEveryXMonthsForAmount,
        RepeatYearlyForAmount,
        RepeatEveryXYearsForAmount,
        RepeatSecondlyForever,
        RepeatEveryXSecondsForever,
        RepeatMinutelyForever,
        RepeatEveryXMinutesForever,
        RepeatHourlyForever,
        RepeatEveryXHoursForever,
        RepeatDailyForever,
        RepeatEveryXDaysForever,
        RepeatWeeklyForever,
        RepeatEveryXWeeksForever,
        RepeatMonthlyForever,
        RepeatEveryXMonthsForever,
        RepeatYearlyForever,
        RepeatEveryXYearsForever,
    }
}
