using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MidiDomotica.Core.Handlers;
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
    public class ControlsController : ControllerBase
    {
        private ControlsHandler _handler;

        public ControlsController(ControlsHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int rpp = 10)
        {
            return Ok(_handler.GetControls(page, rpp));
        }

        [HttpGet("subscriptions/{controlName}")]
        public IActionResult GetSubsciptionsForControl(string controlName, int page = 1, int rpp = 10)
        {
            controlName = controlName.Replace("%2f", "/");
            return Ok(_handler.GetSubsciptionsForControl(controlName, page, rpp));
        }
    }
}
