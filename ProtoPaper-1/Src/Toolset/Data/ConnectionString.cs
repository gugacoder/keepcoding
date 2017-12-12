using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Toolset.Data
{
  public class ConnectionString : ICloneable
  {
    private NameValueCollection parameters;
    private IConnectionStringParser provider;

    public ConnectionString()
    {
      this.parameters = new NameValueCollection();
      this.provider = new ConnectionStringParserFromKeyValuePairs();
    }

    public ConnectionString(string connectionString)
    {
      this.parameters = new NameValueCollection();
      this.provider = new ConnectionStringParserFromKeyValuePairs();

      CopyProperties(connectionString);
    }

    protected ConnectionString(IConnectionStringParser provider)
    {
      this.parameters = new NameValueCollection();
      this.provider = provider;
    }

    protected ConnectionString(IConnectionStringParser provider, string connectionString)
    {
      this.parameters = new NameValueCollection();
      this.provider = provider;

      CopyProperties(connectionString);
    }

    #region Métodos customizáveis

    /// <summary>
    /// Padroniza a chave de acesso às propriedades da conexão.
    /// Alguns valores alternativos são permitidos para as chaves de conexão.
    /// A cada acesso à chave a instância de ConnectionString procura por uma chave
    /// apropriada padronizada.
    /// </summary>
    /// <param name="alternateKey">Um nome de chave alterativa suportada.</param>
    /// <returns>A chave apropriada e padronizada.</returns>
    protected virtual string GetCanonicalKey(string alternateKey)
    {
      return alternateKey;
    }

    #endregion

    public string[] AllKeys
    {
      get { return parameters.AllKeys; }
    }

    public string this[string chave]
    {
      get
      {
        chave = CanonicalizeKey(chave);
        return parameters[chave];
      }
      set
      {
        chave = CanonicalizeKey(chave);
        parameters[chave] = value;
      }
    }

    public string this[int indice]
    {
      get { return parameters[indice]; }
    }

    public string GetKey(int indice)
    {
      return parameters.GetKey(indice);
    }

    public ConnectionString CopyProperties(string connectionString)
    {
      connectionString = connectionString ?? "";
      NameValueCollection parametros = provider.ExtractParameters(connectionString);
      CopyProperties(parametros);
      return this;
    }

    public ConnectionString CopyProperties(NameValueCollection connectionString)
    {
      this.parameters.Clear();
      foreach (var chave in connectionString.AllKeys)
      {
        var valor = connectionString[chave];

        var chaveApropriada = CanonicalizeKey(chave);
        this.parameters.Add(chaveApropriada, valor);
      }
      return this;
    }

    public string CanonicalizeKey(string alternateKey)
    {
      alternateKey = alternateKey.ToLower();
      var canonicalKey = this.GetCanonicalKey(alternateKey);
      return canonicalKey;
    }

    public ConnectionString Clone(ConnectionStringFormat format)
    {
      var instance = Activator.CreateInstance(this.GetType());

      var clone = (ConnectionString)instance;
      clone.provider = this.provider;

      var connectionString = this.ToString(format);
      clone.CopyProperties(connectionString);

      return clone;
    }

    public ConnectionString Clone()
    {
      var instance = Activator.CreateInstance(this.GetType());
      var clone = (ConnectionString)instance;
      clone.provider = this.provider;
      clone.parameters = new NameValueCollection(this.parameters);
      return clone;
    }

    object ICloneable.Clone()
    {
      return this.Clone();
    }

    public override string ToString()
    {
      var texto = provider.FormatString(this.parameters, ConnectionStringFormat.Default);
      return texto;
    }

    public string ToString(ConnectionStringFormat formato)
    {
      var texto = provider.FormatString(this.parameters, formato);
      return texto;
    }

  }
}