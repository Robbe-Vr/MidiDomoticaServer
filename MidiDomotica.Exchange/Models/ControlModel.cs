using MidiDomotica.Exchange.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.Exchange.Models
{
    public class ControlModel : BaseControlModel
    {
        public ControlModel() {}
        public ControlModel(BaseControlModel baseModel) : base(baseModel)
        {

        }
    }
}
