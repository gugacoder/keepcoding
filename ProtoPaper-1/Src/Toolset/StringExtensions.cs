using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toolset
{
  /// <summary>
  /// Coleção de extensões para o tipo string.
  /// </summary>
  public static class StringExtensions
  {
    /// <summary>
    /// Determina se o texto é formado apenas por números.
    /// </summary>
    /// <param name="text">O texto a ser validado.</param>
    /// <returns>
    /// Verdadeiro caso o texto seja composto apenas de números.
    /// </returns>
    public static bool IsNumeric(this string text)
    {
      return text.All(char.IsNumber);
    }

    /// <summary>
    /// Determina se o texto corresponde a um número.
    /// </summary>
    /// <param name="text">O texto a ser validado.</param>
    /// <returns>
    /// Verdadeiro caso o texto seja composto apenas de números e
    /// separador de decimal.
    /// </returns>
    public static bool IsNumber(this string text)
    {
      return text.All(c => char.IsNumber(c) || char.IsDigit(c));
    }

    /// <summary>
    /// Coloca o texto entre aspas a menos que o texto representa
    /// um número de até 9 dígitos.
    /// </summary>
    /// <param name="text">O texto a ser colocado entre aspas.</param>
    /// <returns>O texto encapsulado entre aspas.</returns>
    public static string Quote(this string text)
    {
      return Quote(text, '"', false);
    }

    /// <summary>
    /// Coloca o texto entre aspas a menos que o texto representa
    /// um número de até 9 dígitos.
    /// </summary>
    /// <param name="text">O texto a ser colocado entre aspas.</param>
    /// <param name="character">O caracter para encapsulamento do texto.</param>
    /// <returns>O texto encapsulado entre aspas.</returns>
    public static string Quote(this string text, char character)
    {
      return Quote(text, '"');
    }

    /// <summary>
    /// Coloca o texto entre aspas a menos que o texto representa
    /// um número de até 9 dígitos.
    /// </summary>
    /// <param name="text">O texto a ser colocado entre aspas.</param>
    /// <param name="force">
    /// Ativa ou desativa o encapsulamento entre aspas para qualquer
    /// texto, mesmo quando o texto representar um número com até
    /// 9 digitos.
    /// </param>
    /// <returns>O texto encapsulado entre aspas.</returns>
    public static string Quote(this string text, bool force)
    {
      return Quote(text, '"', force);
    }

    /// <summary>
    /// Coloca o texto entre aspas a menos que o texto representa
    /// um número de até 9 dígitos.
    /// </summary>
    /// <param name="text">O texto a ser colocado entre aspas.</param>
    /// <param name="character">O caracter para encapsulamento do texto.</param>
    /// <param name="force">
    /// Ativa ou desativa o encapsulamento entre aspas para qualquer
    /// texto, mesmo quando o texto representar um número com até
    /// 9 digitos.
    /// </param>
    /// <returns>O texto encapsulado entre aspas.</returns>
    public static string Quote(this string text, char character, bool force)
    {
      if (force
      || (text.IsNumeric() && text.Length > 9)
      || (!text.IsNumber()))
      {
        return $"{character}{text}{character}";
      }
      return text;
    }

    public static string ChangeCase(this string sentence, TextCase textCase)
    {
      if (textCase == TextCase.Default || textCase == TextCase.KeepOriginal)
        return sentence;

      var words = EnumerateWords(sentence);
      if (!words.Any())
        return string.Empty;

      var properCaseMask = TextCase.UpperCase | TextCase.LowerCase | TextCase.ProperCase;
      var properCase = textCase & properCaseMask;

      var delimiterMask = TextCase.Hyphenated | TextCase.Underscore | TextCase.Dotted | TextCase.Spaced | TextCase.Joined;
      var delimiter = textCase & delimiterMask;

      // camel case recebe tratamento especial por este algoritmo
      var camelCaseMask = (int)(TextCase.CamelCase ^ TextCase.ProperCase ^ TextCase.Joined);
      var isCamelCase = (((int)textCase) & camelCaseMask) != 0;

      switch (properCase)
      {
        case TextCase.UpperCase:
          words = words.Select(word => word.ToUpper());
          break;

        case TextCase.ProperCase:
          words = words.Select(word => char.ToUpper(word[0]) + word.Substring(1));
          break;
      }

      if (isCamelCase)
      {
        var primeiro = words.Take(1).Select(x => x.ToLower());
        var outros = words.Skip(1);
        words = primeiro.Union(outros);
      }

      switch (delimiter)
      {
        case TextCase.Hyphenated:
          return string.Join("-", words);

        case TextCase.Underscore:
          return string.Join("_", words);

        case TextCase.Dotted:
          return string.Join(".", words);

        case TextCase.Joined:
          return string.Concat(words);

        case TextCase.Spaced:
        default:
          return string.Join(" ", words);
      }
    }

    public static IEnumerable<string> EnumerateWords(string sentence)
    {
      var chars = ReplaceDelimiters(sentence);
      var phrase = new string(chars.ToArray());
      return phrase.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x));
    }

    private static IEnumerable<char> ReplaceDelimiters(string sentence)
    {
      var delimiters = new[] { '_', '-', '.', ':' };

      var priorCharacter = '\x0';
      foreach (var character in sentence.Trim())
      {
        if (delimiters.Contains(character))
        {
          yield return ' ';
        }
        else if (char.IsNumber(character))
        {
          if (!char.IsNumber(priorCharacter))
          {
            yield return ' ';
          }
          yield return character;
        }
        else if (char.IsUpper(character))
        {
          if (!char.IsUpper(priorCharacter))
          {
            yield return ' ';
          }
          yield return char.ToLower(character);
        }
        else
        {
          yield return character;
        }
        priorCharacter = character;
      }
    }
  }
}
