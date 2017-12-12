using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Toolset.Crypto
{
  /// <summary>
  /// Encriptador de texto.
  /// </summary>
  public static class Encrypter
  {
    private static Func<string, byte[]> FromString = Encoding.UTF8.GetBytes;
    private static Func<string, byte[]> FromBase64 = Convert.FromBase64String;
    private static Func<byte[], string> GetString = Encoding.UTF8.GetString;
    private static Func<byte[], string> GetBase64 = Convert.ToBase64String;

    /// <summary>
    /// Enccripta a sequência com uma chave de criptografia.
    /// É possível decriptar a sequência novamente com a chave usada na sua criptografia.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser encriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes encriptada.</returns>
    private static byte[] Process(IEnumerable<byte> sequence, IEnumerable<byte> keyPass)
    {
      // Inicia três iterações sucessivas do algoritmo DES. Ele pode usar dois ou três chaves de 56 bits.
      // Esse algoritmo oferece suporte comprimentos de keyPass de 128 bits para 192 bits em incrementos de 64 bits.
      ICryptoTransform transform;

      using (var des = new TripleDESCryptoServiceProvider())
      {
        using (var hashmd5 = new MD5CryptoServiceProvider())
        {
          des.Key = hashmd5.ComputeHash(keyPass.ToArray());
        }

        // Modo de operação do algoritmo simétrico:
        //   Cipher Block Chaining (CBC)
        //   Antes de Encrypt cada bloco de texto, ele é combinado com o texto cifrado do
        //   bloco anterior por uma operação exclusiva OR bit a bit.
        //   Isso garante que mesmo se o texto tiver muitos blocos idênticos,
        //   eles vão ter uma criptografica diferente.
        des.Mode = CipherMode.ECB;

        //Define as operações de criptografia (ICryptoTransform)
        transform = des.CreateEncryptor();
      }

      byte[] buffer = sequence.ToArray();

      //Criptografa e retorna o array de bytes
      return transform.TransformFinalBlock(buffer, 0, buffer.Length);
    }

    #region Métodos de criptografia...

    /// <summary>
    /// Enccripta a sequência com uma chave de criptografia.
    /// É possível decriptar a sequência novamente com a chave usada na sua criptografia.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser encriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes encriptada.</returns>
    public static byte[] EncryptToBytes(IEnumerable<byte> sequence, IEnumerable<byte> keyPass)
    {
      return Process(sequence, keyPass);
    }

    /// <summary>
    /// Enccripta a sequência com uma chave de criptografia.
    /// É possível decriptar a sequência novamente com a chave usada na sua criptografia.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser encriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes encriptada.</returns>
    public static byte[] EncryptToBytes(string sequence, IEnumerable<byte> keyPass)
    {
      return Process(FromString(sequence), keyPass);
    }

    /// <summary>
    /// Enccripta a sequência com uma chave de criptografia.
    /// É possível decriptar a sequência novamente com a chave usada na sua criptografia.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser encriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes encriptada.</returns>
    public static byte[] EncryptToBytes(IEnumerable<byte> sequence, string keyPass)
    {
      return Process(sequence, FromString(keyPass));
    }

    /// <summary>
    /// Enccripta a sequência com uma chave de criptografia.
    /// É possível decriptar a sequência novamente com a chave usada na sua criptografia.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser encriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes encriptada.</returns>
    public static byte[] EncryptToBytes(string sequence, string keyPass)
    {
      return Process(FromString(sequence), FromString(keyPass));
    }

    /// <summary>
    /// Enccripta a sequência com uma chave de criptografia.
    /// É possível decriptar a sequência novamente com a chave usada na sua criptografia.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser encriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes encriptada.</returns>
    public static string EncryptToString(IEnumerable<byte> sequence, IEnumerable<byte> keyPass)
    {
      return GetBase64(Process(sequence, keyPass));
    }

    /// <summary>
    /// Enccripta a sequência com uma chave de criptografia.
    /// É possível decriptar a sequência novamente com a chave usada na sua criptografia.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser encriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes encriptada.</returns>
    public static string EncryptToString(string sequence, IEnumerable<byte> keyPass)
    {
      return GetBase64(Process(FromString(sequence), keyPass));
    }

    /// <summary>
    /// Enccripta a sequência com uma chave de criptografia.
    /// É possível decriptar a sequência novamente com a chave usada na sua criptografia.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser encriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes encriptada.</returns>
    public static string EncryptToString(IEnumerable<byte> sequence, string keyPass)
    {
      return GetBase64(Process(sequence, FromString(keyPass)));
    }

    /// <summary>
    /// Enccripta a sequência com uma chave de criptografia.
    /// É possível decriptar a sequência novamente com a chave usada na sua criptografia.
    /// </summary>
    /// <param name="sequence">A sequência de bytes a ser encriptada.</param>
    /// <param name="keyPass">A chave de criptografia.</param>
    /// <returns>A sequência de bytes encriptada.</returns>
    public static string EncryptToString(string sequence, string keyPass)
    {
      return GetBase64(Process(FromString(sequence), FromString(keyPass)));
    }

    #endregion
  }
}