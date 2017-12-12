using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Toolset.Crypto;

namespace Toolset.Data
{
  /// <summary>
  /// Contrato do parser responsável pelo desmonte e remonte das strings de conexão.
  /// </summary>
  public interface IConnectionStringParser
  {
    /// <summary>
    /// Interpreta os parâmetros da string de conexão e retorna a coleção de chave e valor obtida.
    /// </summary>
    /// <param name="connectionString">A string de conexão.</param>
    /// <returns>A coleção de chave e valor obtida.</returns>
    NameValueCollection ExtractParameters(string connectionString);

    /// <summary>
    /// Monta a string de conexão de acordo com a coleção de chave e valor.
    /// </summary>
    /// <param name="connectionString">A coleção de chave e valor para montagem da string de conexão.</param>
    /// <param name="format">Os formatos aplicáveis.</param>
    /// <returns>a string de conexão montada.</returns>
    string FormatString(NameValueCollection connectionString, ConnectionStringFormat format);
  }
}