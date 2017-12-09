using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Toolset
{
  public static class ExceptionExtensions
  {
    public static string GetStackTrace(this Exception exception)
    {
      using (var output = new StringWriter())
      {
        Trace(exception, output);
        return output.ToString();
      }
    }

    public static void Debug(this Exception exception)
    {
      try
      {
        var stack = GetStackTrace(exception);
        System.Diagnostics.Debug.WriteLine(stack);
      }
      catch (Exception ex)
      {
        Dump(exception, ex);
      }
    }

    public static void Trace(this Exception exception)
    {
      try
      {
        var stack = GetStackTrace(exception);
        System.Diagnostics.Trace.TraceError(stack);
      }
      catch (Exception ex)
      {
        Dump(exception, ex);
      }
    }

    public static void TraceWarning(this Exception exception)
    {
      var stack = GetStackTrace(exception);
      try
      {
        System.Diagnostics.Trace.TraceWarning(stack);
      }
      catch (Exception ex)
      {
        Dump(exception, ex);
      }
    }

    public static void Trace(this Exception exception, string mensagem)
    {
      var stack = GetStackTrace(exception);
      try
      {
        System.Diagnostics.Trace.TraceError("{0}\nCausa:\n{1}", mensagem, stack);
      }
      catch (Exception ex)
      {
        Dump(exception, ex);
      }
    }

    public static void Trace(this Exception exception, string formato, params object[] args)
    {
      Trace(exception, string.Format(formato, args));
    }

    public static void TraceWarning(this Exception exception, string mensagem)
    {
      try
      {
        var stack = GetStackTrace(exception);
        System.Diagnostics.Trace.TraceWarning("{0}\nCausa:\n{1}", mensagem, stack);
      }
      catch (Exception ex)
      {
        Dump(exception, ex);
      }
    }

    public static void TraceWarning(this Exception exception, string formato, params object[] args)
    {
      TraceWarning(exception, string.Format(formato, args));
    }

    public static void Trace(this Exception exception, Stream output)
    {
      using (var writer = new StreamWriter(output))
      {
        Trace(exception, writer);
      }
    }

    public static void Trace(this Exception exception, TextWriter output)
    {
      output.Write("fault ");
      Exception ex = exception;
      do
      {
        output.WriteLine(ex.Message);
        output.Write(" type ");
        output.WriteLine(ex.GetType().FullName);

        var trace = ex.StackTrace;
        if (trace != null)
        {
          output.Write(trace);
          if (!trace.EndsWith(Environment.NewLine))
          {
            output.WriteLine();
          }
        }

        ex = ex.InnerException;
        if (ex != null)
        {
          output.Write("cause ");
        }
      } while (ex != null);
    }

    [Obsolete("Use Trace(...) em vez deste método.")]
    public static void Imprimir(this Exception exception, Stream output)
    {
      Trace(exception, output);
    }

    [Obsolete("Use Trace(...) em vez deste método.")]
    public static void Imprimir(this Exception exception, TextWriter output)
    {
      Trace(exception, output);
    }

    [Obsolete("Use Trace(...) em vez deste método.")]
    public static void Imprimir(this Exception exception)
    {
      Trace(exception);
    }

    /// <summary>
    /// Este método tenta registrar as exceções da forma possível.
    /// Deve ser usado em caso de falha do método geral de gravação de
    /// exceções.
    /// </summary>
    /// <param name="exceptions">As exceções a serem gravadas.</param>
    private static void Dump(params Exception[] exceptions)
    {
      foreach (var exception in exceptions)
      {
        try
        {
          var stack = GetStackTrace(exception);

          Console.Write("[FALHA]");
          Console.WriteLine(exception.Message);
          Console.WriteLine(stack);

          System.Diagnostics.Debug.Write("[FALHA]");
          System.Diagnostics.Debug.WriteLine(exception.Message);
          System.Diagnostics.Debug.WriteLine(stack);
        }
        catch
        {
          // nada a fazer
        }
      }
    }
  }
}