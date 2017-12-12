using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toolset.Data
{
  public class MongoDbConnectionString : ConnectionString
  {
    public const string ServerKey = "server";
    public const string PortKey = "port";
    public const string DatabaseKey = "database";
    public const string UserKey = "uid";
    public const string PasswordKey = "pwd";

    public const int DefaultPort = 27017;

    public MongoDbConnectionString()
      : base(new ConnectionStringParserFromUri())
    {
    }

    public MongoDbConnectionString(string connectionString)
      : base(new ConnectionStringParserFromUri(), connectionString)
    {
    }

    public string Server
    {
      get { return this[ServerKey]; }
      set { this[ServerKey] = value; }
    }

    public int Port
    {
      get
      {
        var valor = this[PortKey];

        int numero = 0;
        var ok = int.TryParse(valor, out numero);

        return ok ? numero : DefaultPort;
      }
      set
      {
        this[PortKey] = (value == 0) ? null : value.ToString();
      }
    }

    public string Database
    {
      get { return this[DatabaseKey]; }
      set { this[DatabaseKey] = value; }
    }

    public string User
    {
      get { return this[UserKey]; }
      set { this[UserKey] = value; }
    }

    public string Password
    {
      get { return this[PasswordKey]; }
      set { this[PasswordKey] = value; }
    }

    /// <summary>
    /// Padroniza a chave de acesso às propriedades da conexão.
    /// Alguns valores alternativos são permitidos para as chaves de conexão.
    /// A cada acesso à chave a instância de ConnectionString procura por uma chave
    /// apropriada padronizada.
    /// </summary>
    /// <param name="alternateKey">Um nome de chave alterativa suportada.</param>
    /// <returns>A chave apropriada e padronizada.</returns>
    protected override string GetCanonicalKey(string alternateKey)
    {
      switch (alternateKey)
      {
        case "data source":
        case "server":
        case "address":
        case "addr":
        case "network address":
          return ServerKey;

        case "port":
          return PortKey;

        case "database":
        case "catalog":
        case "initial catalog":
        case "default catalog":
          return DatabaseKey;

        case "uid":
        case "user":
        case "user id":
          return UserKey;

        case "pwd":
        case "pass":
        case "password":
          return PasswordKey;

        default:
          return alternateKey;
      }
    }

  }
}