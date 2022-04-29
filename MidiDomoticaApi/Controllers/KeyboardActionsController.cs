using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MidiDomotica.Core.Handlers;
using MidiDomotica.Core.Handlers.Actions;
using MidiDomotica.Exchange.Models;
using MidiDomoticaApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MidiDomotica.Core.WrapperTranslators.HueTranslator;

namespace MidiDomoticaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorized]
    public class KeyboardActionsController : ControllerBase
    {
        private KeyboardActionsHandler _handler;

        public KeyboardActionsController(KeyboardActionsHandler handler)
        {
            _handler = handler;
        }
        
        [HttpGet("keys")]
        public IActionResult GetPowerModes()
        {
            return Ok(_handler.GetKeys());
        }
    }
}
