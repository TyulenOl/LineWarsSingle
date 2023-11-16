using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    /// <summary>
    /// Класс команд, которые являются обертками над ExecutorAction
    /// </summary>
    public abstract class ActionCommand<TExecutor, TAction> : 
        IActionCommand<TAction>
        where TExecutor : IExecutor<TExecutor, TAction>, IExecutor
        where TAction : IExecutorAction<TExecutor>
    {
        public TAction Action { get; }
        public TExecutor Executor { get; }

        protected ActionCommand([NotNull] TExecutor executor) :
            this(executor.TryGetUnitAction<TAction>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(TExecutor)} does not contain {nameof(TAction)}"))
        {
        }

        protected ActionCommand([NotNull] TAction action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
            Executor = action.Executor;
        }
        
        public abstract void Execute();
        public abstract bool CanExecute();
        public abstract string GetLog();
    }
}