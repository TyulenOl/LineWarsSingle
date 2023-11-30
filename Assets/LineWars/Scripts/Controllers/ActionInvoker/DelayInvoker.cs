using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Controllers
{
    public class DelayInvoker: ActionInvokerBase
    {
        private readonly List<Action> actions = new();
        
        public override void Invoke([NotNull] Action action)
        {
            if (action == null)
                throw new ArgumentNullException();
            actions.Add(action);
        }

        private void Update()
        {
            if (actions.Count == 0)
                return;
            var temp = actions.ToArray();
            actions.Clear();
            foreach (var action in temp)
            {
                action.Invoke();
            }
        }
    }
}