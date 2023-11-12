using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class ExecutorRedrawMessage
    {
        public IEnumerable<TargetActionInfo> Data { get; }

        public ExecutorRedrawMessage(IEnumerable<TargetActionInfo> data)
        {
            Data = data.ToArray();
        }
    }

    public class TargetActionInfo
    {
        public ITarget Target { get; }
        
        public CommandType CommandType { get; }

        public TargetActionInfo(ITarget target, CommandType commandType)
        {
            Target = target;
            CommandType = commandType;
        }
    }
}