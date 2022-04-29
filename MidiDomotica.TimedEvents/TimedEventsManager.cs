using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidiDomotica.TimedEvents
{
    public static class TimedEventsManager
    {
        private static IScheduler _scheduler;

        private static Type _defaultExecutionType;

        public static async Task Init(Type defaultExecutionType)
        {
            _defaultExecutionType = defaultExecutionType;

            StdSchedulerFactory factory = new StdSchedulerFactory();

            _scheduler = await factory.GetScheduler();
            await _scheduler.Start();
        }

        public static async Task<Dictionary<string, IEnumerable<DateTimeOffset>>> GetUpcomingEvents()
        {
            Dictionary<string, IEnumerable<DateTimeOffset>> upcomingEvents = new Dictionary<string, IEnumerable<DateTimeOffset>>();

            IReadOnlyCollection<JobKey> jobkeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            foreach (JobKey key in jobkeys)
            {
                IJobDetail job = await _scheduler.GetJobDetail(key);

                IReadOnlyCollection<ITrigger> triggers = await _scheduler.GetTriggersOfJob(key);

                List<DateTimeOffset> fireTimes = new List<DateTimeOffset>();

                foreach (ITrigger trigger in triggers)
                {
                    DateTimeOffset? firetime = trigger.GetNextFireTimeUtc();

                    if (firetime.HasValue)
                    {
                        fireTimes.Add(firetime.Value);
                    }
                }

                upcomingEvents.Add($"{key.Group}-_-{key.Name}", fireTimes);
            }

            return upcomingEvents;
        }

        public static async Task<bool> RemoveEvent(string group, string name)
        {
            return await _scheduler.DeleteJob(new JobKey(name, group));
        }

        public static async Task<string> AddJob(TimedEventType type, string actionGroup, string methodStr, int second = -1, int minute = -1, int hour = -1, int dayOfMonth = -1, int month = -1, int year = -1, int X = 1, int amount = 0, DaysOfWeek daysOfWeek = DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday | DaysOfWeek.Friday | DaysOfWeek.Saturday | DaysOfWeek.Sunday)
        {
            JobKey expectedKey = new JobKey(methodStr);

            IJobDetail job;
            List<ITrigger> triggers = new List<ITrigger>();
            bool exists = await _scheduler.CheckExists(expectedKey);
            if (exists)
            {
                job = await _scheduler.GetJobDetail(expectedKey);

                triggers.AddRange(
                        await _scheduler.GetTriggersOfJob(expectedKey)
                    );
            }
            else
            {
                job = CreateJob(actionGroup, methodStr);
            }

            IEnumerable<ITrigger> newTriggers = CreateTrigger(job.Key, type, second, minute, hour, dayOfMonth, month, year, amount, X, daysOfWeek);
            triggers.AddRange(newTriggers);

            await _scheduler.ScheduleJob(job, triggers, replace: true);

            TimedEventsStorage.StoreNew(
                new TimedEventData()
                {
                    MethodStr = methodStr,
                    Type = type,
                    ClassType = _defaultExecutionType,
                    Offset = newTriggers.First().StartTimeUtc,
                }
            );

            return job.Key.Name;
        }

        private static IJobDetail CreateJob(string actionGroup, string methodStr)
        {
            JobBuilder builder = JobBuilder.Create().OfType(_defaultExecutionType)
                .WithIdentity(methodStr, actionGroup);
            
            builder.UsingJobData("actionGroup", actionGroup);
            builder.UsingJobData("methodStr", methodStr);

            IJobDetail job = builder.Build();

            return job;
        }

        private static IEnumerable<ITrigger> CreateTrigger(JobKey key, TimedEventType type, int second = 0, int minute = 0, int hour = 0, int dayOfMonth = 0, int month = 0, int year = 0, int amount = 0, int X = 1, DaysOfWeek daysOfWeek = DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday | DaysOfWeek.Friday | DaysOfWeek.Saturday | DaysOfWeek.Sunday)
        {
            DateTimeOffset startingAt = new DateTimeOffset(new DateTime(year, month, dayOfMonth, hour, minute, second).ToUniversalTime());
            
            return CreateTrigger(key, type, startingAt, amount, X, daysOfWeek);
        }

        private static IEnumerable<ITrigger> CreateTrigger(JobKey key, TimedEventType type, DateTimeOffset startingAt, int amount = 0, int X = 0, DaysOfWeek daysOfWeek = DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday | DaysOfWeek.Friday | DaysOfWeek.Saturday | DaysOfWeek.Sunday)
        {
            return (type) switch
            {
                TimedEventType.Once => CreateOnceTrigger(key, startingAt),
                TimedEventType.WeekSchedule => CreateWeekScheduleTrigger(key, daysOfWeek, startingAt),
                TimedEventType.RepeatSecondlyForever => CreateRepeatSecondlyForeverTrigger(key, startingAt),
                TimedEventType.RepeatSecondlyForAmount => CreateRepeatSecondlyForAmountTrigger(key, startingAt, amount),
                TimedEventType.RepeatEveryXSecondsForever => CreateRepeatEveryXSecondsForeverTrigger(key, startingAt, X),
                TimedEventType.RepeatEveryXSecondsForAmount => CreateRepeatEveryXSecondsForAmountTrigger(key, startingAt, X, amount),

                TimedEventType.RepeatMinutelyForever => CreateRepeatMinutelyForeverTrigger(key, startingAt),
                TimedEventType.RepeatMinutelyForAmount => CreateRepeatMinutelyForAmountTrigger(key, startingAt, amount),
                TimedEventType.RepeatEveryXMinutesForever => CreateRepeatEveryXMinutesForeverTrigger(key, startingAt, X),
                TimedEventType.RepeatEveryXMinutesForAmount => CreateRepeatEveryXMinutesForAmountTrigger(key, startingAt, X, amount),

                TimedEventType.RepeatHourlyForever => CreateRepeatHourlyForeverTrigger(key, startingAt),
                TimedEventType.RepeatHourlyForAmount => CreateRepeatHourlyForAmountTrigger(key, startingAt, amount),
                TimedEventType.RepeatEveryXHoursForever => CreateRepeatEveryXHoursForeverTrigger(key, startingAt, X),
                TimedEventType.RepeatEveryXHoursForAmount => CreateRepeatEveryXHoursForAmountTrigger(key, startingAt, X, amount),

                TimedEventType.RepeatDailyForever => CreateRepeatDailyForeverTrigger(key, startingAt),
                TimedEventType.RepeatDailyForAmount => CreateRepeatDailyForAmountTrigger(key, startingAt, amount),
                TimedEventType.RepeatEveryXDaysForever => CreateRepeatEveryXDaysForeverTrigger(key, startingAt, X),
                TimedEventType.RepeatEveryXDaysForAmount => CreateRepeatEveryXDaysForAmountTrigger(key, startingAt, X, amount),

                TimedEventType.RepeatWeeklyForever => CreateRepeatWeeklyForeverTrigger(key, startingAt),
                TimedEventType.RepeatWeeklyForAmount => CreateRepeatWeeklyForAmountTrigger(key, startingAt, amount),
                TimedEventType.RepeatEveryXWeeksForever => CreateRepeatEveryXWeeksForeverTrigger(key, startingAt, X),
                TimedEventType.RepeatEveryXWeeksForAmount => CreateRepeatEveryXWeeksForAmountTrigger(key, startingAt, X, amount),

                TimedEventType.RepeatMonthlyForever => CreateRepeatMonthlyForeverTrigger(key, startingAt),
                TimedEventType.RepeatMonthlyForAmount => CreateRepeatMonthlyForAmountTrigger(key, startingAt, amount),
                TimedEventType.RepeatEveryXMonthsForever => CreateRepeatEveryXMonthsForeverTrigger(key, startingAt, X),
                TimedEventType.RepeatEveryXMonthsForAmount => CreateRepeatEveryXMonthsForAmountTrigger(key, startingAt, X, amount),

                TimedEventType.RepeatYearlyForever => CreateRepeatYearlyForeverTrigger(key, startingAt),
                TimedEventType.RepeatYearlyForAmount => CreateRepeatYearlyForAmountTrigger(key, startingAt, amount),
                TimedEventType.RepeatEveryXYearsForever => CreateRepeatEveryXYearsForeverTrigger(key, startingAt, X),
                TimedEventType.RepeatEveryXYearsForAmount => CreateRepeatEveryXYearsForAmountTrigger(key, startingAt, X, amount),

                _ => CreateOnceTrigger(key, startingAt),
            };
        }

        private static IEnumerable<ITrigger> CreateOnceTrigger(JobKey key, DateTimeOffset startingAt)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"once_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatMinutelyForTotalCount(1))
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateWeekScheduleTrigger(JobKey key, DaysOfWeek daysOfWeek, DateTimeOffset startingAt)
        {
            List<DayOfWeek> days = new List<DayOfWeek>();
            foreach (DaysOfWeek day in Enum.GetValues<DaysOfWeek>().Where(day => daysOfWeek.HasFlag(day)))
            {
                days.Add(Enum.Parse<DayOfWeek>(day.ToString()));
            }

            yield return TriggerBuilder.Create()
                    .ForJob(key)
                    .WithIdentity($"weekly_{String.Join(',', days)}_{startingAt.DateTime}", key.Name)
                    .StartAt(startingAt)
                    .WithSchedule(
                        CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(startingAt.Hour, startingAt.Minute, days.ToArray())
                    )
                    .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatSecondlyForAmountTrigger(JobKey key, DateTimeOffset startingAt, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"secondly_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatSecondlyForTotalCount(amount)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatSecondlyForeverTrigger(JobKey key, DateTimeOffset startingAt)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"secondly_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatSecondlyForever()
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXSecondsForeverTrigger(JobKey key, DateTimeOffset startingAt, int X)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_seconds_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatSecondlyForever()
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXSecondsForAmountTrigger(JobKey key, DateTimeOffset startingAt, int X, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_seconds_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatSecondlyForever()
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatMinutelyForAmountTrigger(JobKey key, DateTimeOffset startingAt, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"minutely_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatMinutelyForTotalCount(amount)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatMinutelyForeverTrigger(JobKey key, DateTimeOffset startingAt)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"minutely_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatMinutelyForever()
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXMinutesForeverTrigger(JobKey key, DateTimeOffset startingAt, int X)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_minutes_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatMinutelyForever()
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXMinutesForAmountTrigger(JobKey key, DateTimeOffset startingAt, int X, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_minutes_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatMinutelyForever()
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatHourlyForAmountTrigger(JobKey key, DateTimeOffset startingAt, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"hourly_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatHourlyForTotalCount(amount)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatHourlyForeverTrigger(JobKey key, DateTimeOffset startingAt)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"hourly_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatHourlyForever()
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXHoursForeverTrigger(JobKey key, DateTimeOffset startingAt, int X)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_hours_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatHourlyForever()
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXHoursForAmountTrigger(JobKey key, DateTimeOffset startingAt, int X, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_hours_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    SimpleScheduleBuilder.RepeatHourlyForever()
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatDailyForAmountTrigger(JobKey key, DateTimeOffset startingAt, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"daily_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInWeeks(1)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatDailyForeverTrigger(JobKey key, DateTimeOffset startingAt)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"daily_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInDays(1)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXDaysForeverTrigger(JobKey key, DateTimeOffset startingAt, int X)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_days_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInDays(X)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXDaysForAmountTrigger(JobKey key, DateTimeOffset startingAt, int X, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_days_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInDays(X)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatWeeklyForAmountTrigger(JobKey key, DateTimeOffset startingAt, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"weekly_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInWeeks(1)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatWeeklyForeverTrigger(JobKey key, DateTimeOffset startingAt)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"weekly_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInWeeks(1)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXWeeksForeverTrigger(JobKey key, DateTimeOffset startingAt, int X)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_weeks_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInWeeks(X)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXWeeksForAmountTrigger(JobKey key, DateTimeOffset startingAt, int X, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_weeks_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInWeeks(X)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatMonthlyForAmountTrigger(JobKey key, DateTimeOffset startingAt, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"monthly_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInMonths(1)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatMonthlyForeverTrigger(JobKey key, DateTimeOffset startingAt)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"monthly_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInMonths(1)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXMonthsForeverTrigger(JobKey key, DateTimeOffset startingAt, int X)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_months_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInMonths(X)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXMonthsForAmountTrigger(JobKey key, DateTimeOffset startingAt, int X, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_months_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInMonths(X)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatYearlyForAmountTrigger(JobKey key, DateTimeOffset startingAt, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"yearly_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInYears(1)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatYearlyForeverTrigger(JobKey key, DateTimeOffset startingAt)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"yearly_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInYears(1)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXYearsForeverTrigger(JobKey key, DateTimeOffset startingAt, int X)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_years_forever_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInYears(X)
                 )
                 .Build();
        }

        private static IEnumerable<ITrigger> CreateRepeatEveryXYearsForAmountTrigger(JobKey key, DateTimeOffset startingAt, int X, int amount)
        {
            yield return TriggerBuilder.Create()
                 .ForJob(key)
                 .WithIdentity($"every_{X}_years_for_amount_{amount}_{startingAt.DateTime}", key.Name)
                 .StartAt(startingAt)
                 .WithSchedule(
                    CalendarIntervalScheduleBuilder.Create().WithIntervalInYears(X)
                 )
                 .Build();
        }
    }
}
