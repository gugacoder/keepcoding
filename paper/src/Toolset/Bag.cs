using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolset.Application;

namespace Toolset
{
  /// <summary>
  /// Coleção de configurações chaveadas.
  /// </summary>
  /// <typeparam name="T">Tipo da configuração.</typeparam>
  public class Bag<T>
  {
    private readonly Dictionary<string, T> bag;

    public Bag(int capacity = 0)
    {
      bag = new Dictionary<string, T>(capacity);
    }

    /// <summary>
    /// Valor padrão da propriedade.
    /// Se aplica quando não há um valor nomeado definido.
    /// </summary>
    public T DefaultValue
    {
      get { return bag.ContainsKey(string.Empty) ? bag[string.Empty] : default(T); }
      set { bag[string.Empty] = value; }
    }

    /// <summary>
    /// Valor para o domínio corrente.
    /// </summary>
    public T Value
    {
      get { return bag[App.User.Domain]; }
      set { bag[App.User.Domain] = value; }
    }

    /// <summary>
    /// Valor nomeado de uma propriedade.
    /// Quando não definido, o valor padrão é retornado.
    /// </summary>
    /// <param name="key">O nome da propriedade.</param>
    /// <returns>O valor da propriedade ou o valor padrão, quando não definida.</returns>
    public T this[string key]
    {
      get { return bag.ContainsKey(key) ? bag[key] : DefaultValue; }
      set { bag[key] = value; }
    }
  }
}

