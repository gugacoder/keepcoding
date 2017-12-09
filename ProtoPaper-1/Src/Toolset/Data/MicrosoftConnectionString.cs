namespace Toolset.Data
{
  public class MicrosoftConnectionString : ConnectionString
  {
    public const string ServerKey = "server";
    public const string DatabaseKey = "database";
    public const string UserKey = "uid";
    public const string PasswordKey = "pwd";
    public const string TimeoutKey = "timeout";

    public MicrosoftConnectionString()
    {
    }

    public MicrosoftConnectionString(string connectionString)
      : base(connectionString)
    {
    }

    public string Server
    {
      get { return this[ServerKey]; }
      set { this[ServerKey] = value; }
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

    public int Timeout
    {
      get
      {
        var value = this[TimeoutKey];
        int number = 0;
        int.TryParse(value, out number);
        return number;
      }
      set { this[TimeoutKey] = value.ToString(); }
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

        case "timeout":
        case "connect timeout":
        case "connection timeout":
          return TimeoutKey;

        default:
          return alternateKey;
      }
    }

  }
}