using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Toolset.Crypto;

namespace Toolset.Data
{
  /// <summary>
  /// Parser responsável pelo desmonte e remonte das strings de conexão no formato padrão
  /// do MongoDb.
  /// 
  /// Referência:
  /// - https://docs.mongodb.com/manual/reference/connection-string/
  /// </summary>
  public class ConnectionStringParserFromUri : IConnectionStringParser
  {
    /// <summary>
    /// Interpreta os parâmetros da string de conexão e retorna a coleção de chave e valor obtida.
    /// 
    /// Padrão de uma string de conexão do MongoDb:
    ///   mongodb://[username:password@]host1[:port1][,host2[:port2],...[,hostN[:portN]]][/[database][?options]]
    ///   
    /// O método também permite a extração de parâmetros de uma coleção de chave valor no padrão de
    /// string de conexão da Microsoft.
    /// </summary>
    /// <param name="connectionString">A string de conexão.</param>
    /// <returns>A coleção de chave e valor obtida.</returns>
    public NameValueCollection ExtractParameters(string connectionString)
    {
      if (connectionString.StartsWith("mongodb://"))
      {
        return ExtractParametersFromUri(connectionString);
      }
      else
      {
        return ExtractParametersFromKeyPairs(connectionString);
      }
    }

    /// <summary>
    /// Extrai parâmetros de uma coleção de chave valor no padrão de string de conexão da Microsoft.
    /// </summary>
    /// <param name="connectionString">A string de conexão.</param>
    /// <returns>A coleção de chave e valor obtida.</returns>
    private NameValueCollection ExtractParametersFromKeyPairs(string connectionString)
    {
      var parser = new ConnectionStringParserFromKeyValuePairs();
      var collection = parser.ExtractParameters(connectionString);
      return collection;
    }

    /// <summary>
    /// Extrai parâmetros da URI do MongoDb.
    /// 
    /// Padrão de uma string de conexão do MongoDb:
    ///   mongodb://[username:password@]host1[:port1][,host2[:port2],...[,hostN[:portN]]][/[database][?options]]
    ///   
    /// </summary>
    /// <param name="connectionString">A string de conexão.</param>
    /// <returns>A coleção de chave e valor obtida.</returns>
    private NameValueCollection ExtractParametersFromUri(string connectionString)
    {
      var collection = new NameValueCollection();

      //
      // Expressão regular para extrair parâmetros da string de conexão do MongoDb:
      //   mongodb://[username:password@]host1[:port1][,host2[:port2],...[,hostN[:portN]]][/[database][?options]]
      //
      var regex = new Regex(@"^mongodb://(?:([^:]*):([^@]*)@)?([^/:]*)(?::([^/]*))?(?:/([^?]*))?(?:\?(.*))?$");

      var match = regex.Match(connectionString);
      if (match.Success)
      {
        var user = match.Groups[1].Value;
        var pass = match.Groups[2].Value;
        var host = match.Groups[3].Value;
        var port = match.Groups[4].Value;
        var database = match.Groups[5].Value;
        var options = match.Groups[6].Value;

        collection[MongoDbConnectionString.UserKey] = user;
        collection[MongoDbConnectionString.PasswordKey] = pass;
        collection[MongoDbConnectionString.ServerKey] = host;
        collection[MongoDbConnectionString.PortKey] = port;
        collection[MongoDbConnectionString.DatabaseKey] = database;

        var tokens =
          from properties in options.Split('&')
          let parts = properties.Split('=')
          let key = parts.First().Trim()
          let value = string.Join("=", parts.Skip(1))
          select new { key, value };

        foreach (var token in tokens)
        {
          collection[token.key] = token.value;
        }
      }

      return collection;
    }

    /// <summary>
    /// Monta a string de conexão de acordo com a coleção de chave e valor.
    /// 
    /// Padrão de uma string de conexão do MongoDb:
    ///   mongodb://[username:password@]host1[:port1][,host2[:port2],...[,hostN[:portN]]][/[database][?options]]
    /// 
    /// </summary>
    /// <param name="collection">A coleção de chave e valor para montagem da string de conexão.</param>
    /// <param name="format">Os formatos aplicáveis.</param>
    /// <returns>a string de conexão montada.</returns>
    public string FormatString(NameValueCollection collection, ConnectionStringFormat format)
    {
      //
      // Montando uma string de conexão no padrão do mongo
      //   mongodb://[username:password@]host1[:port1][,host2[:port2],...[,hostN[:portN]]][/[database][?options]]
      //

      var user = collection[MongoDbConnectionString.UserKey];
      var pass = collection[MongoDbConnectionString.PasswordKey];
      var host = collection[MongoDbConnectionString.ServerKey];
      var port = collection[MongoDbConnectionString.PortKey];
      var database = collection[MongoDbConnectionString.DatabaseKey];

      var builder = new StringBuilder("mongodb://");

      if (!string.IsNullOrEmpty(user))
      {
        builder.Append(user);
        if (!string.IsNullOrEmpty(pass))
        {
          var formattedPass = Formatar(pass, format);
          builder.Append(":").Append(formattedPass);
        }
        builder.Append("@");
      }

      builder.Append(host ?? "localhost");
      if (!string.IsNullOrEmpty(port))
      {
        builder.Append(":").Append(port);
      }
      builder.Append("/");

      if (!string.IsNullOrEmpty(database))
      {
        builder.Append(database);
      }

      var chavesAdicionais = collection.AllKeys.Except(new[]
      {
        MongoDbConnectionString.UserKey,
        MongoDbConnectionString.PasswordKey,
        MongoDbConnectionString.ServerKey,
        MongoDbConnectionString.PortKey,
        MongoDbConnectionString.DatabaseKey
      });

      if (chavesAdicionais.Any())
      {
        builder.Append("?");

        var options =
          from key in chavesAdicionais
          let value = collection[key]
          let formattedValue = value.Replace(" ", "%20") // em URLs espaços devem ser codificados como %20
          select key + "=" + value;

        var text = string.Join("&", options);
        builder.Append(text);
      }

      var connectionString = builder.ToString();
      return connectionString;
    }

    /// <summary>
    /// Aplica as formatações necessárias para a senha indicada.
    /// </summary>
    /// <param name="key">O valor da senha.</param>
    /// <param name="format">As formatações aplicáveis.</param>
    /// <returns>O valor da senha devidamente formatado.</returns>
    public string Formatar(string key, ConnectionStringFormat format)
    {
      if (format != ConnectionStringFormat.Default)
      {
        var isEncrypted = format.HasFlag(ConnectionStringFormat.Encrypted);
        if (isEncrypted)
        {
          key = Encryption.Encrypt(key);
        }
        else
        {
          key = Encryption.Decrypt(key);
        }
      }
      return key;
    }

  }
}