using UnityEngine;

namespace Utilities.Runtime
{
    public class ConditionallyVisibleAttribute: PropertyAttribute
    {
        public string propertyName { get; }
        
        public ConditionallyVisibleAttribute(string propName)
        {
            propertyName = propName;
        }
    }
}