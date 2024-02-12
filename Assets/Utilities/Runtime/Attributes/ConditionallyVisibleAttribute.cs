using UnityEngine;

namespace Utilities.Runtime
{
    public class ConditionallyVisibleAttribute: PropertyAttribute
    {
        public bool invert { get; }
        public string propertyName { get; }
        
        public ConditionallyVisibleAttribute(string propName, bool invert = false)
        {
            this.invert = invert;
            propertyName = propName;
        }
    }
}