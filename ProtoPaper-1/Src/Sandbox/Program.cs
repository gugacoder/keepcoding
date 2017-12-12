using System;
using Toolset;

namespace Sandbox
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("olá, mundo!".ChangeCase(TextCase.Hyphenated));
    }
  }
}
