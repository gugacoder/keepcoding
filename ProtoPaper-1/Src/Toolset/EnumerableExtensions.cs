using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toolset
{
  public static class EnumerableExtensions
  {
    /// <summary>
    /// Enumera um único objeto.
    /// </summary>
    /// <typeparam name="T">O tipo do objeto.</typeparam>
    /// <param name="element">O objeto a ser enumerado.</param>
    /// <returns>O enumerador do objeto.</returns>
    public static IEnumerable<T> Yield<T>(this T element)
    {
      yield return element;
    }
  }
}