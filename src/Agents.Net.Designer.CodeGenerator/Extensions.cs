using System;
using System.Collections.Generic;
using System.Linq;

namespace Agents.Net.Designer.CodeGenerator
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> original)
        {
            IEnumerator<T>[] enumerators = original.Select(e => e.GetEnumerator()).ToArray();
            try
            {
                while (enumerators.All(e => e.MoveNext()))
                {
                    yield return enumerators.Select(e => e.Current).ToArray();
                }
            }
            finally
            {
                Array.ForEach(enumerators, e => e.Dispose());
            }
        }
    }
}
