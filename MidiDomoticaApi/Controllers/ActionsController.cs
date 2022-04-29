using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MidiDomotica.Core.Handlers;
using MidiDomotica.Exchange.Models;
using MidiDomoticaApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidiDomoticaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorized]
    public class ActionsController : ControllerBase
    {
        private ActionsHandler _handler;

        public ActionsController(ActionsHandler handler)
        {
            _handler = handler;
        }

        [HttpPost("{action}")]
        public IActionResult Instant(ActionDataModel model)
        {
            return Ok(_handler.InstantExecuteAction(model.ActionGroup, model.ActionDataString));
        }

        [HttpPost("{action}/{eventName}")]
        public IActionResult Link(string eventName, ActionDataModel model)
        {
            eventName = eventName.Replace("%2f", "/");
            return Ok(_handler.LinkActionToEvent(eventName, model.ActionGroup, model.ActionDataString));
        }

        [HttpPost("{action}/{eventName}")]
        public IActionResult Unlink(string eventName, ActionDataModel model)
        {
            eventName = eventName.Replace("%2f", "/");
            return Ok(_handler.UnlinkActionFromEvent(eventName, model.ActionGroup, model.ActionDataString));
        }
    }
}
