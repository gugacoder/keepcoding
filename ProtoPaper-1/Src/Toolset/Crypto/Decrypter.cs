using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;

namespace Toolset.Crypto
{
  /// <summary>
  /// Decriptador de sequências.
  /// A sequência deve ter sido encriptada com a contra parte Processa.Net.Seguranca.Encriptador.
  /// </summary>
  public static class Decrypter
  {
    private static Func<string, byte[]> FromString = Encoding.UTF8.GetBytes;
    private static Func<string, byte[]> FromBase64 = Convert.FromBase64String;
    private static Func<byte[], string> GetString = Encoding.UTF8.GetString;
    private static Func<byte[], string> GetBase64 = Convert.ToBase64String;

    /// <summary>
    /// Decripta a sequência de bytes com uma chave de criptografia.
    /// A sequência dever ter sido encriptada com a contra parte Processa.Net.Seguranca.Encriptador.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser decriptada.</param>
    /// <param name="passKey">A chave de criptografia.</param>
    /// <returns>A sequência de bytes decriptada.</returns>
    private static byte[] Process(IEnumerable<byte> sequence, IEnumerable<byte> passKey)
    {
      if (!sequence.Any())
        return new byte[0];

      // Inicia três iterações sucessivas do algoritmo DES. Ele pode usar dois ou três chaves de 56 bits.
      // Esse algoritmo oferece suporte comprimentos de keyPass de 128 bits para 192 bits em
      // incrementos de 64 bits.
      ICryptoTransform transform;

      using (var des = new TripleDESCryptoServiceProvider())
      {
        using (var hashmd5 = new MD5CryptoServiceProvider())
        {
          des.Key = hashmd5.ComputeHash(passKey.ToArray());
        }

        // Modo de operação do algoritmo simétrico:
        //   Cipher Block Chaining (CBC)
        //   Antes de encriptar cada bloco de texto, ele é combinado com o texto cifrado do
        //   bloco anterior por uma operação exclusiva OR bit a bit.
        //   Isso garante que mesmo se o texto tiver muitos blocos idênticos,
        //   eles vão ter uma criptografica diferente.
        des.Mode = CipherMode.ECB;

        // Define as operações de criptografia (ICryptoTransform)
        transform = des.CreateDecryptor();
      }

      var buffer = sequence.ToArray();

      // Criptografa e retorna o array de bytes
      return transform.TransformFinalBlock(buffer, 0, buffer.Length);
    }

    #region Métodos de criptografia...

    /// <summary>
    /// Decripta a sequência de bytes com uma chave de criptografia.
    /// A sequência dever ter sido encriptada com a contra parte Processa.Net.Seguranca.Encriptador.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser decriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes decriptada.</returns>
    public static byte[] DecryptToBytes(IEnumerable<byte> sequence, IEnumerable<byte> keyPass)
    {
      return Process(sequence, keyPass);
    }

    /// <summary>
    /// Decripta a sequência de bytes com uma chave de criptografia.
    /// A sequência dever ter sido encriptada com a contra parte Processa.Net.Seguranca.Encriptador.
    /// </summary>
    /// <param name="base64Sequence">A sequência de bytes a ser decriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes decriptada.</returns>
    public static byte[] DecryptToBytes(string base64Sequence, IEnumerable<byte> keyPass)
    {
      return Process(FromBase64(base64Sequence), keyPass);
    }

    /// <summary>
    /// Decripta a sequência de bytes com uma chave de criptografia.
    /// A sequência dever ter sido encriptada com a contra parte Processa.Net.Seguranca.Encriptador.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser decriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes decriptada.</returns>
    public static byte[] DecryptToBytes(IEnumerable<byte> sequence, string keyPass)
    {
      return Process(sequence, FromString(keyPass));
    }

    /// <summary>
    /// Decripta a sequência de bytes com uma chave de criptografia.
    /// A sequência dever ter sido encriptada com a contra parte Processa.Net.Seguranca.Encriptador.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser decriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes decriptada.</returns>
    public static byte[] DecryptToBytes(string sequence, string keyPass)
    {
      return Process(FromBase64(sequence), FromString(keyPass));
    }

    /// <summary>
    /// Decripta a sequência de bytes com uma chave de criptografia.
    /// A sequência dever ter sido encriptada com a contra parte Processa.Net.Seguranca.Encriptador.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser decriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes decriptada.</returns>
    public static string DecryptToString(IEnumerable<byte> sequence, IEnumerable<byte> keyPass)
    {
      return GetString(Process(sequence, keyPass));
    }

    /// <summary>
    /// Decripta a sequência de bytes com uma chave de criptografia.
    /// A sequência dever ter sido encriptada com a contra parte Processa.Net.Seguranca.Encriptador.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser decriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes decriptada.</returns>
    public static string DecryptToString(string sequence, IEnumerable<byte> keyPass)
    {
      return GetString(Process(FromBase64(sequence), keyPass));
    }

    /// <summary>
    /// Decripta a sequência de bytes com uma chave de criptografia.
    /// A sequência dever ter sido encriptada com a contra parte Processa.Net.Seguranca.Encriptador.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser decriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes decriptada.</returns>
    public static string DecryptToString(IEnumerable<byte> sequence, string keyPass)
    {
      return GetString(Process(sequence, FromString(keyPass)));
    }

    /// <summary>
    /// Decripta a sequência de bytes com uma chave de criptografia.
    /// A sequência dever ter sido encriptada com a contra parte Processa.Net.Seguranca.Encriptador.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser decriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes decriptada.</returns>
    public static string DecryptToString(string sequence, string keyPass)
    {
      return GetString(Process(FromBase64(sequence), FromString(keyPass)));
    }

    #endregion
    
  }
}