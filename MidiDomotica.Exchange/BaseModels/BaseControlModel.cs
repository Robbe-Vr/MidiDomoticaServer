using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.Exchange.BaseModels
{
    public class BaseControlModel
    {
        public BaseControlModel() {}
        public BaseControlModel(BaseControlModel model)
        {
            this.Name = model.Name;
        }

        public string Name { get; set; }
    }
}
