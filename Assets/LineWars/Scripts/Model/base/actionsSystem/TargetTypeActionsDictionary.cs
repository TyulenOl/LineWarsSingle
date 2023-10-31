using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public class TargetTypeActionsDictionary : Dictionary<Type, ITargetedAction[]>
    {
        public TargetTypeActionsDictionary(IEnumerable<KeyValuePair<Type, ITargetedAction[]>> collection) : base(collection) { }

        public new ITargetedAction[] this[Type type]
        {
            get
            {
                if (base.TryGetValue(type, out var value))
                {
                    return value;
                }
                foreach (var key in base.Keys)
                {
                    if (key.IsAssignableFrom(type))
                    {
                        return base[key];
                    }
                }
                throw new KeyNotFoundException();
            }
        }
    }
}