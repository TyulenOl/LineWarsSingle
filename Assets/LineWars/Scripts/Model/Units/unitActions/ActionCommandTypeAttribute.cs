using System;

namespace LineWars.Model.unitActions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ActionCommandTypeAttribute: Attribute
    {
        public ActionCommandTypeAttribute(CommandType commandType)
        {
            CommandType = commandType;
        }

        public CommandType CommandType { get; }
    }
}