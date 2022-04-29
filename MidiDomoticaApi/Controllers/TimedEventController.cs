using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MidiDomotica.Core.Handlers;
using MidiDomotica.Exchange.Models;
using MidiDomoticaApi.Filters;
using MidiDomoticaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomoticaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorized]
    public class TimedEventController : ProcessingController
    {
        public TimedEventController()
        {
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingEvents()
        {
            return await Process(async () =>
            {
                try
                {
                    Dictionary<string, IEnumerable<DateTimeOffset>> upcomingEvents = await TimedEventsHandler.GetUpcomingEvents();

                    return Ok(upcomingEvents.Select(x =>
                    {
                        string[] data = x.Key.Split("-_-");
                        return new
                        { 
                            Name = data[0] + "-" + data[1],
                            ActionGroup = data[0],
                            MethodStr = data[1],
                            NextFireTimes = x.Value.Select(x => x.ToUnixTimeSeconds()),
                        };
                    }));
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add once timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpGet("remove/{group}/{name}")]
        public async Task<IActionResult> RemoveTimedEvent(string group, string name)
        {
            return await Process(async () =>
            {
                try
                {
                    bool result = await TimedEventsHandler.RemoveTimedEvent(group, name);

                    return Ok(new {
                        Result = result,
                        Message = result ? "Timed event has been removed!" : "Failed to remove timed event!",
                    });
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add once timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/once")]
        public async Task<IActionResult> AddOnceTimedEvent([FromForm] TimedEventDataModel model)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId = await TimedEventsHandler.AddOnceTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year);

                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add once timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/weekschedule")]
        public async Task<IActionResult> AddWeekScheduleTimedEvent([FromForm] TimedEventDataModel model)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId = await TimedEventsHandler.AddWeekScheduleTimedEvent(model.ActionGroup, model.MethodStr, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, model.DaysOfWeek);

                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add week schedule timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/secondly/{amount}")]
        public async Task<IActionResult> AddSecondlyTimedEventAtAmount([FromForm] TimedEventDataModel model,
            string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddSecondlyTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddSecondlyTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add secondly timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/every/{X}/seconds/{amount}")]
        public async Task<IActionResult> AddEveryXSecondsTimedEventAtAmount([FromForm] TimedEventDataModel model,
            int X, string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddEveryXSecondsTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddEveryXSecondsTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add every {X} seconds timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/minutely/{amount}")]
        public async Task<IActionResult> AddEveryXMinutesTimedEventAtAmount([FromForm] TimedEventDataModel model,
            [FromForm] string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddMinutelyTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddMinutelyTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add minutely timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/every/{X}/minutes/{amount}")]
        public async Task<IActionResult> AddMinutelyTimedEventAtAmount([FromForm] TimedEventDataModel model,
            int X, string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddEveryXMinutesTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddEveryXMinutesTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add every {X} minutes timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/hourly/{amount}")]
        public async Task<IActionResult> AddEveryXHoursTimedEventAtAmount([FromForm] TimedEventDataModel model,
            string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddHourlyTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddHourlyTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add hourly timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/every/{X}/hours/{amount}")]
        public async Task<IActionResult> AddEveryXHoursTimedEventAtAmount([FromForm] TimedEventDataModel model,
            int X, string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddEveryXHoursTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddEveryXHoursTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add every {X} hours timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/daily/{amount}")]
        public async Task<IActionResult> AddDailyTimedEventAtAmount([FromForm] TimedEventDataModel model,
            string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddDailyTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddDailyTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add daily timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/every/{X}/days/{amount}")]
        public async Task<IActionResult> AddEveryXDaysTimedEventAtAmount([FromForm] TimedEventDataModel model,
            int X, string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddEveryXDaysTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddEveryXDaysTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add every {X} days timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/weekly/{amount}")]
        public async Task<IActionResult> AddWeeklyTimedEventAtAmount([FromForm] TimedEventDataModel model,
            string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddWeeklyTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddWeeklyTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add weekly timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/every/{X}/weeks/{amount}")]
        public async Task<IActionResult> AddEveryXWeeksTimedEventAtAmount([FromForm] TimedEventDataModel model,
            int X, string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddEveryXWeeksTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddEveryXWeeksTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add every {X} weeks timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/monthly/{amount}")]
        public async Task<IActionResult> AddEveryXMonthsTimedEventAtAmount([FromForm] TimedEventDataModel model,
            string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddMonthlyTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddMonthlyTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add monthly timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/every/{X}/months/{amount}")]
        public async Task<IActionResult> AddEveryXMonthsTimedEventAtAmount([FromForm] TimedEventDataModel model,
            int X, string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddEveryXMonthsTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddEveryXMonthsTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add every {X} months timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/yearly/{amount}")]
        public async Task<IActionResult> AddYearlyTimedEventAtAmount([FromForm] TimedEventDataModel model,
            string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddYearlyTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddYearlyTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add yearly timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("add/every/{X}/years/{amount}")]
        public async Task<IActionResult> AddEveryXYearsTimedEventAtAmount([FromForm] TimedEventDataModel model,
            int X, string amount)
        {
            return await Process(async () =>
            {
                try
                {
                    string eventId;
                    if (amount.ToLower() == "forever")
                    {
                        eventId = await TimedEventsHandler.AddEveryXYearsTimedEvent(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X);
                    }
                    else
                    {
                        int amountInt = 0;
                        if (int.TryParse(amount, out amountInt))
                            eventId = await TimedEventsHandler.AddEveryXYearsTimedEventAtAmount(model.ActionGroup, model.MethodStr, model.Second, model.Minute, model.Hour, model.DayOfMonth, model.Month, model.Year, X, amountInt);
                        else
                            eventId = "Invalid amount supplied!";
                    }
                    return Ok(eventId);
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while attempting to add every {X} years timed event!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }
    }
}
