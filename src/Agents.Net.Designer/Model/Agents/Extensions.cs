using System;

namespace Agents.Net.Designer.Model.Agents
{
    public static class Extensions
    {
        public static T AssertTypeOf<T>(this object value)
        {
            if (value == null)
            {
                return default;
            }
            if (value is T result)
            {
                return result;
            }
            throw new InvalidOperationException($"{value} was expected to be {typeof(T)}, but is {value?.GetType()}");
        }
    }
}