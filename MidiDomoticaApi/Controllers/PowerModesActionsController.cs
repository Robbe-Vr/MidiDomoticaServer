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
    public class PowerModesActionsController : ControllerBase
    {
        private PowerModesActionsHandler _handler;

        public PowerModesActionsController(PowerModesActionsHandler handler)
        {
            _handler = handler;
        }
        
        [HttpGet("modes")]
        public IActionResult GetPowerModes()
        {
            return Ok(_handler.GetPowerModes());
        }

        [HttpGet("name/{guid}")]
        public IActionResult GetName(string guid)
        {
            return Ok(_handler.GetName(guid));
        }

        [HttpGet("id/{name}")]
        public IActionResult GetId(string name)
        {
            return Ok(_handler.GetId(name));
        }
    }
}
