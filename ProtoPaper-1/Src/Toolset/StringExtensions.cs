using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toolset
{
  public static class StringExtensions
  {
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
