using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidiDomoticaApi.Models
{
    public class AuthorizationModel
    {
        public string AppSecret { get; set; }
        public string Password { get; set; }
    }
}
