using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Toolset.Crypto;

namespace Toolset.Data
{
  /// <summary>
  /// Utilitário de criptografia de senhas.
  /// </summary>
  public class Encryption
  {
    /// <summary>
    /// Prefixo para identificacao de valor encriptado.
    /// Um text iniciado com este prefixo é considerado criptografado.
    /// </summary>
    public const string EncryptionPrefix = "enc:";

    /// <summary>
    /// Chave interna e reservada usada pelos algoritmos de criptografia.
    /// </summary>
    internal static readonly IEnumerable<byte> EncryptionKey;

    /// <summary>
    /// Construtor estático para inicialização da chave de criptografia.
    /// </summary>
    static Encryption()
    {
      try
      {

        // O código abaixo é um embaralhamento da chave de acesso.
        // O código deve produzir um result exatamente igual a esta linha:
        //    ChaveDeCriptografia = Encoding.UTF8.GetBytes("7947A834-9762-4a0f-9253-D81D4FABD422");
        //
        // O objetivo e evitar manter a chave de acesso disponível no Assembly.
        //
        // Em outras palavras, substituir o embaralhamento abaixo pelo código acima deve produzir
        // exatamente o mesmo result.

        EncryptionKey =
          Shuffler.Shuffle(
            "4Q8vtme2ml3qTVRErQRxXQ7q80cPAYdd83HmR3F0dARU6q1nXecyTI9NDk2aVDIEh2dH6bYydIwB4R9uwWeMjHEBR7QBZ+p3bukEDuGZh5p0mXGPbgQBcYLz5rbhbjJUD2e2RLbzreZn5kRu83HzDrZM6Yfhtue2824fRFSCTOrBD1SPcURUAQG2H+G05oeMmgHztkfmtk3qTERNAbat6g=="
          );
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }

    /// <summary>
    /// Diz se a chave indicada corresponde a uma chave de senha dentro
    /// da string de conexão.
    /// </summary>
    internal static bool IsEncrypted(string text)
    {
      return (text != null) && text.StartsWith(EncryptionPrefix);
    }

    /// <summary>
    /// Produz um text encriptado.
    /// O text encriptado é prefixado com o valor da constante PrefixoDeCriptografia.
    /// </summary>
    /// <param name="text">O text a ser encriptado.</param>
    /// <returns>O text encriptado e prefixado pelo valor da constante PrefixoDeCriptografia.</returns>
    internal static string Encrypt(string text)
    {
      if (IsEncrypted(text))
      {
        return text;
      }

      var encryptedKey = Encrypter.EncryptToString(text, EncryptionKey);
      var result = EncryptionPrefix + encryptedKey;
      return result;
    }

    /// <summary>
    /// Produz um text decriptado.
    /// O text encriptado deve ter sido prefixado com o valor da constante PrefixoDeCriptografia.
    /// </summary>
    /// <param name="text">O text encriptado.</param>
    /// <returns>O text decriptado e sem o prefixo da constante PrefixoDeCriptografia.</returns>
    internal static string Decrypt(string text)
    {
      if (!IsEncrypted(text))
      {
        return text;
      }

      var noPrefix = text.Substring(EncryptionPrefix.Length);
      var decryptedText = Decrypter.DecryptToString(noPrefix, EncryptionKey);

      return decryptedText;
    }
  }
}