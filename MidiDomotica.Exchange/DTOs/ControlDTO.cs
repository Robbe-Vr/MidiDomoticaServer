using MidiDomotica.Exchange.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.Exchange.DTOs
{
    public class ControlDTO : BaseControlModel
    {
        public ControlDTO() {}
        public ControlDTO(BaseControlModel baseModel) : base(baseModel)
        {

        }
    }
}
