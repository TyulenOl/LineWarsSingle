using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class ExecutorMessage
    {
        public IEnumerable<TargetActionInfo> Data { get; }

        public ExecutorMessage(IEnumerable<TargetActionInfo> data)
        {
            Data = data.ToArray();
        }
    }

    public class TargetActionInfo
    {
        public IMonoTarget Target { get; }
        
        public CommandType CommandType { get; }

        public TargetActionInfo(IMonoTarget target, CommandType commandType)
        {
            Target = target;
            CommandType = commandType;
        }
    }
}