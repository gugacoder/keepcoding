using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Toolset.Crypto;

namespace Toolset.Data
{
  public class ConnectionStringParserFromKeyValuePairs : IConnectionStringParser
  {
    /// <summary>
    /// Interpreta os parâmetros da string de conexão e retorna a coleção de key e value obtida.
    /// </summary>
    /// <param name="connectionString">A string de conexão.</param>
    /// <returns>A coleção de key e value obtida.</returns>
    public NameValueCollection ExtractParameters(string connectionString)
    {
      var collection = new NameValueCollection();

      var tokens =
        from properties in connectionString.Split(';')
        let parts = properties.Split('=')
        let key = parts.First().Trim()
        let value = string.Join("=", parts.Skip(1))
        select new { key, value };

      foreach (var token in tokens)
      {
        collection.Add(token.key, token.value);
      }

      return collection;
    }

    /// <summary>
    /// Monta a string de conexão de acordo com a coleção de key e value.
    /// </summary>
    /// <param name="connectionString">A coleção de key e value para montagem da string de conexão.</param>
    /// <param name="formato">Os formatos aplicáveis.</param>
    /// <returns>a string de conexão montada.</returns>
    public string FormatString(NameValueCollection connectionString, ConnectionStringFormat formato)
    {
      var tokens =
        from key in connectionString.AllKeys
        where !string.IsNullOrWhiteSpace(key)
        let value = connectionString[key]
        where !string.IsNullOrWhiteSpace(value)
        let formattedValue = Formatar(key, value, formato)
        select key + "=" + formattedValue;

      var text = string.Join(";", tokens);
      return text;
    }

    /// <summary>
    /// Aplica as formatações necessárias de acordo com o requerido para o campo.
    /// </summary>
    /// <param name="key">A key do campo.</param>
    /// <param name="value">O value do campo.</param>
    /// <param name="formato">As formatações aplicáveis.</param>
    /// <returns>O value do campo devidamente formatado.</returns>
    public string Formatar(string key, string value, ConnectionStringFormat formato)
    {
      if (formato != ConnectionStringFormat.Default)
      {
        var isPassKey = IsPassKey(key);
        if (isPassKey)
        {
          var isEncrypted = formato.HasFlag(ConnectionStringFormat.Encrypted);
          if (isEncrypted)
          {
            value = Encryption.Encrypt(value);
          }
          else
          {
            value = Encryption.Decrypt(value);
          }
        }
      }
      return value;
    }

    /// <summary>
    /// Diz se a key indicada corresponde a uma key de senha dentro
    /// da string de conexão.
    /// </summary>
    private bool IsPassKey(string key)
    {
      return key.Equals("pwd", StringComparison.InvariantCultureIgnoreCase)
          || key.Equals("pass", StringComparison.InvariantCultureIgnoreCase)
          || key.Equals("password", StringComparison.InvariantCultureIgnoreCase);
    }

  }
}