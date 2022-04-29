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
    public class AudioActionsController : ControllerBase
    {
        private WindowsAudioActionsHandler _handler;

        public AudioActionsController(WindowsAudioActionsHandler handler)
        {
            _handler = handler;
        }
        
        [HttpGet("outputs")]
        public IActionResult GetOutputDevices()
        {
            return Ok(_handler.GetOutputDevices());
        }

        [HttpGet("output/name/{guid}")]
        public IActionResult GetOutputName(string guid)
        {
            return Ok(_handler.GetOutputName(guid));
        }

        [HttpGet("output/id/{name}")]
        public IActionResult GetOutputId(string name)
        {
            return Ok(_handler.GetOutputId(name));
        }

        [HttpGet("inputs")]
        public IActionResult GetInputDevices()
        {
            return Ok(_handler.GetInputDevices());
        }

        [HttpGet("input/name/{guid}")]
        public IActionResult GetInputName(string guid)
        {
            return Ok(_handler.GetInputName(guid));
        }

        [HttpGet("input/id/{name}")]
        public IActionResult GetInputId(string name)
        {
            return Ok(_handler.GetInputId(name));
        }

        [HttpGet("mixers")]
        public IActionResult GetMixers()
        {
            return Ok(_handler.GetMixers());
        }

        [HttpGet("mixer/name/{guid}")]
        public IActionResult GetMixerName(string guid)
        {
            return Ok(_handler.GetMixerName(guid));
        }

        [HttpGet("mixer/id/{name}")]
        public IActionResult GetMixerId(string name)
        {
            return Ok(_handler.GetMixerId(name));
        }
    }
}
