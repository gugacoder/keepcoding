using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolset.Application
{
  /// <summary>
  /// Utilitário de manipulação de chaves por domínio.
  /// </summary>
  internal static class KeyNames
  {
    /// <summary>
    /// Constrói um enumerado de nomes de chave por domínio.
    /// A partir do domínio mais longo até o domínio mais curto terminando com o nome simples da chave.
    /// 
    /// Por exemplo, para a chave "MinhaChave" do domínio "MeuDominio.com" os nomes produzidos
    /// seriam:
    /// 
    /// -   MeuDominio.com:MinhaChave
    /// -   com:MinhaChave
    /// -   MinhaChave
    /// </summary>
    /// <param name="domain">Um nome de domínio.</param>
    /// <param name="section">Seção de configuração onde a chave se encontra.</param>
    /// <param name="key">A chave a ser enumerada.</param>
    /// <returns>Os nomes produzidos para a identificação da chave por domínio.</returns>
    public static IEnumerable<string> GetKeys(string domain, string section, string key)
    {
      section = section.ChangeCase(TextCase.CamelCase);
      key = key.ChangeCase(TextCase.CamelCase);
      if (!string.IsNullOrWhiteSpace(domain))
      {
        var tokens = domain.Split('.');
        for (int count = 0; count < tokens.Length; count++)
        {
          var prefix = string.Join(":", tokens.Skip(count).Reverse());
          yield return $"{prefix}:{section}:{key}";
        }
      }
      yield return $"{section}:{key}";
    }
  }
}
