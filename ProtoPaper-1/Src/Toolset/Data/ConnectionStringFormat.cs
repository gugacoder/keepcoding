using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toolset.Data
{
  [Flags]
  public enum ConnectionStringFormat
  {
    Default,
    Encrypted,
    Decrypted
  }
}