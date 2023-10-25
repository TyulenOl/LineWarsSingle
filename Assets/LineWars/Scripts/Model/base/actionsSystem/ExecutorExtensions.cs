using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public static class ExecutorExtensions
    {
        public static IEnumerable<(ITarget, CommandType)> GetAllAvailableTargets(this IExecutor executor)
        {
            return executor.Accept(new GetAllAvailableCommandsVisitor())
                .Select(x => (x.Item1, x.Item2.First().CommandType));
        }
    }
}