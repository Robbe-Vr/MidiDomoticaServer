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
using static MidiDomotica.Core.WrapperTranslators.SoundboardTranslator;

namespace MidiDomoticaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorized]
    public class SoundboardActionsController : ControllerBase
    {
        private SoundboardActionsHandler _handler;

        public SoundboardActionsController(SoundboardActionsHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("sound/{soundfile}")]
        public IActionResult GetSound(string soundfile)
        {
            soundfile = soundfile.Replace("%2f", "/");

            string ext = System.IO.Path.GetExtension(soundfile);
            string type = ext == ".mp3" ? "mpeg" : "wav";
            return File(System.IO.File.ReadAllBytes(soundfile), $"audio/{type}");
        }

        [HttpGet("sounds")]
        public IActionResult GetSounds()
        {
            return Ok(_handler.GetSoundboardSounds());
        }

        [HttpGet("soundfolders")]
        public IActionResult GetSoundFolders()
        {
            return Ok(_handler.GetSoundboardSoundFolders());
        }

        [HttpGet("sounds/{folder}")]
        public IActionResult GetSounds(string folder)
        {
            return Ok(_handler.GetSoundboardSoundsFromFolder(folder));
        }

        [HttpGet("inputs")]
        public IActionResult GetInputs()
        {
            return Ok(_handler.GetInputDevices());
        }

        [HttpGet("outputs")]
        public IActionResult GetOutputs()
        {
            return Ok(_handler.GetOutputDevices());
        }

        [HttpPost("set/input/{deviceInfo}")]
        public IActionResult SetInput(string deviceInfo)
        {
            return Ok(_handler.SetInputDevice(deviceInfo));
        }

        [HttpPost("set/throughput/{deviceInfo}")]
        public IActionResult SetThroughput(string deviceInfo)
        {
            return Ok(_handler.SetThroughputDevice(deviceInfo));
        }

        [HttpPost("set/output/{deviceInfo}")]
        public IActionResult SetOutput(string deviceInfo)
        {
            return Ok(_handler.SetOutputDevice(deviceInfo));
        }
    }
}
