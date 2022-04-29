using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.Core
{
    public abstract class AbstractJob : IJob
    {
        public abstract Task Execute(IJobExecutionContext context);
    }
}
