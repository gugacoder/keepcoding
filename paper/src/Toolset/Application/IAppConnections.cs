using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolset.Data;

namespace Toolset.Application
{
  public interface IAppConnections
  {
    ConnectionString this[string connectionName] { get; }
  }
}
