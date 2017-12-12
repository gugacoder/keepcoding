using System;
using System.Text;

namespace Toolset.Standard
{
  public class Ret : IRet
  {
    private bool? _ok;
    private string _faultCode;
    private string _faultReason;

    public bool Ok
    {
      get
      {
        if (_ok != null) return _ok.Value;
        return FaultCode == null && FaultReason == null;
      }
      set { _ok = value; }
    }

    public string FaultCode
    {
      get
      {
        if (_faultCode != null) return _faultCode;
        if (_faultReason != null) return _faultReason.ChangeCase(TextCase.Hyphenated);
        return null;
      }
      set { _faultCode = value; }
    }

    public string FaultReason
    {
      get
      {
        if (_faultReason != null) return _faultReason;
        if (_faultCode != null) return _faultCode.ChangeCase(TextCase.PascalCase);
        return null;
      }
      set { _faultReason = value; }
    }

    #region Succeed

    public static Ret Succeed()
    {
      return new Ret();
    }

    public static Ret<T> Succeed<T>(T data)
    {
      return new Ret<T> { Data = data };
    }

    #endregion

    #region Fail

    public static Ret Fail(string code)
    {
      return new Ret { FaultCode = code };
    }

    public static Ret Fail(string code, string reason)
    {
      return new Ret { FaultCode = code, FaultReason = reason };
    }

    public static Ret Fail(Exception exception)
    {
      return Fail("internal-server-error", exception);
    }

    public static Ret Fail(string code, Exception exception)
    {
      var builder = new StringBuilder();
      while (exception != null)
      {
        if (builder.Length > 0)
        {
          builder.AppendLine();
        }
        builder.Append(exception.Message);
        exception = exception.InnerException;
      }
      return new Ret { FaultCode = code, FaultReason = builder.ToString() };
    }

    #endregion

    #region Fail<T>

    public static Ret<T> Fail<T>(string code)
    {
      return new Ret<T> { FaultCode = code };
    }

    public static Ret<T> Fail<T>(string code, string reason)
    {
      return new Ret<T> { FaultCode = code, FaultReason = reason };
    }

    public static Ret<T> Fail<T>(Exception exception)
    {
      return Fail<T>("internal-server-error", exception);
    }

    public static Ret<T> Fail<T>(string code, Exception exception)
    {
      var builder = new StringBuilder();
      while (exception != null)
      {
        if (builder.Length > 0)
        {
          builder.AppendLine();
        }
        builder.Append(exception.Message);
        exception = exception.InnerException;
      }
      return new Ret<T> { FaultCode = code, FaultReason = builder.ToString() };
    }

    #endregion
  }

  public class Ret<T> : Ret, IRet, IRet<T>
  {
    public T Data { get; set; }
  }
}