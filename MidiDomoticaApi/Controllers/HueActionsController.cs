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
    public class HueActionsController : ControllerBase
    {
        private HueActionsHandler _handler;

        public HueActionsController(HueActionsHandler handler)
        {
            _handler = handler;
        }
        
        [HttpGet("targets/{mode}")]
        public IActionResult GetTargetsForMode(Modes mode)
        {
            return Ok(_handler.GetTargetsForMode(mode));
        }

        [HttpGet("entertainmentmodes")]
        public IActionResult GetEntertainmentModes()
        {
            return Ok(_handler.GetEntertainmentModes());
        }

        [HttpGet("connect/{bridgeInfo}")]
        public IActionResult ConnectToBridge(string bridgeInfo)
        {
            return Ok(_handler.ConnectToBridge(bridgeInfo));
        }
    }
}
