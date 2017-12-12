using System;

namespace Toolset.Standard
{
  public interface IRet
  {
    bool Ok { get; }

    string FaultCode { get; }

    string FaultReason { get; }
  }

  public interface IRet<T> : IRet
  {
    T Data { get; }
  }
}