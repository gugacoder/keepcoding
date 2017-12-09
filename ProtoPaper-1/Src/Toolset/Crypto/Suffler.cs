using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Security;

namespace Toolset.Crypto
{
  // shuffler de strings.
  //
  // O shuffler produz quatro caracteres para cada caracter encontrado na sequência
  // original e posiciona o caracter original aleatoriamente num destes quatro caracteres.
  // O algoritmo usa um `seed` de zero no randomizador para produzir uma sequência
  // previsível de números aleatórios.
  //
  // Esta classe intencionalmente não possui documentação com metatags C#.
  public static class Shuffler
  {
    // Embaralha a coleção de bytes.
    // O embaralhamento tem quatro vezes o tamanho da sequência oridinal e contém os caracteres
    // da sequência distribuído aleatoriamente.
    internal static IEnumerable<byte> Shuffle(IEnumerable<byte> sequence)
    {
      var code = Encrypter.EncryptToBytes(sequence, Assembly.GetExecutingAssembly().GetName().Name);
      var indexer = new Random(0);
      var shuffler = new Random(0);
      var shuffled =
        code.SelectMany(
          x =>
          {
            var index = indexer.Next(4);
            var vector = Enumerable.Range(0, 4).Select(n => code[shuffler.Next(code.Length)]).ToArray();
            vector[index] = x;
            return vector;
          }
        ).ToArray();
      return shuffled;
    }

    // Extrai a sequence do conteúdo shuffled.
    // O conteúdo shuffled tem quatro vezes o tamanho da sequence original com seus
    // caracteres distribuídos aleatoriamente.
    internal static IEnumerable<byte> Deshuffle(IEnumerable<byte> shuffled)
    {
      var indexer = new Random(0);
      var blocks = Enumerable.Range(0, shuffled.Count() / 4).Select(i => shuffled.Skip(i * 4).Take(4).ToArray());
      var code =
        blocks.Select(
          vector =>
          {
            var index = indexer.Next(4);
            return vector[index];
          }
        ).ToArray();
      var sequence = Decrypter.DecryptToBytes(code, Assembly.GetExecutingAssembly().GetName().Name);
      return sequence;
    }

    #region Algoritmos extras...

    // Embaralha a coleção de bytes.
    // O embaralhamento tem quatro vezes o tamanho da sequência oridinal e contém os caracteres
    // da sequência distribuído aleatoriamente.
    internal static IEnumerable<byte> Shuffle(string sequence)
    {
      return Shuffle(Encoding.UTF8.GetBytes(sequence));
    }

    // Extrai a sequence do conteúdo shuffled.
    // O conteúdo shuffled tem quatro vezes o tamanho da sequence original com seus
    // caracteres distribuídos aleatoriamente.
    //
    // É esperado que a string esteja formatada na base 64.
    internal static IEnumerable<byte> Deshuffle(string shuffled)
    {
      return Deshuffle(Convert.FromBase64String(shuffled));
    }

    // Embaralha a coleção de bytes.
    // O embaralhamento tem quatro vezes o tamanho da sequência oridinal e contém os caracteres
    // da sequência distribuído aleatoriamente.
    internal static string SuffleToString(IEnumerable<byte> sequence)
    {
      var bytes = Shuffle(sequence);
      return Convert.ToBase64String(bytes.ToArray());
    }

    // Embaralha a coleção de bytes.
    // O embaralhamento tem quatro vezes o tamanho da sequência oridinal e contém os caracteres
    // da sequência distribuído aleatoriamente.
    internal static string SuffleToString(string sequence)
    {
      var bytes = Shuffle(Encoding.UTF8.GetBytes(sequence));
      return Convert.ToBase64String(bytes.ToArray());
    }

    // Embaralha a coleção de bytes.
    // O embaralhamento tem quatro vezes o tamanho da sequência oridinal e contém os caracteres
    // da sequência distribuído aleatoriamente.
    internal static string DeshuffleToString(IEnumerable<byte> shuffled)
    {
      var bytes = Deshuffle(shuffled);
      return Encoding.UTF8.GetString(bytes.ToArray());
    }

    // Embaralha a coleção de bytes.
    // O embaralhamento tem quatro vezes o tamanho da sequência oridinal e contém os caracteres
    // da sequência distribuído aleatoriamente.
    // É esperado que a string esteja formatada na base 64.
    internal static string DeshuffleToString(string shuffled)
    {
      var bytes = Deshuffle(Convert.FromBase64String(shuffled));
      return Encoding.UTF8.GetString(bytes.ToArray());
    }

    #endregion
  }
}